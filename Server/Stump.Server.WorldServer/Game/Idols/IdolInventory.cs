using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Parties;
using Stump.Server.WorldServer.Handlers.Idols;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Idols
{
    public sealed class IdolInventory
    {
        [Variable]
        const int MaxActiveIdols = 6;

        public IdolInventory(Character owner)
        {
            Owner = owner;
            ActiveIdols = owner.Record.Idols.Select(x => IdolManager.Instance.CreatePlayerIdol(Owner, x)).Where(x => x != null).ToList();

            Owner.Inventory.ItemRemoved += OnInventoryItemRemoved;
        }

        public IdolInventory(Party party)
        {
            Party = party;
            ActiveIdols = new List<PlayerIdol>();

            BindEvents(party);
        }

        private void OnPartyDeleted(Party party)
        {
            UnBindEvents(party);
        }

        public Party Party
        {
            get;
        }

        public bool IsPartyIdols
        {
            get { return Party != null; }
        }

        public Character Owner
        {
            get;
            private set;
        }

        private List<PlayerIdol> ActiveIdols
        {
            get;
            set;
        }

        public bool HasIdol(int templateId) => ActiveIdols.Any(x => x.Template.Id == templateId);

        public void Add(short idolId)
        {
            var owner = Owner;

            if (IsPartyIdols)
            {
                var partyIdol = GetPartyIdol(idolId);

                if (partyIdol == null)
                    return;

                owner = Party.GetMember((int)partyIdol.ownersIds.First());
            }

            var idol = IdolManager.Instance.CreatePlayerIdol(owner, idolId);

            if (idol == null)
                return;

            if (HasIdol(idol.Id))
                return;

            if (ActiveIdols.Count >= MaxActiveIdols)
                return;

            if (!owner.Inventory.HasItem(idol.Template.IdolItem))
                return;

            ActiveIdols.Add(idol);
            IdolHandler.SendIdolSelectedMessage(IsPartyIdols ? Party.Clients : Owner.Client, true, IsPartyIdols, (short)idol.Id);

            if (!owner.IsInFight() || owner.Fight.State != FightState.Placement)
                return;

            IdolHandler.SendIdolFightPreparationUpdate(Owner.Fight.Clients, ActiveIdols.Select(x => x.GetNetworkIdol()));
        }

        public bool Remove(short idolId)
        {
            var idol = ActiveIdols.FirstOrDefault(x => x.Id == idolId);

            if (idol == null)
                return false;

            return Remove(idol);
        }

        public bool Remove(PlayerIdol idol)
        {
            var result = ActiveIdols.Remove(idol);

            if (!result)
                return false;

            IdolHandler.SendIdolSelectedMessage(IsPartyIdols ? Party.Clients : Owner.Client, false, IsPartyIdols, (short)idol.Id);

            if (!Owner.IsInFight() || Owner.Fight.State != FightState.Placement)
                return true;

            IdolHandler.SendIdolFightPreparationUpdate(Owner.Fight.Clients, ActiveIdols.Select(x => x.GetNetworkIdol()));

            return true;
        }

        public IEnumerable<PlayerIdol> GetIdols()
        {
            return ActiveIdols.ToArray();
        }

        public PartyIdol GetPartyIdol(short idolId)
        {
            return GetPartyIdols().FirstOrDefault(x => x.id == idolId);
        }

        public IEnumerable<PartyIdol> GetPartyIdols()
        {
            var idolItems = Party.Members.SelectMany(x => x.Inventory.GetItems(y => y.Template.TypeId == (uint)ItemTypeEnum.IDOLE)).ToArray();
            var partyIdols = new Dictionary<int, PartyIdol>();

            foreach (var idolItem in idolItems)
            {
                var template = IdolManager.Instance.GetTemplateByItemId(idolItem.Template.Id);
                var ownerIds = new List<long>();

                if (partyIdols.TryGetValue(template.Id, out var partyIdol))
                {
                    ownerIds.AddRange(partyIdol.ownersIds);
                    partyIdols.Remove(partyIdol.id);
                }

                ownerIds.Add(idolItem.Owner.Id);
                partyIdol = new PartyIdol((short)template.Id, (short)template.ExperienceBonus, (short)template.DropBonus, ownerIds);

                partyIdols.Add(template.Id, partyIdol);
            }

            return partyIdols.Values;
        }

        private bool CanUseIdol(PlayerIdol idol, FightPvM fight)
        {
            if (!idol.Template.IdolSpellId.HasValue)
                return false;
            else if (ActiveIdols.Count(x => x.Id == idol.Id) > 1)
                return false;
            else if (fight.MonsterTeam.Fighters.OfType<MonsterFighter>().Any(x => idol.Template.IncompatibleMonsters.Contains(x.Monster.Template.Id)))
                return false;
            else if (fight.MonsterTeam.Fighters.OfType<MonsterFighter>().Any(x => x.Monster.Template.AllIdolsDisabled))
                return false;
            else if (idol.Template.GroupOnly && (fight.PlayerTeam.Fighters.Count < 4 || fight.MonsterTeam.Fighters.Count < 4))
                return false;
            else if (!idol.Owner.Inventory.HasItem(idol.Template.IdolItem))
                return false;
            else if (!idol.Owner.IsInFight() || idol.Owner.Fight.Id != fight.Id)
                return false;

            return true;
        }

        public IEnumerable<PlayerIdol> ComputeIdols(FightPvM fight)
        {
            foreach (var idol in ActiveIdols.ToArray())
            {
                if (CanUseIdol(idol, fight))
                    continue;

                Remove(idol);
            }

            if (ActiveIdols.Count > MaxActiveIdols)
                return new PlayerIdol[0];

            return GetIdols();
        }

        #region Events

        private void BindEvents(Party party)
        {
            party.LeaderChanged += OnPartyLeaderChanged;
            party.MemberRemoved += OnPartyMemberRemoved;
            party.GuestPromoted += OnPartyGuestPromoted;
            party.PartyDeleted += OnPartyDeleted;
        }

        private void UnBindEvents(Party party)
        {
            party.LeaderChanged -= OnPartyLeaderChanged;
            party.MemberRemoved -= OnPartyMemberRemoved;
            party.GuestPromoted -= OnPartyGuestPromoted;
            party.PartyDeleted -= OnPartyDeleted;
        }

        private void OnPartyGuestPromoted(Party party, Character member)
        {
            member.Inventory.ItemRemoved += OnInventoryItemRemoved;
        }

        private void OnPartyMemberRemoved(Party party, Character member, bool kicked)
        {
            member.Inventory.ItemRemoved -= OnInventoryItemRemoved;
        }

        private void OnPartyLeaderChanged(Party party, Character leader)
        {
            Owner = leader;
        }

        private void OnInventoryItemRemoved(ItemsCollection<BasePlayerItem> sender, BasePlayerItem item)
        {
            foreach (var idol in ActiveIdols.Where(x => x.Template.IdolItemId == item.Template.Id).ToArray())
            {
                if (idol.Owner.Inventory.HasItem(idol.Template.IdolItem))
                    continue;

                Remove(idol);

                if (IsPartyIdols)
                    IdolHandler.SendIdolPartyLostMessage(Party.Clients, (short)idol.Id);
            }
        }

        #endregion Events

        #region Save

        public void Save()
        {
            Owner.Record.Idols = ActiveIdols.Select(x => x.Id).ToList();
        }

        #endregion Save
    }
}
