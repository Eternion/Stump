using Stump.Core.Attributes;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
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

        private void OnInventoryItemRemoved(ItemsCollection<BasePlayerItem> sender, BasePlayerItem item)
        {
            foreach (var idol in ActiveIdols.Where(x => x.Template.IdolItemId == item.Template.Id).ToArray())
            {
                if (Owner.Inventory.HasItem(idol.Template.IdolItem))
                    continue;

                Remove(idol);
            }
        }

        public Character Owner
        {
            get;
        }

        private List<PlayerIdol> ActiveIdols
        {
            get;
            set;
        }

        public bool HasIdol(int templateId) => ActiveIdols.Any(x => x.Template.Id == templateId);

        public void Add(short idolId)
        {
            var idol = IdolManager.Instance.CreatePlayerIdol(Owner, idolId);

            if (idol == null)
                return;

            if (HasIdol(idol.Id))
                return;

            if (ActiveIdols.Count >= MaxActiveIdols)
                return;

            if (!Owner.Inventory.HasItem(idol.Template.IdolItem))
                return;

            ActiveIdols.Add(idol);
            IdolHandler.SendIdolSelectedMessage(Owner.Client, true, false, (short)idol.Id);

            if (!Owner.IsInFight() || Owner.Fight.State != FightState.Placement)
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

            IdolHandler.SendIdolSelectedMessage(Owner.Client, false, false, (short)idol.Id);

            if (!Owner.IsInFight() || Owner.Fight.State != FightState.Placement)
                return true;

            IdolHandler.SendIdolFightPreparationUpdate(Owner.Fight.Clients, ActiveIdols.Select(x => x.GetNetworkIdol()));

            return true;
        }

        public IEnumerable<PlayerIdol> GetIdols()
        {
            return ActiveIdols.ToArray();
        }

        private bool CanUseIdol(PlayerIdol idol, FightPvM fight)
        {
            if (!idol.Template.IdolSpellId.HasValue)
                return false;
            else if (ActiveIdols.Count(x => x.Id == idol.Id) > 1)
                return false;
            else if (fight.MonsterTeam.Fighters.OfType<MonsterFighter>().Any(x => idol.Template.IncompatibleMonsters.Contains(x.Monster.Template.Id)))
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

        public void Save()
        {
            Owner.Record.Idols = ActiveIdols.Select(x => x.Id).ToList();
        }
    }
}
