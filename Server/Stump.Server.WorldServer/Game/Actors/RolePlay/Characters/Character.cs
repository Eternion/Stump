using MongoDB.Bson;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.BaseServer.Logging;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Database.Breeds;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Accounts;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Arena;
using Stump.Server.WorldServer.Game.Breeds;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Game.Dialogs.Interactives;
using Stump.Server.WorldServer.Game.Dialogs.Merchants;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Game.Exchanges;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Game.Items.BidHouse;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Items.Player.Custom;
using Stump.Server.WorldServer.Game.Jobs;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Game.Notifications;
using Stump.Server.WorldServer.Game.Parties;
using Stump.Server.WorldServer.Game.Shortcuts;
using Stump.Server.WorldServer.Game.Social;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.Server.WorldServer.Handlers.Chat;
using Stump.Server.WorldServer.Handlers.Compass;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Handlers.Context.RolePlay.Party;
using Stump.Server.WorldServer.Handlers.Guilds;
using Stump.Server.WorldServer.Handlers.Initialization;
using Stump.Server.WorldServer.Handlers.Interactives;
using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Server.WorldServer.Handlers.Moderation;
using Stump.Server.WorldServer.Handlers.Titles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Database.Quests;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Quests;
using Stump.Server.WorldServer.Database.Mounts;
using Stump.Server.WorldServer.Database.Social;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Maps.Spawns;
using Stump.Server.WorldServer.Handlers.Mounts;
using GuildMember = Stump.Server.WorldServer.Game.Guilds.GuildMember;
using Stump.Server.WorldServer.Game.Exchanges.Paddock;
using Stump.Server.WorldServer.Game.Idols;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Characters
{
    public sealed class Character : Humanoid, IStatsOwner, IInventoryOwner, ICommandsUser
    {
        [Variable]
        public static ushort HonorLimit = 20000;

        readonly CharacterRecord m_record;
        bool m_recordLoaded;

        public Character(CharacterRecord record, WorldClient client)
        {
            m_record = record;
            Client = client;
            SaveSync = new object();
            LoggoutSync = new object();
            Status = new PlayerStatus((sbyte)PlayerStatusEnum.PLAYER_STATUS_AVAILABLE);
        }

        #region Events

        public event Action<Character> LoggedIn;

        void OnLoggedIn()
        {
            if (GuildMember != null)
            {
                GuildMember.OnCharacterConnected(this);

                if (Guild.MotdContent != null)
                    GuildHandler.SendGuildMotdMessage(Client, Guild);

                if (Guild.BulletinContent != null)
                    GuildHandler.SendGuildBulletinMessage(Client, Guild);
            }
            else
                RemoveEmote(EmotesEnum.EMOTE_GUILD);

            //Arena
            CheckArenaDailyProperties();

            if (PrestigeRank > 0 && PrestigeManager.Instance.PrestigeEnabled)
            {
                var item = GetPrestigeItem();
                if (item == null)
                    CreatePrestigeItem();
                else
                {
                    item.UpdateEffects();
                    Inventory.RefreshItem(item);
                }
                RefreshStats();
            }
            else
            {
                var item = GetPrestigeItem();
                if (item != null)
                    Inventory.RemoveItem(item, true);
            }

            OnPlayerLifeStatusChanged(PlayerLifeStatus);

            if (!IsGhost())
            {
                var energyGain = (short)(DateTime.Now - Record.LastUsage.Value).TotalMinutes;

                energyGain = (short)((Energy + energyGain) > EnergyMax ? (EnergyMax - Energy) : energyGain);

                if (energyGain <= 0)
                    return;

                Energy += energyGain;

                RefreshStats();

                //Vous avez récupéré <b>%1</b> points d'énergie.
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 7, energyGain);
            }

            Record.LastUsage = DateTime.Now;

            var document = new BsonDocument
            {
                { "AcctId", Account.Id },
                { "AcctName", Account.Login },
                { "CharacterId", Id },
                { "CharacterName", Name },
                { "IPAddress", Client.IP },
                { "Action", "Login" },
                { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
            };

            MongoLogger.Instance.Insert("characters_connections", document);

            LoggedIn?.Invoke(this);
        }

        public event Action<Character> LoggedOut;

        void OnLoggedOut()
        {
            EnterMap -= OnFollowedMemberEnterMap;

            if (FriendsBook != null)
                FriendsBook.CheckDC(); // attempt to resolve leaks

            if (Fight != null && (Fight.State == FightState.Placement || Fight.State == FightState.Fighting))
                Record.LeftFightId = Fight.Id;
            else
                Record.LeftFightId = null;

            if (GuildMember != null)
                GuildMember.OnCharacterDisconnected(this);

            if (TaxCollectorDefendFight != null)
                TaxCollectorDefendFight.RemoveDefender(this);

            if (ArenaManager.Instance.IsInQueue(this))
                ArenaManager.Instance.RemoveFromQueue(this);

            if (ArenaPopup != null)
                ArenaPopup.Deny();

            if (Jobs != null)
                foreach (var job in Jobs.Where(x => x.IsIndexed))
                    job.Template.RemoveAvaiableCrafter(this);

            var document = new BsonDocument
            {
                { "AcctId", Client.Account.Id },
                { "AcctName", Client.Account.Login },
                { "CharacterId", Id },
                { "CharacterName", Name },
                { "IPAddress", Client.IP },
                { "Action", "Loggout" },
                { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
            };

            MongoLogger.Instance.Insert("characters_connections", document);

            Record.LastUsage = DateTime.Now;

            LoggedOut?.Invoke(this);
        }

        public event Action<Character> Saved;

        public void OnSaved()
        {
            IsAuthSynced = true;
            UnBlockAccount();

            Saved?.Invoke(this);
        }

        public event Action<Character, int> LifeRegened;

        private void OnLifeRegened(int regenedLife)
        {
            LifeRegened?.Invoke(this, regenedLife);
        }

        public event Action<Character> AccountUnblocked;

        private void OnAccountUnblocked()
        {
            AccountUnblocked?.Invoke(this);
        }

        public event Action<Character> LookRefreshed;

        private void OnLookRefreshed()
        {
            LookRefreshed?.Invoke(this);
        }

        public event Action<Character> StatsResfreshed;

        private void OnStatsResfreshed()
        {
            StatsResfreshed?.Invoke(this);
        }

        public event Action<Character, Npc, NpcActionTypeEnum, NpcAction> InteractingWith;

        public void OnInteractingWith(Npc npc, NpcActionTypeEnum actionType, NpcAction action)
        {
            InteractingWith?.Invoke(this, npc, actionType, action);
        }
        #endregion Events

        #region Properties

        public WorldClient Client
        {
            get;
        }

        public AccountData Account
        {
            get { return Client.Account; }
        }

        public WorldAccount WorldAccount
        {
            get { return Client.WorldAccount; }
        }


        public UserGroup UserGroup
        {
            get
            {
                return Client.UserGroup;
            }
        }

        public object SaveSync
        {
            get;
            private set;
        }

        public object LoggoutSync
        {
            get;
            private set;
        }

        private bool m_inWorld;

        public override bool IsInWorld
        {
            get
            {
                return m_inWorld;
            }
        }

        public CharacterMerchantBag MerchantBag
        {
            get;
            private set;
        }

        #region Identifier

        public override string Name
        {
            get { return m_record.Name; }
            protected set
            {
                m_record.Name = value;
                base.Name = value;
            }
        }

        public override int Id
        {
            get { return m_record.Id; }
            protected set
            {
                m_record.Id = value;
                base.Id = value;
            }
        }

        #endregion Identifier

        #region Inventory

        public Inventory Inventory
        {
            get;
            private set;
        }

        public int Kamas
        {
            get { return Record.Kamas; }
            set { Record.Kamas = value; }
        }

        #endregion Inventory

        #region Jobs

        public JobsCollection Jobs
        {
            get;
            private set;
        }

        #endregion Jobs

        #region Interactives

        public InteractiveObject CurrentUsedInteractive => CurrentUsedSkill?.InteractiveObject;

        public Skill CurrentUsedSkill
        {
            get;
            private set;
        }

        public void SetCurrentSkill(Skill skill)
        {
            CurrentUsedSkill = skill;
        }

        public void ResetCurrentSkill()
        {
            CurrentUsedSkill = null;
        }

        #endregion

        #region Position

        public override ICharacterContainer CharacterContainer
        {
            get
            {
                if (IsFighting())
                    return Fight;

                return Map;
            }
        }

        #endregion Position

        #region Dialog

        private IDialoger m_dialoger;

        public IDialoger Dialoger
        {
            get { return m_dialoger; }
            private set
            {
                m_dialoger = value;
                m_dialog = value != null ? m_dialoger.Dialog : null;
            }
        }

        private IDialog m_dialog;

        public IDialog Dialog
        {
            get { return m_dialog; }
            private set
            {
                m_dialog = value;
                if (m_dialog == null)
                    m_dialoger = null;
            }
        }

        public NpcShopDialogLogger NpcShopDialog => Dialog as NpcShopDialogLogger;

        public ZaapDialog ZaapDialog => Dialog as ZaapDialog;

        public ZaapiDialog ZaapiDialog => Dialog as ZaapiDialog;

        public MerchantShopDialog MerchantShopDialog => Dialog as MerchantShopDialog;

        public RequestBox RequestBox
        {
            get;
            private set;
        }

        public void SetDialoger(IDialoger dialoger)
        {
            if (Dialog != null)
                Dialog.Close();

            Dialoger = dialoger;
        }

        public void SetDialog(IDialog dialog)
        {
            if (Dialog != null)
                Dialog.Close();

            Dialog = dialog;
        }

        public void CloseDialog(IDialog dialog)
        {
            if (Dialog == dialog)
                Dialoger = null;
        }

        public void ResetDialog()
        {
            Dialoger = null;
        }

        public void OpenRequestBox(RequestBox request)
        {
            RequestBox = request;
        }

        public void ResetRequestBox()
        {
            RequestBox = null;
        }

        public bool IsBusy() => IsInRequest() || IsDialoging();

        public bool IsDialoging() => Dialog != null;

        public bool IsInRequest() => RequestBox != null;

        public bool IsRequestSource() => IsInRequest() && RequestBox.Source == this;

        public bool IsRequestTarget() => IsInRequest() && RequestBox.Target == this;

        public bool IsTalkingWithNpc() => Dialog is NpcDialog;

        public bool IsInZaapDialog() => Dialog is ZaapDialog;

        public bool IsInZaapiDialog() => Dialog is ZaapiDialog;

        #endregion Dialog

        #region Party

        private readonly Dictionary<int, PartyInvitation> m_partyInvitations
            = new Dictionary<int, PartyInvitation>();

        private readonly List<PartyTypeEnum> m_partiesLoyalTo = new List<PartyTypeEnum>();

        private Character m_followedCharacter;

        private Party m_party;
        private ArenaParty m_arenaParty;

        public Party Party
        {
            get { return m_party; }
            private set
            {
                if (m_party != null && value != m_party) SetLoyalToParty(PartyTypeEnum.PARTY_TYPE_CLASSICAL, false);
                m_party = value;
            }
        }

        public ArenaParty ArenaParty
        {
            get { return m_arenaParty; }
            private set
            {
                if (m_arenaParty != null && value != m_arenaParty) SetLoyalToParty(PartyTypeEnum.PARTY_TYPE_ARENA, false);
                m_arenaParty = value;
            }
        }

        public Party[] Parties => new[] { Party, ArenaParty }.Where(x => x != null).ToArray();

        public bool IsInParty()
        {
            return Party != null || ArenaParty != null;
        }

        public bool IsInParty(int id)
        {
            return (Party != null && Party.Id == id) || (ArenaParty != null && ArenaParty.Id == id);
        }

        public bool IsInParty(PartyTypeEnum type)
        {
            return (type == PartyTypeEnum.PARTY_TYPE_CLASSICAL && Party != null) || (type == PartyTypeEnum.PARTY_TYPE_ARENA && ArenaParty != null);
        }

        public bool IsPartyLeader()
        {
            return Party?.Leader == this;
        }

        public bool IsPartyLeader(int id)
        {
            return GetParty(id)?.Leader == this;
        }

        public Party GetParty(int id)
        {
            if (Party != null && Party.Id == id)
                return Party;

            if (ArenaParty != null && ArenaParty.Id == id)
                return ArenaParty;

            return null;
        }

        public bool IsLoyalToParty(PartyTypeEnum type) => m_partiesLoyalTo.Contains(type);
        public void SetLoyalToParty(PartyTypeEnum type, bool loyal)
        {
            if (loyal) m_partiesLoyalTo.Add(type); else m_partiesLoyalTo.Remove(type);

            PartyHandler.SendPartyLoyaltyStatusMessage(Client, GetParty(type), loyal);
        }

        public Party GetParty(PartyTypeEnum type)
        {
            switch (type)
            {
                case PartyTypeEnum.PARTY_TYPE_CLASSICAL:
                    return Party;

                case PartyTypeEnum.PARTY_TYPE_ARENA:
                    return ArenaParty;

                default:
                    throw new NotImplementedException(string.Format("Cannot manage party of type {0}", type));
            }
        }

        public void SetParty(Party party)
        {
            switch (party.Type)
            {
                case PartyTypeEnum.PARTY_TYPE_CLASSICAL:
                    Party = party;
                    break;

                case PartyTypeEnum.PARTY_TYPE_ARENA:
                    ArenaParty = (ArenaParty)party;
                    break;

                default:
                    logger.Error("Cannot manage party of type {0} ({1})", party.GetType(), party.Type);
                    break;
            }
        }

        public void ResetParty(PartyTypeEnum type)
        {
            switch (type)
            {
                case PartyTypeEnum.PARTY_TYPE_CLASSICAL:
                    Party = null;
                    break;

                case PartyTypeEnum.PARTY_TYPE_ARENA:
                    ArenaParty = null;
                    break;

                default:
                    logger.Error("Cannot manage party of type {0}", type);
                    break;
            }

            CompassHandler.SendCompassResetMessage(Client, CompassTypeEnum.COMPASS_TYPE_PARTY);
        }

        #endregion Party

        #region Trade

        public IExchange Exchange
        {
            get { return Dialog as IExchange; }
        }

        public Exchanger Exchanger => Dialoger as Exchanger;

        public ITrade Trade
        {
            get { return Dialog as ITrade; }
        }

        public PlayerTrade PlayerTrade
        {
            get { return Trade as PlayerTrade; }
        }

        public Trader Trader
        {
            get { return Dialoger as Trader; }
        }

        public bool IsInExchange()
        {
            return Exchanger != null;
        }

        public bool IsTrading()
        {
            return Trade != null;
        }

        public bool IsTradingWithPlayer()
        {
            return PlayerTrade != null;
        }

        #endregion Trade

        #region Idols

        public IdolInventory IdolInventory
        {
            get;
            set;
        }

        #endregion

        #region Titles & Ornaments

        public ReadOnlyCollection<short> Titles => Record.Titles.AsReadOnly();

        public ReadOnlyCollection<short> Ornaments => Record.Ornaments.AsReadOnly();

        public short? SelectedTitle
        {
            get { return Record.TitleId; }
            private set { Record.TitleId = value; }
        }

        public bool HasTitle(short title) => Record.Titles.Contains(title);

        public void AddTitle(short title)
        {
            if (HasTitle(title))
                return;

            Record.Titles.Add(title);
            TitleHandler.SendTitleGainedMessage(Client, title);
        }

        public bool RemoveTitle(short title)
        {
            var result = Record.Titles.Remove(title);

            if (result)
                TitleHandler.SendTitleLostMessage(Client, title);

            if (title == SelectedTitle)
                ResetTitle();

            return result;
        }

        public bool SelectTitle(short title)
        {
            if (!HasTitle(title))
                return false;

            SelectedTitle = title;
            TitleHandler.SendTitleSelectedMessage(Client, title);
            RefreshActor();
            return true;
        }

        public void ResetTitle()
        {
            SelectedTitle = null;
            TitleHandler.SendTitleSelectedMessage(Client, 0);
            RefreshActor();
        }

        public short? SelectedOrnament
        {
            get
            {
                return Record.Ornament;
            }
            private set
            {
                Record.Ornament = value;
            }
        }

        public bool HasOrnament(short ornament)
        {
            return Record.Ornaments.Contains(ornament);
        }

        public void AddOrnament(short ornament)
        {
            if (!HasOrnament(ornament))
                Record.Ornaments.Add(ornament);

            TitleHandler.SendOrnamentGainedMessage(Client, ornament);
        }

        public bool RemoveOrnament(short ornament)
        {
            var result = Record.Ornaments.Remove(ornament);

            if (result)
                TitleHandler.SendTitlesAndOrnamentsListMessage(Client, this);

            if (ornament == SelectedOrnament)
                ResetOrnament();

            return result;
        }

        public void RemoveAllOrnament()
        {
            Record.Ornaments.Clear();
            TitleHandler.SendTitlesAndOrnamentsListMessage(Client, this);
        }

        public bool SelectOrnament(short ornament)
        {
            if (!HasOrnament(ornament))
                return false;

            SelectedOrnament = ornament;
            TitleHandler.SendOrnamentSelectedMessage(Client, ornament);
            RefreshActor();
            return true;
        }

        public void ResetOrnament()
        {
            SelectedOrnament = null;
            TitleHandler.SendOrnamentSelectedMessage(Client, 0);
            RefreshActor();
        }

        #endregion Titles & Ornaments

        #region Apparence

        public bool CustomLookActivated
        {
            get { return m_record.CustomLookActivated; }
            set { m_record.CustomLookActivated = value; }
        }

        public ActorLook CustomLook
        {
            get { return m_record.CustomEntityLook; }
            set { m_record.CustomEntityLook = value; }
        }

        public ActorLook DefaultLook
        {
            get { return m_record.DefaultLook; }
            set
            {
                m_record.DefaultLook = value;

                UpdateLook();
            }
        }

        public override ActorLook Look
        {
            get { return CustomLookActivated ? CustomLook : m_look; }
            set
            {
                m_look = value;
                m_record.LastLook = value;
            }
        }

        public override SexTypeEnum Sex
        {
            get { return m_record.Sex; }
            protected set { m_record.Sex = value; }
        }

        public PlayableBreedEnum BreedId
        {
            get { return m_record.Breed; }
            private set
            {
                m_record.Breed = value;
                Breed = BreedManager.Instance.GetBreed(value);
            }
        }

        public Breed Breed
        {
            get;
            private set;
        }

        public Head Head
        {
            get;
            set;
        }

        public bool Invisible
        {
            get;
            private set;
        }

        public PlayerStatus Status
        {
            get;
            private set;
        }

        public void SetStatus(PlayerStatusEnum status)
        {
            if (Status.statusId == (sbyte)status)
                return;

            Status = new PlayerStatus((sbyte)status);
            CharacterStatusHandler.SendPlayerStatusUpdateMessage(Client, Status);
        }

        public bool IsAvailable(Character character, bool msg)
        {
            if (Status.statusId == (sbyte)PlayerStatusEnum.PLAYER_STATUS_SOLO)
                return false;

            if (Status.statusId == (sbyte)PlayerStatusEnum.PLAYER_STATUS_PRIVATE && !FriendsBook.IsFriend(character.Account.Id))
                return false;

            if (Status.statusId == (sbyte)PlayerStatusEnum.PLAYER_STATUS_AFK && !msg)
                return false;

            return true;
        }

        public bool ToggleInvisibility(bool toggle)
        {
            Invisible = toggle;

            if (!IsInFight())
                Map.Refresh(this);

            SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, toggle ? (short)236 : (short)237);

            return Invisible;
        }

        public bool ToggleInvisibility() => ToggleInvisibility(!Invisible);

        public void ResetDefaultLook()
        {
            var look = Breed.GetLook(Sex, true);
            look.SetColors(DefaultLook.Colors);

            foreach (var skin in Head.Skins)
                look.AddSkin(skin);

            DefaultLook = look;
        }

        public void UpdateLook(bool send = true)
        {
            var look = DefaultLook.Clone();

            look = Inventory.Where(x => x.IsEquiped()).Aggregate(look, (current, item) => item.UpdateItemSkin(current));

            switch (PlayerLifeStatus)
            {
                case PlayerLifeStatusEnum.STATUS_PHANTOM:
                    look.BonesID = 3;
                    look.AddSkin(Sex == SexTypeEnum.SEX_FEMALE ? (short)323 : (short)322);
                    look.AddSkin(Sex == SexTypeEnum.SEX_FEMALE ? Breed.FemaleGhostBonesId : Breed.MaleGhostBonesId);
                    break;
                case PlayerLifeStatusEnum.STATUS_TOMBSTONE:
                    look.BonesID = Breed.TombBonesId;
                    break;
            }

            if (IsRiding)
            {
                var mountLook = EquippedMount.Look.Clone();
                look.BonesID = 2;
                mountLook.SetRiderLook(look);

                look = mountLook;
            }
            var currentEmote = GetCurrentEmote();

            if (currentEmote != null)
            {
                look = currentEmote.UpdateEmoteLook(this, look, true);
            }

            Look = look;

            if (send)
                SendLookUpdated();
        }

        public void UpdateLook(Emote emote, bool apply, bool send = true)
        {
            Look = emote.UpdateEmoteLook(this, Look, apply);

            if (send)
                SendLookUpdated();
        }

        public void UpdateLook(BasePlayerItem item, bool send = true)
        {
            Look = item.UpdateItemSkin(Look);

            if (send)
                SendLookUpdated();
        }


        private void SendLookUpdated()
        {
            if (Fight != null)
            {
                Fighter.Look = Look.Clone();
                Fighter.Look.RemoveAuras();

                ContextHandler.SendGameContextRefreshEntityLookMessage(CharacterContainer.Clients, Fighter);
            }
            else
            {
                ContextHandler.SendGameContextRefreshEntityLookMessage(CharacterContainer.Clients, this);
            }
        }

        public void RefreshActor()
        {
            if (Fight != null)
            {
                Fighter.Look = Look.Clone();
                Fighter.Look.RemoveAuras();

                Fight.Map.Area.ExecuteInContext(() =>
                    Fight.RefreshActor(Fighter));
            }
            else if (Map != null)
            {
                Map.Area.ExecuteInContext(() =>
                    Map.Refresh(this));
            }

            OnLookRefreshed();
        }

        #endregion Apparence

        #region Stats

        #region Delegates

        public delegate void LevelChangedHandler(Character character, byte currentLevel, int difference);

        public delegate void GradeChangedHandler(Character character, sbyte currentGrade, int difference);

        #endregion Delegates

        #region Levels

        public byte Level
        {
            get;
            private set;
        }

        public long Experience
        {
            get { return RealExperience - PrestigeRank * ExperienceManager.Instance.HighestCharacterExperience; }
            private set
            {
                RealExperience = PrestigeRank * ExperienceManager.Instance.HighestCharacterExperience + value;
                if ((value < UpperBoundExperience || Level >= ExperienceManager.Instance.HighestCharacterLevel) &&
                    value >= LowerBoundExperience) return;
                var lastLevel = Level;

                Level = ExperienceManager.Instance.GetCharacterLevel(value);

                LowerBoundExperience = ExperienceManager.Instance.GetCharacterLevelExperience(Level);
                UpperBoundExperience = ExperienceManager.Instance.GetCharacterNextLevelExperience(Level);

                var difference = Level - lastLevel;

                OnLevelChanged(Level, difference);
            }
        }

        public void LevelUp(byte levelAdded)
        {
            byte level;

            if (levelAdded + Level > ExperienceManager.Instance.HighestCharacterLevel)
                level = ExperienceManager.Instance.HighestCharacterLevel;
            else
                level = (byte)(levelAdded + Level);

            var experience = ExperienceManager.Instance.GetCharacterLevelExperience(level);

            Experience = experience;
        }

        public void LevelDown(byte levelRemoved)
        {
            byte level;

            if (Level - levelRemoved < 1)
                level = 1;
            else
                level = (byte)(Level - levelRemoved);

            var experience = ExperienceManager.Instance.GetCharacterLevelExperience(level);

            Experience = experience;
        }

        public void AddExperience(int amount)
        {
            Experience += amount;
        }

        public void AddExperience(long amount)
        {
            Experience += amount;
        }

        public void AddExperience(double amount)
        {
            Experience += (long)amount;
        }

        #endregion Levels

        public long LowerBoundExperience
        {
            get;
            private set;
        }

        public long UpperBoundExperience
        {
            get;
            private set;
        }

        public ushort StatsPoints
        {
            get { return m_record.StatsPoints; }
            set { m_record.StatsPoints = value; }
        }

        public ushort SpellsPoints
        {
            get { return m_record.SpellsPoints; }
            set { m_record.SpellsPoints = value; }
        }

        public short EnergyMax
        {
            get { return m_record.EnergyMax; }
            set { m_record.EnergyMax = value; }
        }

        public short Energy
        {
            get { return m_record.Energy; }
            set
            {
                var energy = (short)(value < 0 ? 0 : value);
                var diff = (short)(energy - m_record.Energy);

                m_record.Energy = energy;
                OnEnergyChanged(energy, diff);
            }
        }

        public PlayerLifeStatusEnum PlayerLifeStatus
        {
            get { return m_record.PlayerLifeStatus; }
            set
            {
                m_record.PlayerLifeStatus = value;
                OnPlayerLifeStatusChanged(value);
            }
        }

        public int LifePoints
        {
            get { return Stats.Health.Total; }
        }

        public int MaxLifePoints
        {
            get { return Stats.Health.TotalMax; }
        }

        public SpellInventory Spells
        {
            get;
            private set;
        }

        public StatsFields Stats
        {
            get;
            private set;
        }

        public bool GodMode
        {
            get;
            private set;
        }

        public bool CriticalMode
        {
            get;
            private set;
        }

        private void OnEnergyChanged(short energy, short diff)
        {
            if (diff < 0)
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 34, Math.Abs(diff)); //Vous avez perdu <b>%1</b> points d'énergie.

            if (energy > 0 && energy <= (Level * 10) && diff < 0)
                SendSystemMessage(11, false, energy);

            PlayerLifeStatus = energy > 0 ? PlayerLifeStatusEnum.STATUS_ALIVE_AND_KICKING : PlayerLifeStatusEnum.STATUS_TOMBSTONE;
        }

        private void OnPlayerLifeStatusChanged(PlayerLifeStatusEnum status)
        {
            if (status != PlayerLifeStatusEnum.STATUS_ALIVE_AND_KICKING)
                Dismount();

            var phoenixMapId = 0;

            if (status == PlayerLifeStatusEnum.STATUS_PHANTOM)
            {
                phoenixMapId = World.Instance.GetNearestGraveyard(Map).PhoenixMapId;
                StartRegen();
            }

            CharacterHandler.SendGameRolePlayPlayerLifeStatusMessage(Client, status, phoenixMapId);
            InitializationHandler.SendSetCharacterRestrictionsMessage(Client, this);

            UpdateLook();
        }

        public void FreeSoul()
        {
            if (PlayerLifeStatus != PlayerLifeStatusEnum.STATUS_TOMBSTONE)
                return;

            var graveyard = World.Instance.GetNearestGraveyard(Map);
            Teleport(graveyard.Map, graveyard.Map.GetCell(graveyard.CellId));

            PlayerLifeStatus = PlayerLifeStatusEnum.STATUS_PHANTOM;
        }

        public event LevelChangedHandler LevelChanged;

        private void OnLevelChanged(byte currentLevel, int difference)
        {
            if (difference > 0)
            {
                SpellsPoints += (ushort)difference;
                StatsPoints += (ushort)(difference * 5);
            }

            Stats.Health.Base += (short)(difference * 5);
            Stats.Health.DamageTaken = 0;

            if (currentLevel >= 100 && currentLevel - difference < 100)
            {
                Stats.AP.Base++;
                AddOrnament((short)OrnamentEnum.NIVEAU_100);
                AddEmote(EmotesEnum.EMOTE_AURA_DE_PUISSANCE);
            }
            else if (currentLevel < 100 && currentLevel - difference >= 100)
            {
                Stats.AP.Base--;
                RemoveOrnament((short)OrnamentEnum.NIVEAU_100);
                RemoveEmote(EmotesEnum.EMOTE_AURA_DE_PUISSANCE);
            }

            if (currentLevel >= 160 && currentLevel - difference < 160)
                AddOrnament((short)OrnamentEnum.NIVEAU_160);
            else if (currentLevel < 160 && currentLevel - difference >= 160)
                RemoveOrnament((short)OrnamentEnum.NIVEAU_160);

            if (currentLevel >= 200 && currentLevel - difference < 200)
                AddOrnament((short)OrnamentEnum.NIVEAU_200);
            else if (currentLevel < 200 && currentLevel - difference >= 200)
                RemoveOrnament((short)OrnamentEnum.NIVEAU_200);

            var shortcuts = Shortcuts.SpellsShortcuts;
            foreach (var spell in Breed.Spells)
            {
                if (spell.ObtainLevel > currentLevel)
                {
                    foreach (var shortcut in shortcuts.Where(x => x.Value.SpellId == spell.Spell).ToArray())
                        Shortcuts.RemoveShortcut(ShortcutBarEnum.SPELL_SHORTCUT_BAR, shortcut.Key);

                    if (Spells.HasSpell(spell.Spell))
                    {
                        Spells.UnLearnSpell(spell.Spell);
                    }
                }
                else if (spell.ObtainLevel <= currentLevel && !Spells.HasSpell(spell.Spell))
                {
                    Spells.LearnSpell(spell.Spell);
                    Shortcuts.AddSpellShortcut(Shortcuts.GetNextFreeSlot(ShortcutBarEnum.SPELL_SHORTCUT_BAR),
                        (short)spell.Spell);
                }
            }

            RefreshStats();

            if (currentLevel > 1)
            {
                if (difference > 0)
                    CharacterHandler.SendCharacterLevelUpMessage(Client, currentLevel);
                CharacterHandler.SendCharacterLevelUpInformationMessage(Map.Clients, this, currentLevel);
            }

            LevelChanged?.Invoke(this, currentLevel, difference);
        }

        public void ResetStats(bool additional = false)
        {
            Stats.Agility.Base = 0;
            Stats.Strength.Base = 0;
            Stats.Vitality.Base = 0;
            Stats.Wisdom.Base = 0;
            Stats.Intelligence.Base = 0;
            Stats.Chance.Base = 0;

            if (additional)
            {
                Stats.Agility.Additional = 0;
                Stats.Strength.Additional = 0;
                Stats.Vitality.Additional = 0;
                Stats.Wisdom.Additional = 0;
                Stats.Intelligence.Additional = 0;
                Stats.Chance.Additional = 0;
            }

            var newPoints = (Level - 1) * 5;
            StatsPoints = (ushort)newPoints;

            RefreshStats();
            Inventory.CheckItemsCriterias();

            //Caractéristiques (de base et additionnelles) réinitialisées.(469)
            //Caractéristiques de base réinitialisées.(470)
            SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, (short)(additional ? 469 : 470));
        }

        public void RefreshStats()
        {
            if (IsRegenActive())
                UpdateRegenedLife();

            CharacterHandler.SendCharacterStatsListMessage(Client);

            OnStatsResfreshed();
        }

        public void ToggleGodMode(bool state)
        {
            GodMode = state;
        }

        public void ToggleCriticalMode(bool state)
        {
            CriticalMode = state;
        }

        public bool IsGameMaster()
        {
            return UserGroup.IsGameMaster;
        }

        public void SetBreed(PlayableBreedEnum breed)
        {
            BreedId = breed;
            ResetDefaultLook();
        }

        #endregion Stats

        #region Mount

        private List<Mount> m_stabledMounts = new List<Mount>();
        private List<Mount> m_publicPaddockedMounts = new List<Mount>();
        private Queue<Mount> m_releaseMounts = new Queue<Mount>();

        public Mount EquippedMount
        {
            get { return m_equippedMount; }
            private set
            {
                m_equippedMount = value;
                Record.EquippedMount = value?.Id;

                if (value == null)
                    IsRiding = false;
            }
        }

        public bool IsRiding
        {
            get { return EquippedMount != null && Record.IsRiding; }
            private set { Record.IsRiding = value; }
        }

        public ReadOnlyCollection<Mount> PublicPaddockedMounts => m_publicPaddockedMounts.AsReadOnly();
        public ReadOnlyCollection<Mount> StabledMounts => m_stabledMounts.AsReadOnly();

        public Mount GetStabledMount(int mountId)
        {
            return m_stabledMounts.FirstOrDefault(x => x.Id == mountId);
        }

        public Mount GetPublicPaddockedMount(int mountId)
        {
            return m_publicPaddockedMounts.FirstOrDefault(x => x.Id == mountId);
        }

        private void LoadMounts()
        {
            var database = MountManager.Instance.Database;

            m_stabledMounts = database.Query<MountRecord>(string.Format(MountRecordRelator.FindByOwnerStabled, Id)).Select(x => new Mount(this, x)).ToList();
            m_publicPaddockedMounts = database.Query<MountRecord>(string.Format(MountRecordRelator.FindByOwnerPublicPaddocked, Id)).Select(x => new Mount(this, x)).ToList();

            if (Record.EquippedMount.HasValue)
            {
                EquippedMount = new Mount(this, database.Single<MountRecord>(string.Format(MountRecordRelator.FindById, Record.EquippedMount.Value)));

                if (IsRiding)
                    EquippedMount.ApplyMountEffects(false);
            }
        }

        private void SaveMounts()
        {
            var database = MountManager.Instance.Database;
            if (EquippedMount != null && (EquippedMount.IsDirty || EquippedMount.Record.IsNew))
                EquippedMount.Save(database);

            foreach (var mount in m_publicPaddockedMounts.Where(x => x.IsDirty || x.Record.IsNew))
                mount.Save(database);

            foreach (var mount in m_stabledMounts.Where(x => x.IsDirty || x.Record.IsNew))
                mount.Save(database);

            while (m_releaseMounts.Count > 0)
            {
                var deletedMount = m_releaseMounts.Dequeue();
                MountManager.Instance.DeleteMount(deletedMount.Record);
            }
        }

        public void AddStabledMount(Mount mount)
        {
            mount.Owner = this;
            m_stabledMounts.Add(mount);
        }

        public void RemoveStabledMount(Mount mount)
        {
            m_stabledMounts.Remove(mount);
        }

        public void AddPublicPaddockedMount(Mount mount)
        {
            m_publicPaddockedMounts.Add(mount);
        }

        public void RemovePublicPaddockedMount(Mount mount)
        {
            m_publicPaddockedMounts.Remove(mount);
        }

        public void SetOwnedMount(Mount mount)
        {
            mount.Owner = this;
        }

        public bool HasEquippedMount()
        {
            return EquippedMount != null;
        }

        public bool EquipMount(Mount mount)
        {
            if (mount.Owner != this)
                return false;

            EquippedMount = mount;

            MountHandler.SendMountSetMessage(Client, mount.GetMountClientData());
            MountHandler.SendMountXpRatioMessage(Client, mount.GivenExperience);
            return true;
        }

        public void UnEquipMount()
        {
            if (EquippedMount != null)
            {
                if (EquippedMount.Harness != null)
                {
                    Inventory.MoveItem(EquippedMount.Harness, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

                    // Votre harnachement est déposé dans votre inventaire.
                    BasicHandler.SendTextInformationMessage(Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 661);
                }

                Dismount();
                EquippedMount = null;
                MountHandler.SendMountUnSetMessage(Client);

            }
        }

        public bool ReleaseMount()
        {
            if (EquippedMount == null)
                return false;

            var mount = EquippedMount;
            UnEquipMount();

            MountHandler.SendMountReleaseMessage(Client, mount.Id);
            m_releaseMounts.Enqueue(mount);
            return true;
        }

        public bool RideMount()
        {
            return !IsRiding && ToggleRiding();
        }

        public bool Dismount()
        {
            return IsRiding && ToggleRiding();
        }

        public bool ToggleRiding()
        {
            if (EquippedMount == null)
                return false;

            if (!IsRiding && Level < Mount.RequiredLevel)
            {
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 227, Mount.RequiredLevel);
                return false;
            }

            if ((IsBusy() && !(Dialog is PaddockExchange)) || (IsInFight() && Fight.State != FightState.Placement))
            {
                //Une action est déjà en cours. Impossible de monter ou de descendre de votre monture.
                BasicHandler.SendTextInformationMessage(Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 355);
                return false;
            }

            if (!IsRiding && !Map.Outdoor && !Map.SpawningPools.Any(x => x is DungeonSpawningPool))
            {
                //Impossible d'être sur une monture à l'intérieur d'une maison.
                BasicHandler.SendTextInformationMessage(Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 117);

                return false;
            }

            IsRiding = !IsRiding;

            if (IsRiding)
            {
                var pet = Inventory.TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS);
                if (pet != null)
                    Inventory.MoveItem(pet, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

                EquippedMount.ApplyMountEffects();
            }
            else
            {
                //Vous descendez de votre monture.
                BasicHandler.SendTextInformationMessage(Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 273);

                EquippedMount.UnApplyMountEffects();
            }

            UpdateLook();

            MountHandler.SendMountRidingMessage(Client, IsRiding);

            return true;
        }

        #endregion Mount

        #region Guild

        public GuildMember GuildMember
        {
            get;
            set;
        }

        public Guild Guild
        {
            get { return GuildMember != null ? GuildMember.Guild : null; }
        }

        public bool WarnOnGuildConnection
        {
            get { return Record.WarnOnGuildConnection; }
            set
            {
                Record.WarnOnGuildConnection = value;
                GuildHandler.SendGuildMemberWarnOnConnectionStateMessage(Client, value);
            }
        }

        #endregion Guild

        #region Alignment

        public AlignmentSideEnum AlignmentSide
        {
            get { return m_record.AlignmentSide; }
            private set
            {
                m_record.AlignmentSide = value;
            }
        }

        public sbyte AlignmentGrade
        {
            get;
            private set;
        }

        public sbyte AlignmentValue
        {
            get { return m_record.AlignmentValue; }
            private set { m_record.AlignmentValue = value; }
        }

        public ushort Honor
        {
            get { return m_record.Honor; }
            set
            {
                m_record.Honor = value;
                if ((value > LowerBoundHonor && value < UpperBoundHonor))
                    return;

                var lastGrade = AlignmentGrade;

                AlignmentGrade = (sbyte)ExperienceManager.Instance.GetAlignementGrade(m_record.Honor);

                LowerBoundHonor = ExperienceManager.Instance.GetAlignementGradeHonor((byte)AlignmentGrade);
                UpperBoundHonor = ExperienceManager.Instance.GetAlignementNextGradeHonor((byte)AlignmentGrade);

                var difference = AlignmentGrade - lastGrade;

                if (difference != 0)
                    OnGradeChanged(AlignmentGrade, difference);
            }
        }

        public ushort LowerBoundHonor
        {
            get;
            private set;
        }

        public ushort UpperBoundHonor
        {
            get;
            private set;
        }

        public ushort Dishonor
        {
            get { return m_record.Dishonor; }
            private set { m_record.Dishonor = value; }
        }

        public int CharacterPower
        {
            get { return Id + Level; }
        }

        public bool PvPEnabled
        {
            get { return m_record.PvPEnabled; }
            private set
            {
                m_record.PvPEnabled = value;
                OnPvPToggled();
            }
        }

        public void ChangeAlignementSide(AlignmentSideEnum side)
        {
            AlignmentSide = side;

            OnAligmenentSideChanged();
        }

        public void AddHonor(ushort amount)
        {
            Honor += (Honor + amount) >= HonorLimit ? (ushort)(HonorLimit - Honor) : amount;
        }

        public void SubHonor(ushort amount)
        {
            if (Honor - amount < 0)
                Honor = 0;
            else
                Honor -= amount;
        }

        public void AddDishonor(ushort amount)
        {
            Dishonor += amount;
        }

        public void SubDishonor(ushort amount)
        {
            if (Dishonor - amount < 0)
                Dishonor = 0;
            else
                Dishonor -= amount;
        }

        public void TogglePvPMode(bool state)
        {
            if (IsInFight())
                return;

            PvPEnabled = state;
        }

        public event GradeChangedHandler GradeChanged;

        private void OnGradeChanged(sbyte currentLevel, int difference)
        {
            Map.Refresh(this);
            RefreshStats();

            GradeChanged?.Invoke(this, currentLevel, difference);
        }

        public event Action<Character, bool> PvPToggled;

        private void OnPvPToggled()
        {
            foreach (var item in Inventory.GetItems(x => x.Position == CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD && x.AreConditionFilled(this)))
                Inventory.MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

            if (!PvPEnabled)
            {
                var amount = (ushort)Math.Round(Honor * 0.05);
                SubHonor(amount);
                SendServerMessage($"Vous avez perdu <b>{amount}</b> points d' honneur.");
            }

            Map.Refresh(this);
            RefreshStats();

            PvPToggled?.Invoke(this, PvPEnabled);
        }

        public event Action<Character, AlignmentSideEnum> AligmenentSideChanged;

        private void OnAligmenentSideChanged()
        {
            TogglePvPMode(false);
            Map.Refresh(this);

            Honor = 0;
            Dishonor = 0;

            AligmenentSideChanged?.Invoke(this, AlignmentSide);
        }

        #endregion Alignment

        #region Fight

        public CharacterFighter Fighter
        {
            get;
            private set;
        }

        public FightSpectator Spectator
        {
            get;
            private set;
        }

        public FightPvT TaxCollectorDefendFight
        {
            get;
            private set;
        }

        public IFight Fight
        {
            get { return Fighter == null ? (Spectator != null ? Spectator.Fight : null) : Fighter.Fight; }
        }

        public FightTeam Team
        {
            get { return Fighter != null ? Fighter.Team : null; }
        }

        public bool IsGhost()
        {
            return PlayerLifeStatus != PlayerLifeStatusEnum.STATUS_ALIVE_AND_KICKING;
        }

        public bool IsSpectator()
        {
            return Spectator != null;
        }

        public bool IsInFight()
        {
            return IsSpectator() || IsFighting();
        }

        public bool IsFighting()
        {
            return Fighter != null;
        }

        public void SetDefender(FightPvT fight)
        {
            TaxCollectorDefendFight = fight;
        }

        public void ResetDefender()
        {
            TaxCollectorDefendFight = null;
        }

        #endregion Fight

        #region Shortcuts

        public ShortcutBar Shortcuts
        {
            get;
            private set;
        }

        #endregion Shortcuts

        #region Regen

        public byte RegenSpeed
        {
            get;
            private set;
        }

        public DateTime? RegenStartTime
        {
            get;
            private set;
        }

        #endregion Regen

        #region Chat

        public ChatHistory ChatHistory
        {
            get;
            private set;
        }

        public DateTime? MuteUntil
        {
            get { return m_record.MuteUntil; }
            private set { m_record.MuteUntil = value; }
        }

        public void Mute(TimeSpan time, Character from)
        {
            MuteUntil = DateTime.Now + time;

            // %1 vous a rendu muet pour %2 minute(s).
            SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 17, from.Name,
                (int)time.TotalMinutes);
        }

        public void Mute(TimeSpan time)
        {
            MuteUntil = DateTime.Now + time;
            // Le principe de précaution vous a rendu muet pour %1 seconde(s).
            SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 123, (int)time.TotalSeconds);
        }

        public void UnMute()
        {
            MuteUntil = null;
            SendServerMessage("Vous avez été démuté.", Color.Red);
        }

        public bool IsMuted()
        {
            return MuteUntil.HasValue && MuteUntil > DateTime.Now;
        }

        public TimeSpan GetMuteRemainingTime()
        {
            if (!MuteUntil.HasValue)
                return TimeSpan.MaxValue;

            return MuteUntil.Value - DateTime.Now;
        }

        #endregion Chat

        #region Smiley

        public event Action<Character, int> MoodChanged;

        private void OnMoodChanged()
        {
            Guild?.UpdateMember(Guild.TryGetMember(Id));
            MoodChanged?.Invoke(this, SmileyMoodId);
        }

        public ReadOnlyCollection<SmileyPacksEnum> SmileyPacks => Record.SmileyPacks.AsReadOnly();

        public int SmileyMoodId
        {
            get { return Record.SmileyMoodId; }
            set { Record.SmileyMoodId = value; }
        }

        public DateTime LastMoodChange
        {
            get;
            private set;
        }

        public bool HasSmileyPack(SmileyPacksEnum pack) => SmileyPacks.Contains(pack);

        public void AddSmileyPack(SmileyPacksEnum pack)
        {
            if (HasSmileyPack(pack))
                return;

            Record.SmileyPacks.Add(pack);
            ChatHandler.SendChatSmileyExtraPackListMessage(Client, SmileyPacks.ToArray());
        }

        public bool RemoveSmileyPack(SmileyPacksEnum pack)
        {
            var result = Record.SmileyPacks.Remove(pack);

            if (result)
                ChatHandler.SendChatSmileyExtraPackListMessage(Client, SmileyPacks.ToArray());

            return result;
        }

        public override void DisplaySmiley(short smileyId)
        {
            ChatHandler.SendChatSmileyMessage(CharacterContainer.Clients, this, smileyId);
        }

        public void SetMood(short smileyId)
        {
            if (DateTime.Now - LastMoodChange < TimeSpan.FromSeconds(5))
                ChatHandler.SendMoodSmileyResultMessage(Client, 2, smileyId);
            else
            {
                SmileyMoodId = smileyId;
                LastMoodChange = DateTime.Now;

                ChatHandler.SendMoodSmileyResultMessage(Client, 0, smileyId);
                OnMoodChanged();
            }
        }

        #endregion Smiley

        #region Prestige

        public int PrestigeRank
        {
            get { return m_record.PrestigeRank; }
            private set { m_record.PrestigeRank = value; }
        }

        public long RealExperience
        {
            get { return m_record.Experience; }
            private set { m_record.Experience = value; }
        }

        public bool IsPrestigeMax() => PrestigeRank == PrestigeManager.PrestigeTitles.Length;

        public PrestigeItem GetPrestigeItem()
        {
            if (!PrestigeManager.Instance.PrestigeEnabled)
                return null;

            return Inventory.TryGetItem(PrestigeManager.BonusItem) as PrestigeItem;
        }

        public PrestigeItem CreatePrestigeItem() => (PrestigeItem)Inventory.AddItem(PrestigeManager.BonusItem);

        public bool IncrementPrestige()
        {
            if (Level < 200 || IsPrestigeMax() && PrestigeManager.Instance.PrestigeEnabled)
                return false;

            PrestigeRank++;
            AddTitle(PrestigeManager.Instance.GetPrestigeTitle(PrestigeRank));

            switch (PrestigeRank)
            {
                case 5:
                    AddOrnament(25);
                    break;

                case 10:
                    AddOrnament(49);
                    break;

                case 15:
                    AddOrnament(50);
                    break;
            }

            var item = GetPrestigeItem();

            if (item == null)
                item = CreatePrestigeItem();
            else
            {
                item.UpdateEffects();
                Inventory.RefreshItem(item);
            }

            OpenPopup(
                string.Format(
                    "Vous venez de passer au rang prestige {0}. \r\nVous repassez niveau 1 et vous avez acquis des bonus permanents visible sur l'objet '{1}' de votre inventaire, ",
                    PrestigeRank, item.Template.Name) +
                "les bonus s'appliquent sans équiper l'objet. \r\nVous devez vous reconnecter pour actualiser votre niveau.", "PRESTIGE", 0);

            foreach (var equippedItem in Inventory.ToArray())
                Inventory.MoveItem(equippedItem, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

            var points = (Spells.CountSpentBoostPoint() + SpellsPoints) - (Level - 1);

            Experience = 0;
            Spells.ForgetAllSpells();
            SpellsPoints = (ushort)(points >= 0 ? points : 0);
            ResetStats();

            return true;
        }

        public bool DecrementPrestige()
        {
            RemoveTitle(PrestigeManager.Instance.GetPrestigeTitle(PrestigeRank));
            PrestigeRank--;

            var item = GetPrestigeItem();

            if (item != null)
            {
                if (PrestigeRank > 0)
                {
                    item.UpdateEffects();
                    Inventory.RefreshItem(item);
                }
                else Inventory.RemoveItem(item);
            }

            OpenPopup(
                string.Format(
                    "Vous venez de passer au rang prestige {0}. Vous repassez niveau 1 et vous avez acquis des bonus permanents visible sur l'objet '{1}' de votre inventaire, ",
                    PrestigeRank + 1, item.Template.Name) +
                "les bonus s'appliquent sans équipper l'objet. Vous devez vous reconnecter pour actualiser votre niveau.");

            return true;
        }

        public void ResetPrestige()
        {
            foreach (var title in PrestigeManager.PrestigeTitles)
            {
                RemoveTitle(title);
            }
            PrestigeRank = 0;

            var item = GetPrestigeItem();

            if (item != null)
            {
                Inventory.RemoveItem(item);
            }
        }

        #endregion Prestige

        #region Arena

        public bool CanEnterArena(bool send = true)
        {
            if (Level < ArenaManager.ArenaMinLevel)
            {
                if (send)
                    // Vous devez être au moins niveau 50 pour faire des combats en Kolizéum.
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 326);
                return false;
            }

            if (ArenaPenality >= DateTime.Now)
            {
                if (send)
                    // Vous êtes interdit de Kolizéum pour un certain temps car vous avez abandonné un match de Kolizéum.
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 323);

                return false;
            }

            if (IsInJail())
            {
                if (send)
                    // Vous ne pouvez pas participer au Kolizéum depuis une prison.
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 339);

                return false;
            }

            if (IsGhost())
            {
                if (send)
                    // Aucun combat de kolizéum ne vous sera proposé tant que vous serez en tombe ou en fantôme.
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 373);

                return false;
            }

            if (Fight is ArenaFight)
            {
                if (send)
                    //Vous êtes déjà en combat de Kolizéum.
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 334);

                return false;
            }

            if (Fight is FightAgression || Fight is FightPvT || Fight is FightDuel)
                return false;

            return true;
        }

        public void CheckArenaDailyProperties()
        {
            if (m_record.ArenaDailyDate.Day == DateTime.Now.Day || ArenaDailyMaxRank <= 0)
                return;

            var amountToken = (int)Math.Floor(ArenaDailyMaxRank / 10d);
            var amountKamas = (ArenaDailyMaxRank * 10);

            m_record.ArenaDailyDate = DateTime.Now;
            ArenaDailyMaxRank = 0;
            ArenaDailyMatchsCount = 0;
            ArenaDailyMatchsWon = 0;

            Inventory.AddItem(ArenaManager.Instance.TokenItemTemplate, amountToken);
            Inventory.AddKamas(amountKamas);

            DisplayNotification(NotificationEnum.KOLIZÉUM, amountKamas, amountToken);
        }

        public int ComputeWonArenaTokens(int rank)
        {
            var result = (int)Math.Floor(rank / 100d);
            return result > 0 ? result : 1;
        }

        public int ComputeWonArenaKamas()
        {
            return (int)Math.Floor((50 * (Level * (Level / 200d))));
        }

        public void UpdateArenaProperties(int rank, bool win)
        {
            CheckArenaDailyProperties();

            ArenaRank = rank;

            if (rank > ArenaMaxRank)
                ArenaMaxRank = rank;

            if (rank > ArenaDailyMaxRank)
                ArenaDailyMaxRank = rank;

            ArenaDailyMatchsCount++;

            if (win)
                ArenaDailyMatchsWon++;

            m_record.ArenaDailyDate = DateTime.Now;

            ContextRoleplayHandler.SendGameRolePlayArenaUpdatePlayerInfosMessage(Client, this);

            if (!win)
                return;

            Inventory.AddItem(ArenaManager.Instance.TokenItemTemplate, ComputeWonArenaTokens(ArenaRank));
            Inventory.AddKamas(ComputeWonArenaKamas());
        }

        public void SetArenaPenality(TimeSpan time)
        {
            ArenaPenality = DateTime.Now + time;

            // Vous êtes interdit de Kolizéum pour un certain temps car vous avez abandonné un match de Kolizéum.
            SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 323);
        }

        public void ToggleArenaPenality()
        {
            SetArenaPenality(TimeSpan.FromMinutes(ArenaManager.ArenaPenalityTime));
        }

        public void ToggleArenaWaitTime()
        {
            SetArenaPenality(TimeSpan.FromMinutes(ArenaManager.ArenaWaitTime));
        }

        public int ArenaRank
        {
            get { return m_record.ArenaRank; }
            set { m_record.ArenaRank = value; }
        }

        public int ArenaMaxRank
        {
            get { return m_record.ArenaMaxRank; }
            set { m_record.ArenaMaxRank = value; }
        }

        public int ArenaDailyMaxRank
        {
            get { return m_record.ArenaDailyMaxRank; }
            set { m_record.ArenaDailyMaxRank = value; }
        }

        public int ArenaDailyMatchsWon
        {
            get { return m_record.ArenaDailyMatchsWon; }
            set { m_record.ArenaDailyMatchsWon = value; }
        }

        public int ArenaDailyMatchsCount
        {
            get { return m_record.ArenaDailyMatchsCount; }
            set { m_record.ArenaDailyMatchsCount = value; }
        }

        public DateTime ArenaPenality
        {
            get { return m_record.ArenaPenalityDate; }
            set { m_record.ArenaPenalityDate = value; }
        }

        public ArenaPopup ArenaPopup
        {
            get;
            set;
        }

        #endregion Arena

        #endregion Properties

        #region Actions

        #region Chat

        public bool AdminMessagesEnabled
        {
            get;
            set;
        }

        public void SendConnectionMessages()
        {
            SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 89);
            if (Account.LastConnection != null)
            {
                var date = Account.LastConnection.Value;

                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 152,
                    date.Year,
                    date.Month,
                    date.Day,
                    date.Hour,
                    date.Minute.ToString("00"),
                    Account.LastConnectionIp);

                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 153, Client.IP);
            }

            var kamasMerchant = 0;
            var merchantSoldItems = new List<ObjectItemGenericQuantityPrice>();

            foreach (var item in MerchantBag.ToArray())
            {
                if (item.StackSold <= 0)
                    continue;

                var price = (int)(item.Price * item.StackSold);
                kamasMerchant += price;

                merchantSoldItems.Add(new ObjectItemGenericQuantityPrice((short)item.Template.Id, (int)item.StackSold, price));

                item.StackSold = 0;

                if (item.Stack == 0)
                    MerchantBag.RemoveItem(item, true);
            }

            Inventory.AddKamas(kamasMerchant);

            var soldItems = BidHouseManager.Instance.GetSoldBidHouseItems(Account.Id);
            var bidhouseSoldItems = new List<ObjectItemGenericQuantityPrice>();
            var kamasBidHouse = 0;

            foreach (var item in soldItems)
            {
                kamasBidHouse += (int)item.Price;
                BidHouseManager.Instance.RemoveBidHouseItem(item, true);

                bidhouseSoldItems.Add(new ObjectItemGenericQuantityPrice((short)item.Template.Id, (int)item.Stack, (int)item.Price));
            }

            Bank.AddKamas(kamasBidHouse);

            if (merchantSoldItems.Any() || bidhouseSoldItems.Any())
                InventoryHandler.SendExchangeOfflineSoldItemsMessage(Client, merchantSoldItems.ToArray(), bidhouseSoldItems.ToArray());
        }

        public void SendServerMessage(string message)
        {
            BasicHandler.SendTextInformationMessage(Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 0, message);
        }

        public void SendServerMessage(string message, Color color)
        {
            SendServerMessage(string.Format("<font color=\"#{0}\">{1}</font>", color.ToArgb().ToString("X"), message));
        }

        public void SendInformationMessage(TextInformationTypeEnum msgType, short msgId, params object[] parameters)
        {
            BasicHandler.SendTextInformationMessage(Client, msgType, msgId, parameters);
        }

        public void SendSystemMessage(short msgId, bool hangUp, params object[] parameters)
        {
            BasicHandler.SendSystemMessageDisplayMessage(Client, hangUp, msgId, parameters);
        }

        public void OpenPopup(string message)
        {
            OpenPopup(message, "Server", 0);
        }

        public void OpenPopup(string message, string sender, byte lockDuration)
        {
            ModerationHandler.SendPopupWarningMessage(Client, message, sender, lockDuration);
        }

        #endregion Chat

        #region Move

        public override void OnEnterMap(Map map)
        {
            ContextRoleplayHandler.SendCurrentMapMessage(Client, map.Id);

            // send actor actions
            foreach (var actor in map.Actors)
            {
                if (!actor.IsMoving())
                    continue;

                var moveKeys = actor.MovementPath.GetServerPathKeys();
                var actorMoving = actor;

                if (actor.MovementPath.Walk)
                    ContextHandler.SendGameCautiousMapMovementMessage(Client, moveKeys, actorMoving);
                else
                    ContextHandler.SendGameMapMovementMessage(Client, moveKeys, actorMoving);

                BasicHandler.SendBasicNoOperationMessage(Client);
            }

            if (map.Zaap != null && !KnownZaaps.Contains(map))
                DiscoverZaap(map);

            if (MustBeJailed() && !IsInJail())
                TeleportToJail();
            else if (!MustBeJailed() && IsInJail() && !IsGameMaster())
                Teleport(Breed.GetStartPosition());

            if (IsRiding && !map.Outdoor && ArenaManager.Instance.Arenas.All(x => x.Value.MapId != map.Id))
                Dismount();

            ResetCurrentSkill();

            foreach (var job in Jobs.Where(x => x.IsIndexed))
            {
                job.Template.RefreshCrafter(this);
            }

            base.OnEnterMap(map);
        }

        public override bool CanMove()
        {
            if (PlayerLifeStatus == PlayerLifeStatusEnum.STATUS_TOMBSTONE)
                return false;

            if (Fight?.State == FightState.Placement || Fight?.State == FightState.NotStarted)
                return false;

            return base.CanMove() && !IsDialoging();
        }

        public override bool IsGonnaChangeZone() => base.IsGonnaChangeZone() || !IsLoggedIn;

        public override bool StartMove(Path movementPath)
        {
            LeaveDialog(); //Close Dialog && RequestBox when moving

            if (Inventory.IsFull())
                movementPath.SetWalk();

            if (!IsFighting() && !MustBeJailed() && IsInJail())
            {
                Teleport(Breed.GetStartPosition());
                return false;
            }

            if (IsFighting())
                if (Fighter.IsSlaveTurn())
                    return Fighter.GetSlave().StartMove(movementPath);
                else return Fighter.StartMove(movementPath);

            CancelEmote(false);

            return base.StartMove(movementPath);
        }

        public override bool StopMove() => IsFighting() ? Fighter.StopMove() : base.StopMove();

        public override bool MoveInstant(ObjectPosition destination) => IsFighting() ? Fighter.MoveInstant(destination) : base.MoveInstant(destination);

        public override bool StopMove(ObjectPosition currentObjectPosition) => IsFighting() ? Fighter.StopMove(currentObjectPosition) : base.StopMove(currentObjectPosition);

        public override bool Teleport(MapNeighbour mapNeighbour)
        {
            var success = base.Teleport(mapNeighbour);

            if (!success)
                SendServerMessage("Unknown map transition");

            return success;
        }

        #region Jail

        private readonly int[] JAILS_MAPS = { 105121026, 105119744, 105120002 };
        private readonly int[][] JAILS_CELLS = { new[] { 179, 445, 184, 435 }, new[] { 314 }, new[] { 300 } };

        public bool TeleportToJail()
        {
            var random = new AsyncRandom();

            var mapIndex = random.Next(0, JAILS_MAPS.Length);
            var cellIndex = random.Next(0, JAILS_CELLS[mapIndex].Length);

            var map = World.Instance.GetMap(JAILS_MAPS[mapIndex]);

            if (map == null)
            {
                logger.Error("Cannot find jail map {0}", JAILS_MAPS[mapIndex]);
                return false;
            }

            var cell = map.Cells[JAILS_CELLS[mapIndex][cellIndex]];

            Teleport(new ObjectPosition(map, cell), false);

            return true;
        }

        public bool MustBeJailed()
        {
            return Client.Account.IsJailed && (Client.Account.BanEndDate == null || Client.Account.BanEndDate > DateTime.Now);
        }

        public bool IsInJail()
        {
            return JAILS_MAPS.Contains(Map.Id);
        }

        #endregion Jail

        protected override void OnTeleported(ObjectPosition position)
        {
            base.OnTeleported(position);

            UpdateRegenedLife();

            if (Dialog != null)
                Dialog.Close();
        }

        public override bool CanChangeMap() => base.CanChangeMap() && !IsFighting() && !Account.IsJailed;

        #endregion Move

        #region Dialog

        public void DisplayNotification(string text, NotificationEnum notification = NotificationEnum.INFORMATION)
        {
            Client.Send(new NotificationByServerMessage((short)notification, new[] { text }, true));
        }

        public void DisplayNotification(NotificationEnum notification, params object[] parameters)
        {
            Client.Send(new NotificationByServerMessage((short)notification, parameters.Select(entry => entry.ToString()), true));
        }

        public void DisplayNotification(Notification notification)
        {
            notification.Display();
        }

        public void LeaveDialog()
        {
            if (IsInRequest())
                CancelRequest();

            if (IsDialoging())
                Dialog.Close();
        }

        public void ReplyToNpc(short replyId)
        {
            if (!IsTalkingWithNpc())
                return;

            ((NpcDialog)Dialog).Reply(replyId);
        }

        public void AcceptRequest()
        {
            if (!IsInRequest())
                return;

            if (RequestBox.Target == this)
                RequestBox.Accept();
        }

        public void DenyRequest()
        {
            if (!IsInRequest())
                return;

            if (RequestBox.Target == this)
                RequestBox.Deny();
        }

        public void CancelRequest()
        {
            if (!IsInRequest())
                return;

            if (IsRequestSource())
                RequestBox.Cancel();
            else if (IsRequestTarget())
                DenyRequest();
        }

        #endregion Dialog

        #region Party

        public void Invite(Character target, PartyTypeEnum type, bool force = false)
        {
            var created = false;
            Party party;
            if (!IsInParty(type))
            {
                party = PartyManager.Instance.Create(type);

                if (!EnterParty(party))
                    return;

                created = true;
            }
            else party = GetParty(type);
            if (!party.CanInvite(target, out var error, this))
            {
                PartyHandler.SendPartyCannotJoinErrorMessage(target.Client, party, error);
                if (created)
                    LeaveParty(party);

                return;
            }

            if (target.m_partyInvitations.ContainsKey(party.Id))
            {
                if (created)
                    LeaveParty(party);

                return; // already invited
            }

            var invitation = new PartyInvitation(party, this, target);
            target.m_partyInvitations.Add(party.Id, invitation);

            party.AddGuest(target);

            if (force)
                invitation.Accept();
            else
                invitation.Display();
        }

        public PartyInvitation GetInvitation(int id)
        {
            return m_partyInvitations.ContainsKey(id) ? m_partyInvitations[id] : null;
        }

        public bool RemoveInvitation(PartyInvitation invitation)
        {
            return m_partyInvitations.Remove(invitation.Party.Id);
        }

        public void DenyAllInvitations()
        {
            foreach (var partyInvitation in m_partyInvitations.ToArray())
            {
                partyInvitation.Value.Deny();
            }
        }

        public void DenyAllInvitations(PartyTypeEnum type)
        {
            foreach (var partyInvitation in m_partyInvitations.Where(x => x.Value.Party.Type == type).ToArray())
            {
                partyInvitation.Value.Deny();
            }
        }

        public void DenyAllInvitations(Party party)
        {
            foreach (var partyInvitation in m_partyInvitations.Where(x => x.Value.Party == party).ToArray())
            {
                partyInvitation.Value.Deny();
            }
        }

        public bool EnterParty(Party party)
        {
            if (IsInParty(party.Type))
                LeaveParty(GetParty(party.Type));

            if (m_partyInvitations.ContainsKey(party.Id))
                m_partyInvitations.Remove(party.Id);

            DenyAllInvitations(party.Type);
            UpdateRegenedLife();

            if (party.Disbanded)
                return false;

            SetParty(party);
            party.MemberRemoved += OnPartyMemberRemoved;
            party.PartyDeleted += OnPartyDeleted;

            if (party.IsMember(this))
                return false;

            if (party.PromoteGuestToMember(this))
                return true;

            // if fails to enter
            party.MemberRemoved -= OnPartyMemberRemoved;
            party.PartyDeleted -= OnPartyDeleted;
            ResetParty(party.Type);

            return false;
        }

        public void LeaveParty(Party party)
        {
            if (!IsInParty(party.Id) || !party.CanLeaveParty(this))
                return;

            party.MemberRemoved -= OnPartyMemberRemoved;
            party.PartyDeleted -= OnPartyDeleted;
            party.RemoveMember(this);
            ResetParty(party.Type);
        }

        private void OnPartyMemberRemoved(Party party, Character member, bool kicked)
        {
            if (m_followedCharacter == member)
                UnfollowMember();

            if (member != this)
                return;

            party.MemberRemoved -= OnPartyMemberRemoved;
            party.PartyDeleted -= OnPartyDeleted;

            ResetParty(party.Type);
        }

        private void OnPartyDeleted(Party party)
        {
            party.MemberRemoved -= OnPartyMemberRemoved;
            party.PartyDeleted -= OnPartyDeleted;

            ResetParty(party.Type);
        }

        public void FollowMember(Character character)
        {
            if (m_followedCharacter != null)
                UnfollowMember();

            m_followedCharacter = character;
            character.EnterMap += OnFollowedMemberEnterMap;

            PartyHandler.SendPartyFollowStatusUpdateMessage(Client, Party, true, character.Id);
            CompassHandler.SendCompassUpdatePartyMemberMessage(Client, character, true);
        }

        public void UnfollowMember()
        {
            if (m_followedCharacter == null)
                return;

            m_followedCharacter.EnterMap -= OnFollowedMemberEnterMap;

            PartyHandler.SendPartyFollowStatusUpdateMessage(Client, Party, true, 0);
            CompassHandler.SendCompassUpdatePartyMemberMessage(Client, m_followedCharacter, false);

            m_followedCharacter = null;
        }

        private void OnFollowedMemberEnterMap(RolePlayActor actor, Map map)
        {
            if (!(actor is Character character))
                return;

            CompassHandler.SendCompassUpdatePartyMemberMessage(Client, character, true);
        }

        #endregion Party

        #region Quest

        private List<Quest> m_quests = new List<Quest>();

        public ReadOnlyCollection<Quest> Quests => m_quests.AsReadOnly();

        public void LoadQuests()
        {
            var database = QuestManager.Instance.Database;

            m_quests = database.Query<QuestRecord>(string.Format(QuestRecordRelator.FetchByOwner, Id)).Select(x => new Quest(this, x)).ToList();
        }

        public void StartQuest(int questStepId)
        {
            var step = QuestManager.Instance.GetQuestStep(questStepId);

            if (step == null)
                throw new Exception($"Step {questStepId} not found");

            StartQuest(step);
        }

        public void StartQuest(QuestStepTemplate questStep)
        {
            var quest = m_quests.FirstOrDefault(x => x.Template.Steps.Contains(questStep));

            if (quest == null)
            {
                quest = new Quest(this, questStep);
                m_quests.Add(quest);
            }
            else
            {
                quest.ChangeQuestStep(questStep);
            }
        }

        #endregion

        #region Fight

        public delegate void CharacterContextChangedHandler(Character character, bool inFight);

        public event CharacterContextChangedHandler ContextChanged;

        public delegate void CharacterFightEndedHandler(Character character, CharacterFighter fighter);

        public event CharacterFightEndedHandler FightEnded;

        public delegate void CharacterDiedHandler(Character character);

        public event CharacterDiedHandler Died;

        private void OnDied()
        {
            var energylost = (short)(10 * Level);

            if (SuperArea.Id == 5) //Dimensions divines
                energylost *= 2;

            Energy -= energylost;

            if (!IsGhost())
            {
                var dest = GetSpawnPoint() ?? Breed.GetStartPosition();

                NextMap = dest.Map;
                Cell = dest.Cell ?? dest.Map.GetRandomFreeCell();
                Direction = dest.Direction;
            }

            Stats.Health.DamageTaken = (Stats.Health.TotalMax - 1);

            Died?.Invoke(this);
        }

        private void OnFightEnded(CharacterFighter fighter)
        {
            FightEnded?.Invoke(this, fighter);
        }

        private void OnCharacterContextChanged(bool inFight)
        {
            ContextChanged?.Invoke(this, inFight);
        }

        public FighterRefusedReasonEnum CanRequestFight(Character target)
        {
            if (!target.IsInWorld || target.IsFighting() || target.IsSpectator() || target.IsBusy() || !target.IsAvailable(this, false))
                return FighterRefusedReasonEnum.OPPONENT_OCCUPIED;

            if (!IsInWorld || IsFighting() || IsSpectator() || IsBusy())
                return FighterRefusedReasonEnum.IM_OCCUPIED;

            if (target == this)
                return FighterRefusedReasonEnum.FIGHT_MYSELF;

            if (target.Map != Map || !Map.AllowChallenge)
                return FighterRefusedReasonEnum.WRONG_MAP;

            if (IsGhost())
                return FighterRefusedReasonEnum.GHOST_REFUSED;

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }

        public FighterRefusedReasonEnum CanAgress(Character target, bool bypassCheck = false)
        {
            if (target == this)
                return FighterRefusedReasonEnum.FIGHT_MYSELF;

            if (!target.PvPEnabled || !PvPEnabled)
                return FighterRefusedReasonEnum.INSUFFICIENT_RIGHTS;

            if (!target.IsInWorld || target.IsFighting() || target.IsSpectator() || target.IsBusy())
                return FighterRefusedReasonEnum.OPPONENT_OCCUPIED;

            if (!bypassCheck && (!IsInWorld || IsFighting() || IsSpectator() || IsBusy()))
                return FighterRefusedReasonEnum.IM_OCCUPIED;

            if (AlignmentSide <= AlignmentSideEnum.ALIGNMENT_NEUTRAL || target.AlignmentSide <= AlignmentSideEnum.ALIGNMENT_NEUTRAL)
                return FighterRefusedReasonEnum.WRONG_ALIGNMENT;

            if (target.AlignmentSide == AlignmentSide)
                return FighterRefusedReasonEnum.WRONG_ALIGNMENT;

            if (!bypassCheck && (target.Map != Map || !Map.AllowAggression))
                return FighterRefusedReasonEnum.WRONG_MAP;

            if (string.Equals(target.Client.IP, Client.IP) && !IsGameMaster())
                return FighterRefusedReasonEnum.MULTIACCOUNT_NOT_ALLOWED;

            if (Math.Abs(Level - target.Level) > 20)
                return FighterRefusedReasonEnum.INSUFFICIENT_RIGHTS;

            if (IsGhost() ||target.IsGhost())
                return FighterRefusedReasonEnum.GHOST_REFUSED;

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }

        public FighterRefusedReasonEnum CanAttack(TaxCollectorNpc target)
        {
            if (GuildMember != null && target.IsTaxCollectorOwner(GuildMember))
                return FighterRefusedReasonEnum.WRONG_GUILD;


            if (IsBusy() || IsFighting() || IsSpectator() || !IsInWorld)
                return FighterRefusedReasonEnum.IM_OCCUPIED;

            if (target.IsBusy() || target.IsFighting || !target.IsInWorld)
                return FighterRefusedReasonEnum.OPPONENT_OCCUPIED;

            if (target.Map != Map)
                return FighterRefusedReasonEnum.WRONG_MAP;

            if (IsGhost())
                return FighterRefusedReasonEnum.GHOST_REFUSED;

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }

        public FighterRefusedReasonEnum CanAttack(MonsterGroup group)
        {
            if (IsFighting() || IsSpectator() || !IsInWorld)
                return FighterRefusedReasonEnum.IM_OCCUPIED;

            if (!group.IsInWorld)
                return FighterRefusedReasonEnum.OPPONENT_OCCUPIED;

            if (group.Map != Map)
                return FighterRefusedReasonEnum.WRONG_MAP;

            if (IsGhost())
                return FighterRefusedReasonEnum.GHOST_REFUSED;

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }

        public CharacterFighter CreateFighter(FightTeam team)
        {
            if (IsFighting() || IsSpectator() || !IsInWorld)
                throw new Exception(string.Format("{0} is already in a fight", this));

            NextMap = Map; // we do not leave the map
            Map.Leave(this);
            StopRegen();

            if (IsInMovement)
                StopMove();

            ContextHandler.SendGameContextDestroyMessage(Client);
            ContextHandler.SendGameContextCreateMessage(Client, 2);

            Fighter = new CharacterFighter(this, team);

            ContextHandler.SendGameFightStartingMessage(Client, team.Fight.FightType, 0, 0);

            if (IsPartyLeader() && Party.RestrictFightToParty && team.Fighters.Count == 0 && !team.IsRestrictedToParty)
                team.ToggleOption(FightOptionsEnum.FIGHT_OPTION_SET_TO_PARTY_ONLY);

            OnCharacterContextChanged(true);

            return Fighter;
        }

        public FightSpectator CreateSpectator(IFight fight)
        {
            if (IsFighting() || IsSpectator() || !IsInWorld)
                throw new Exception(string.Format("{0} is already in a fight", this));

            if (!fight.CanSpectatorJoin(this))
                throw new Exception(string.Format("{0} cannot join fight in spectator", this));

            NextMap = Map; // we do not leave the map
            Map.Leave(this);
            StopRegen();

            if (IsInMovement)
                StopMove();

            ContextHandler.SendGameContextDestroyMessage(Client);
            ContextHandler.SendGameContextCreateMessage(Client, 2);

            ContextHandler.SendGameFightStartingMessage(Client, fight.FightType, fight.ChallengersTeam.Leader.Id, fight.DefendersTeam.Leader.Id);

            Spectator = new FightSpectator(this, fight);

            OnCharacterContextChanged(true);

            return Spectator;
        }

        private CharacterFighter RejoinFightAfterDisconnection(CharacterFighter oldFighter)
        {
            Map.Leave(this);
            Map = oldFighter.Map;
            NextMap = oldFighter.Character.NextMap;

            StopRegen();

            ContextHandler.SendGameContextDestroyMessage(Client);
            ContextHandler.SendGameContextCreateMessage(Client, 2);
            ContextRoleplayHandler.SendCurrentMapMessage(Client, Map.Id);
            ContextRoleplayHandler.SendMapComplementaryInformationsDataMessage(Client);

            oldFighter.RestoreFighterFromDisconnection(this);
            Fighter = oldFighter;

            ContextHandler.SendGameFightStartingMessage(Client, Fighter.Fight.FightType, Fighter.Fight.ChallengersTeam.Leader.Id,
                Fighter.Fight.DefendersTeam.Leader.Id);

            Fighter.Fight.RejoinFightFromDisconnection(Fighter);
            OnCharacterContextChanged(true);

            if (Fight.Challenge != null)
            {
                ContextHandler.SendChallengeInfoMessage(Client, Fight.Challenge);
                if (Fight.Challenge.Status != ChallengeStatusEnum.RUNNING)
                    ContextHandler.SendChallengeResultMessage(Client, Fight.Challenge);
            }

            return Fighter;
        }

        /// <summary>
        /// Rejoin the map after a fight
        /// </summary>
        public void RejoinMap()
        {
            if (!IsFighting() && !IsSpectator())
                return;

            if (Fighter != null)
                OnFightEnded(Fighter);

            if (GodMode)
                Stats.Health.DamageTaken = 0;
            else if (Fighter != null && (Fighter.HasLeft() && !Fighter.IsDisconnected || Fight.Losers == Fighter.Team) && !Fight.IsDeathTemporarily)
                OnDied();

            Fighter = null;
            Spectator = null;

            ContextHandler.SendGameContextDestroyMessage(Client);
            ContextHandler.SendGameContextCreateMessage(Client, 1);

            RefreshStats();

            OnCharacterContextChanged(false);
            StartRegen();

            if (Map == null)
                return;

            if (IsLoggedIn)
            {
                if (!NextMap.Area.IsRunning)
                    NextMap.Area.Start();

                NextMap.Area.ExecuteInContext(() =>
                {
                    if (IsLoggedIn)
                    {
                        LastMap = Map;
                        Map = NextMap;
                        Map.Enter(this);
                        NextMap = null;
                    }
                });
            }
            else
                SaveLater(); // if disconnected in fight we must save the change at the end of the fight
        }

        #endregion Fight

        #region Regen

        public bool IsRegenActive()
        {
            return RegenStartTime.HasValue;
        }

        public void StartRegen()
        {
            StartRegen((byte)(10f / Rates.RegenRate));
        }

        public void StartRegen(byte timePerHp)
        {
            if (IsRegenActive())
                StopRegen();

            if (PlayerLifeStatus == PlayerLifeStatusEnum.STATUS_TOMBSTONE)
                return;

            RegenStartTime = DateTime.Now;
            RegenSpeed = timePerHp;

            CharacterHandler.SendLifePointsRegenBeginMessage(Client, RegenSpeed);
        }

        public void StopRegen()
        {
            if (!IsRegenActive())
                return;

            var regainedLife = (int)Math.Floor((DateTime.Now - RegenStartTime).Value.TotalSeconds / (RegenSpeed / 10f));

            if (LifePoints + regainedLife > MaxLifePoints)
                regainedLife = MaxLifePoints - LifePoints;

            if (regainedLife > 0)
            {
                Stats.Health.DamageTaken -= regainedLife;
            }

            CharacterHandler.SendLifePointsRegenEndMessage(Client, regainedLife);

            RegenStartTime = null;
            RegenSpeed = 0;
            OnLifeRegened(regainedLife);
        }

        public void UpdateRegenedLife()
        {
            if (!IsRegenActive())
                return;

            var regainedLife = (int)Math.Floor((DateTime.Now - RegenStartTime).Value.TotalSeconds / (RegenSpeed / 10f));

            if (LifePoints + regainedLife > MaxLifePoints)
                regainedLife = MaxLifePoints - LifePoints;

            if (regainedLife > 0)
            {
                Stats.Health.DamageTaken -= regainedLife;
                CharacterHandler.SendUpdateLifePointsMessage(Client);
            }

            RegenStartTime = DateTime.Now;

            OnLifeRegened(regainedLife);
        }

        #endregion Regen

        #region Zaaps

        private ObjectPosition m_spawnPoint;

        public List<Map> KnownZaaps
        {
            get { return Record.KnownZaaps; }
        }

        public void DiscoverZaap(Map map)
        {
            if (!KnownZaaps.Contains(map))
                KnownZaaps.Add(map);

            BasicHandler.SendTextInformationMessage(Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 24);
            // new zaap
        }

        public void SetSpawnPoint(Map map)
        {
            Record.SpawnMap = map;
            m_spawnPoint = null;

            BasicHandler.SendTextInformationMessage(Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 6);
            // pos saved

            InteractiveHandler.SendZaapRespawnUpdatedMessage(Client);
        }

        public ObjectPosition GetSpawnPoint()
        {
            if (Record.SpawnMap == null)
                return Breed.GetStartPosition();

            if (m_spawnPoint != null)
                return m_spawnPoint;

            var map = Record.SpawnMap;

            if (map.Zaap == null)
                return new ObjectPosition(map, map.GetRandomFreeCell(), Direction);

            var cell = map.GetRandomAdjacentFreeCell(map.Zaap.Position.Point);
            var direction = map.Zaap.Position.Point.OrientationTo(new MapPoint(cell));

            return new ObjectPosition(map, cell, direction);
        }

        #endregion Zaaps

        #region Emotes


        private LimitedStack<Pair<Emote, DateTime>> m_playedEmotes = new LimitedStack<Pair<Emote, DateTime>>(5);
        private bool m_cancelEmote;

        public ReadOnlyCollection<EmotesEnum> Emotes => Record.Emotes.AsReadOnly();

        public override Pair<Emote, DateTime> LastEmoteUsed => !m_cancelEmote && m_playedEmotes.Count > 0 ? m_playedEmotes.Peek() : null;

        private Pair<Emote, DateTime> GetCurrentEmotePair() => LastEmoteUsed != null && (LastEmoteUsed.First.Duration == 0 || LastEmoteUsed.First.Persistancy || (DateTime.Now - LastEmoteUsed.Second) < TimeSpan.FromMilliseconds(LastEmoteUsed.First.Duration))
            ? LastEmoteUsed
            : null;

        public Emote GetCurrentEmote() => GetCurrentEmotePair()?.First;

        public bool CancelEmote(bool send = true)
        {
            var emote = GetCurrentEmote();

            if (emote == null)
                return false;

            m_cancelEmote = true;
            UpdateLook(emote, false, false);

            if (send)
                ContextRoleplayHandler.SendEmotePlayMessage(CharacterContainer.Clients, this, 0);

            RefreshActor();

            return true;
        }

        public bool HasEmote(EmotesEnum emote) => Emotes.Contains(emote);

        public void AddEmote(EmotesEnum emote)
        {
            if (HasEmote(emote))
                return;

            Record.Emotes.Add(emote);
            ContextRoleplayHandler.SendEmoteAddMessage(Client, (byte)emote);
        }

        public bool RemoveEmote(EmotesEnum emote)
        {
            var result = Record.Emotes.Remove(emote);

            if (result)
                ContextRoleplayHandler.SendEmoteRemoveMessage(Client, (byte)emote);

            return result;
        }

        public void PlayEmote(EmotesEnum emoteId, bool force = false)
        {
            var emote = ChatManager.Instance.GetEmote((int)emoteId);

            if (emote == null)
            {
                ContextRoleplayHandler.SendEmotePlayErrorMessage(Client, (byte)emoteId);
                return;
            }

            if (!HasEmote(emoteId) && !force)
            {
                ContextRoleplayHandler.SendEmotePlayErrorMessage(Client, (byte)emoteId);
                return;
            }

            var currentEmote = GetCurrentEmote();

            if (currentEmote != null)
            {
                CancelEmote();

                if (currentEmote == emote)
                {
                    return;
                }
            }

            m_cancelEmote = false;
            m_playedEmotes.Push(new Pair<Emote, DateTime>(emote, DateTime.Now));
            UpdateLook(emote, true, false);

            RefreshActor();

            ContextRoleplayHandler.SendEmotePlayMessage(CharacterContainer.Clients, this, emoteId);
        }

        #endregion Emotes

        #region FatalBlows

        public ReadOnlyCollection<SpellIdEnum> FatalBlows => Record.FatalBlows.AsReadOnly();

        public bool HasFatalBlow(SpellIdEnum fatal) => FatalBlows.Contains(fatal);

        public void AddFatalBlow(SpellIdEnum fatal)
        {
            if (HasFatalBlow(fatal))
                return;

            Record.FatalBlows.Add(fatal);
        }

        public bool RemoveFatalBlow(SpellIdEnum fatal) => Record.FatalBlows.Remove(fatal);

        #endregion FatalBlows

        #region Friend & Ennemies

        public FriendsBook FriendsBook
        {
            get;
            private set;
        }

        #endregion Friend & Ennemies

        #region Merchant

        private Merchant m_merchantToSpawn;

        public bool CanEnableMerchantMode(bool sendError = true)
        {
            if (MerchantBag.Count == 0)
            {
                if (sendError)
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 23);
                return false;
            }

            if (!Map.AllowHumanVendor)
            {
                if (sendError)
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 237);

                return false;
            }

            if (Map.IsMerchantLimitReached())
            {
                if (sendError)
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 25, Map.MaxMerchantsPerMap);
                return false;
            }

            if (!Map.IsCellFree(Cell.Id, this))
            {
                if (sendError)
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 24);
                return false;
            }

            if (Kamas >= MerchantBag.GetMerchantTax())
                return true;

            if (sendError)
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 76);

            return false;
        }

        public bool EnableMerchantMode()
        {
            if (!CanEnableMerchantMode())
                return false;

            m_merchantToSpawn = new Merchant(this);

            Inventory.SubKamas(MerchantBag.GetMerchantTax());
            MerchantManager.Instance.AddMerchantSpawn(m_merchantToSpawn.Record);
            MerchantManager.Instance.ActiveMerchant(m_merchantToSpawn);
            Client.Disconnect();

            return true;
        }

        private void CheckMerchantModeReconnection()
        {
            foreach (var merchant in MerchantManager.Instance.UnActiveMerchantFromAccount(Client.WorldAccount))
            {
                merchant.Save(WorldServer.Instance.DBAccessor.Database);

                if (merchant.Record.CharacterId != Id)
                    continue;

                MerchantBag.LoadMerchantBag(merchant.Bag);

                MerchantManager.Instance.RemoveMerchantSpawn(merchant.Record);
            }

            // if the merchant wasn't active
            var record = MerchantManager.Instance.GetMerchantSpawn(Id);
            if (record == null)
                return;

            MerchantManager.Instance.RemoveMerchantSpawn(record);
        }

        #endregion Merchant

        #region Bank

        public Bank Bank
        {
            get;
            private set;
        }

        #endregion Bank

        #region Drop Items

        public void GetDroppedItem(WorldObjectItem objectItem)
        {
            if (Inventory.IsFull(objectItem.Item, objectItem.Quantity))
            {
                //Vous ne pouvez pas porter autant d'objets.
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 285);

                //Le nombre maximum d'objets pour cet inventaire est déjà atteint.
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 6);
                return;
            }

            objectItem.Map.Leave(objectItem);
            Inventory.AddItem(objectItem.Item, objectItem.Effects, objectItem.Quantity);
        }

        public void DropItem(int itemId, int quantity)
        {
            if (quantity <= 0)
                return;

            var cell = Position.Point.GetAdjacentCells(x => Map.Cells[x].Walkable && Map.IsCellFree(x) && !Map.IsObjectItemOnCell(x)).FirstOrDefault();
            if (cell == null)
            {
                //Il n'y a pas assez de place ici.
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 145);
                return;
            }

            var item = Inventory.TryGetItem(itemId);
            if (item == null)
                return;

            if (item.IsLinkedToAccount() || item.IsLinkedToPlayer() || item.Template.Id == 20000) //Temporary block orb drop
                return;

            if (item.Stack < quantity)
            {
                //Vous ne possédez pas l'objet en quantité suffisante.
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 252);
                return;
            }

            Inventory.RemoveItem(item, quantity);

            var objectItem = new WorldObjectItem(item.Guid, Map, Map.Cells[cell.CellId], item.Template, item.Effects.Clone(), quantity);

            Map.Enter(objectItem);
        }

        #endregion Drop Items

        #region Debug

        public void ClearHighlight()
        {
            Client.Send(new DebugClearHighlightCellsMessage());
        }

        public Color HighlightCell(Cell cell)
        {
            var rand = new Random();
            var color = Color.FromArgb(0xFF << 24 | rand.Next(0xFFFFFF));
            HighlightCell(cell, color);

            return color;
        }

        public void HighlightCell(Cell cell, Color color)
        {
            Client.Send(new DebugHighlightCellsMessage(color.ToArgb() & 16777215, new[] { cell.Id }));
        }

        public Color HighlightCells(IEnumerable<Cell> cells)
        {
            var rand = new Random();
            var color = Color.FromArgb(0xFF << 24 | rand.Next(0xFFFFFF));

            HighlightCells(cells, color);
            return color;
        }

        public void HighlightCells(IEnumerable<Cell> cells, Color color)
        {
            Client.Send(new DebugHighlightCellsMessage(color.ToArgb() & 16777215, cells.Select(x => x.Id)));
        }

        #endregion Debug

        #endregion Actions

        #region Save & Load

        public bool IsLoggedIn
        {
            get;
            private set;
        }

        public bool IsAccountBlocked
        {
            get;
            private set;
        }

        public bool IsAuthSynced
        {
            get;
            set;
        }

        /// <summary>
        ///   Spawn the character on the map. It can be called once.
        /// </summary>
        public void LogIn()
        {
            if (IsInWorld)
                return;

            CharacterFighter fighter = null;
            if (Record.LeftFightId != null)
            {
                var fight = FightManager.Instance.GetFight(Record.LeftFightId.Value);

                if (fight != null)
                    fighter = fight.GetLeaver(Id);
            }

            if (fighter != null && fighter.IsDisconnected)
            {
                Map.Area.AddMessage(() =>
                {
                    RejoinFightAfterDisconnection(fighter);
                });
            }
            else
            {
                ContextHandler.SendGameContextDestroyMessage(Client);
                ContextHandler.SendGameContextCreateMessage(Client, 1);

                RefreshStats();

                Map.Area.AddMessage(() =>
                {
                    Map.Enter(this);

                    StartRegen();
                });
            }

            World.Instance.Enter(this);
            m_inWorld = true;

            SendServerMessage(Settings.MOTD, Settings.MOTDColor);

            IsLoggedIn = true;
            OnLoggedIn();
        }

        public void LogOut()
        {
            if (Area == null)
            {
                WorldServer.Instance.IOTaskPool.AddMessage(PerformLoggout);
            }
            else
            {
                Area.AddMessage(PerformLoggout);
            }
        }

        private void PerformLoggout()
        {
            lock (LoggoutSync)
            {
                IsLoggedIn = false;

                try
                {
                    OnLoggedOut();

                    if (!IsInWorld)
                        return;

                    DenyAllInvitations();

                    if (IsInRequest())
                        CancelRequest();

                    if (IsDialoging())
                        Dialog.Close();

                    if (ArenaParty != null)
                        LeaveParty(ArenaParty);

                    if (Party != null)
                        LeaveParty(Party);

                    if (Map != null && Map.IsActor(this))
                        Map.Leave(this);
                    else if (Area != null)
                        Area.Leave(this);

                    if (Map != null && m_merchantToSpawn != null)
                        Map.Enter(m_merchantToSpawn);

                    World.Instance.Leave(this);

                    m_inWorld = false;
                }
                catch (Exception ex)
                {
                    logger.Error("Cannot perfom OnLoggout actions, but trying to Save character : {0}", ex);
                }
                finally
                {
                    BlockAccount();
                    WorldServer.Instance.IOTaskPool.AddMessage(
                        () =>
                        {
                            try
                            {
                                SaveNow();
                                UnLoadRecord();
                            }
                            finally
                            {
                                Delete();
                            }
                        });

                }
            }
        }

        public void SaveLater()
        {
            BlockAccount();
            WorldServer.Instance.IOTaskPool.AddMessage(SaveNow);
        }

        internal void SaveNow()
        {
            try
            {
                WorldServer.Instance.IOTaskPool.EnsureContext();
                var database = WorldServer.Instance.DBAccessor.Database;

                lock (SaveSync)
                {
                    using (var transaction = database.GetTransaction())
                    {
                        Inventory.Save(database, false);
                        Bank.Save(database);
                        MerchantBag.Save(database);
                        Spells.Save();
                        Shortcuts.Save();
                        FriendsBook.Save();
                        Jobs.Save(database);
                        IdolInventory.Save();

                        SaveMounts();

                        m_record.MapId = NextMap != null ? NextMap.Id : Map.Id;
                        m_record.CellId = Cell.Id;
                        m_record.Direction = Direction;

                        m_record.AP = Stats[PlayerFields.AP].Base;
                        m_record.MP = Stats[PlayerFields.MP].Base;
                        m_record.Strength = Stats[PlayerFields.Strength].Base;
                        m_record.Agility = Stats[PlayerFields.Agility].Base;
                        m_record.Chance = Stats[PlayerFields.Chance].Base;
                        m_record.Intelligence = Stats[PlayerFields.Intelligence].Base;
                        m_record.Wisdom = Stats[PlayerFields.Wisdom].Base;
                        m_record.Vitality = Stats[PlayerFields.Vitality].Base;

                        m_record.PermanentAddedStrength = (short)Stats[PlayerFields.Strength].Additional;
                        m_record.PermanentAddedAgility = (short)Stats[PlayerFields.Agility].Additional;
                        m_record.PermanentAddedChance = (short)Stats[PlayerFields.Chance].Additional;
                        m_record.PermanentAddedIntelligence = (short)Stats[PlayerFields.Intelligence].Additional;
                        m_record.PermanentAddedWisdom = (short)Stats[PlayerFields.Wisdom].Additional;
                        m_record.PermanentAddedVitality = (short)Stats[PlayerFields.Vitality].Additional;

                        m_record.BaseHealth = Stats.Health.Base;
                        m_record.DamageTaken = Stats.Health.DamageTaken;


                        database.Update(m_record);
                        database.Update(Client.WorldAccount);

                        transaction.Complete();
                    }
                }

                if (IsAuthSynced)
                    OnSaved();
                else
                {
                    IPCAccessor.Instance.SendRequest<CommonOKMessage>(new UpdateAccountMessage(Account),
                        msg =>
                        {
                            OnSaved();
                        });
                }
            }
            catch
            {
                UnBlockAccount();
                throw;
            }
        }

        public void LoadRecord()
        {
            Breed = BreedManager.Instance.GetBreed(BreedId);
            Head = BreedManager.Instance.GetHead(Record.Head);
            var map = World.Instance.GetMap(m_record.MapId);

            if (map == null)
            {
                map = World.Instance.GetMap(Breed.StartMap);
                m_record.CellId = Breed.StartCell;
                m_record.Direction = Breed.StartDirection;
            }

            Position = new ObjectPosition(
                map,
                map.Cells[m_record.CellId],
                m_record.Direction);

            Stats = new StatsFields(this);
            Stats.Initialize(m_record);
            Level = ExperienceManager.Instance.GetCharacterLevel(Experience);
            LowerBoundExperience = ExperienceManager.Instance.GetCharacterLevelExperience(Level);
            UpperBoundExperience = ExperienceManager.Instance.GetCharacterNextLevelExperience(Level);

            AlignmentGrade = (sbyte)ExperienceManager.Instance.GetAlignementGrade(m_record.Honor);
            LowerBoundHonor = ExperienceManager.Instance.GetAlignementGradeHonor((byte)AlignmentGrade);
            UpperBoundHonor = ExperienceManager.Instance.GetAlignementNextGradeHonor((byte)AlignmentGrade);

            Inventory = new Inventory(this);
            Inventory.LoadInventory();
            Inventory.LoadPresets();

            IdolInventory = new IdolInventory(this);

            Bank = new Bank(this);
            Bank.LoadRecord();

            MerchantBag = new CharacterMerchantBag(this);
            CheckMerchantModeReconnection();
            MerchantBag.LoadMerchantBag();

            GuildMember = GuildManager.Instance.TryGetGuildMember(Id);

            UpdateLook(false);

            LoadMounts();

            Spells = new SpellInventory(this);
            Spells.LoadSpells();

            Shortcuts = new ShortcutBar(this);
            Shortcuts.Load();

            FriendsBook = new FriendsBook(this);
            FriendsBook.Load();

            ChatHistory = new ChatHistory(this);

            LoadQuests();

            Jobs = new JobsCollection(this);
            Jobs.LoadJobs();

            m_recordLoaded = true;
        }

        private void UnLoadRecord()
        {
            if (!m_recordLoaded)
                return;

            m_recordLoaded = false;
        }

        private void BlockAccount()
        {
            AccountManager.Instance.BlockAccount(Client.WorldAccount, this);
            IsAccountBlocked = true;
        }

        private void UnBlockAccount()
        {
            if (!IsAccountBlocked)
                return;

            AccountManager.Instance.UnBlockAccount(Client.WorldAccount);
            IsAccountBlocked = false;

            OnAccountUnblocked();
        }

        #endregion Save & Load

        #region Exceptions

        private readonly List<KeyValuePair<string, Exception>> m_commandsError = new List<KeyValuePair<string, Exception>>();
        private Mount m_equippedMount;
        private ActorLook m_look;

        public List<KeyValuePair<string, Exception>> CommandsErrors => m_commandsError;

        #endregion Exceptions

        #region Network

        #region GameRolePlayCharacterInformations

        public override GameContextActorInformations GetGameContextActorInformations(Character character)
        {
            return new GameRolePlayCharacterInformations(
                Id,
                Look.GetEntityLook(),
                GetEntityDispositionInformations(),
                Name,
                GetHumanInformations(),
                Account.Id,
                GetActorAlignmentInformations());
        }

        #endregion GameRolePlayCharacterInformations

        #region ActorAlignmentInformations

        public ActorAlignmentInformations GetActorAlignmentInformations()
        {
            return new ActorAlignmentInformations(
                (sbyte)(PvPEnabled ? AlignmentSide : 0),
                (sbyte)(PvPEnabled ? AlignmentValue : 0),
                (sbyte)(PvPEnabled ? AlignmentGrade : 0),
                CharacterPower);
        }

        #endregion ActorAlignmentInformations

        #region ActorExtendedAlignmentInformations

        public ActorExtendedAlignmentInformations GetActorAlignmentExtendInformations()
        {
            return new ActorExtendedAlignmentInformations(
                (sbyte)AlignmentSide,
                AlignmentValue,
                AlignmentGrade,
                CharacterPower,
                (short)Honor,
                (short)LowerBoundHonor,
                (short)UpperBoundHonor,
                (PvPEnabled ? (sbyte)AggressableStatusEnum.PvP_ENABLED_AGGRESSABLE : (sbyte)AggressableStatusEnum.NON_AGGRESSABLE));
        }

        #endregion ActorExtendedAlignmentInformations

        #region CharacterBaseInformations

        public CharacterBaseInformations GetCharacterBaseInformations()
            => new CharacterBaseInformations(
                Id,
                Name,
                Level,
                Look.GetEntityLook(),
                (sbyte)BreedId,
                Sex == SexTypeEnum.SEX_FEMALE);

        public CharacterMinimalPlusLookInformations GetCharacterMinimalPlusLookInformations()
            => new CharacterMinimalPlusLookInformations(
                Id,
                Name,
                Level,
                Look.GetEntityLook());

        public CharacterCharacteristicsInformations GetCharacterCharacteristicsInformations()
            => new CharacterCharacteristicsInformations(
                        Experience, // EXPERIENCE
                        LowerBoundExperience, // EXPERIENCE level floor
                        UpperBoundExperience, // EXPERIENCE nextlevel floor

                        Kamas, // Amount of kamas.

                        (short)StatsPoints, // Stats points
                        0, // Additionnal points
                        (short)SpellsPoints, // Spell points

                        // Alignment
                        GetActorAlignmentExtendInformations(),
                        Stats.Health.Total, // Life points
                        Stats.Health.TotalMax, // Max Life points

                        Energy, // Energy points
                        EnergyMax, // maxEnergyPoints

                        (short)Stats[PlayerFields.AP]
                                    .Total, // actionPointsCurrent
                        (short)Stats[PlayerFields.MP]
                                    .Total, // movementPointsCurrent

                        Stats[PlayerFields.Initiative],
                        Stats[PlayerFields.Prospecting],
                        Stats[PlayerFields.AP],
                        Stats[PlayerFields.MP],
                        Stats[PlayerFields.Strength],
                        Stats[PlayerFields.Vitality],
                        Stats[PlayerFields.Wisdom],
                        Stats[PlayerFields.Chance],
                        Stats[PlayerFields.Agility],
                        Stats[PlayerFields.Intelligence],
                        Stats[PlayerFields.Range],
                        Stats[PlayerFields.SummonLimit],
                        Stats[PlayerFields.DamageReflection],
                        Stats[PlayerFields.CriticalHit],
                        (short)Inventory.WeaponCriticalHit,
                        Stats[PlayerFields.CriticalMiss],
                        Stats[PlayerFields.HealBonus],
                        Stats[PlayerFields.DamageBonus],
                        Stats[PlayerFields.WeaponDamageBonus],
                        Stats[PlayerFields.DamageBonusPercent],
                        Stats[PlayerFields.TrapBonus],
                        Stats[PlayerFields.TrapBonusPercent],
                        Stats[PlayerFields.GlyphBonusPercent],
                        Stats[PlayerFields.RuneBonusPercent],
                        Stats[PlayerFields.PermanentDamagePercent],
                        Stats[PlayerFields.TackleBlock],
                        Stats[PlayerFields.TackleEvade],
                        Stats[PlayerFields.APAttack],
                        Stats[PlayerFields.MPAttack],
                        Stats[PlayerFields.PushDamageBonus],
                        Stats[PlayerFields.CriticalDamageBonus],
                        Stats[PlayerFields.NeutralDamageBonus],
                        Stats[PlayerFields.EarthDamageBonus],
                        Stats[PlayerFields.WaterDamageBonus],
                        Stats[PlayerFields.AirDamageBonus],
                        Stats[PlayerFields.FireDamageBonus],
                        Stats[PlayerFields.DodgeAPProbability],
                        Stats[PlayerFields.DodgeMPProbability],
                        Stats[PlayerFields.NeutralResistPercent],
                        Stats[PlayerFields.EarthResistPercent],
                        Stats[PlayerFields.WaterResistPercent],
                        Stats[PlayerFields.AirResistPercent],
                        Stats[PlayerFields.FireResistPercent],
                        Stats[PlayerFields.NeutralElementReduction],
                        Stats[PlayerFields.EarthElementReduction],
                        Stats[PlayerFields.WaterElementReduction],
                        Stats[PlayerFields.AirElementReduction],
                        Stats[PlayerFields.FireElementReduction],
                        Stats[PlayerFields.PushDamageReduction],
                        Stats[PlayerFields.CriticalDamageReduction],
                        Stats[PlayerFields.PvpNeutralResistPercent],
                        Stats[PlayerFields.PvpEarthResistPercent],
                        Stats[PlayerFields.PvpWaterResistPercent],
                        Stats[PlayerFields.PvpAirResistPercent],
                        Stats[PlayerFields.PvpFireResistPercent],
                        Stats[PlayerFields.PvpNeutralElementReduction],
                        Stats[PlayerFields.PvpEarthElementReduction],
                        Stats[PlayerFields.PvpWaterElementReduction],
                        Stats[PlayerFields.PvpAirElementReduction],
                        Stats[PlayerFields.PvpFireElementReduction],
                        new List<CharacterSpellModification>(),
                        0);

        #endregion CharacterBaseInformations

        #region PartyMemberInformations

        public PartyInvitationMemberInformations GetPartyInvitationMemberInformations()
            => new PartyInvitationMemberInformations(
                Id,
                Name,
                Level,
                Look.GetEntityLook(),
                (sbyte)BreedId,
                Sex == SexTypeEnum.SEX_FEMALE,
                (short)Map.Position.X,
                (short)Map.Position.Y,
                Map.Id,
                (short)Map.SubArea.Id,
                new PartyCompanionBaseInformations[0]);

        public PartyMemberInformations GetPartyMemberInformations()
            => new PartyMemberInformations(
                Id,
                Name,
                Level,
                Look.GetEntityLook(),
                (sbyte)BreedId,
                Sex == SexTypeEnum.SEX_FEMALE,
                LifePoints,
                MaxLifePoints,
                (short)Stats[PlayerFields.Prospecting].Total,
                RegenSpeed,
                (short)Stats[PlayerFields.Initiative].Total,
                (sbyte)AlignmentSide,
                (short)Map.Position.X,
                (short)Map.Position.Y,
                Map.Id,
                (short)SubArea.Id,
                Status,
                new PartyCompanionMemberInformations[0]);

        public PartyGuestInformations GetPartyGuestInformations(Party party)
        {
            if (!m_partyInvitations.ContainsKey(party.Id))
                return new PartyGuestInformations();

            var invitation = m_partyInvitations[party.Id];

            return new PartyGuestInformations(
                Id,
                invitation.Source.Id,
                Name,
                Look.GetEntityLook(),
                (sbyte)BreedId,
                Sex == SexTypeEnum.SEX_FEMALE,
                Status,
                new PartyCompanionMemberInformations[0]);
        }

        public PartyMemberArenaInformations GetPartyMemberArenaInformations()
            => new PartyMemberArenaInformations(
                Id,
                Name,
                Level,
                Look.GetEntityLook(),
                (sbyte)BreedId,
                Sex == SexTypeEnum.SEX_FEMALE,
                LifePoints,
                MaxLifePoints,
                (short)Stats[PlayerFields.Prospecting].Total,
                RegenSpeed,
                (short)Stats[PlayerFields.Initiative].Total,
                (sbyte)AlignmentSide,
                (short)Map.Position.X,
                (short)Map.Position.Y,
                Map.Id,
                (short)SubArea.Id,
                Status,
                new PartyCompanionMemberInformations[0],
                (short)ArenaRank);

        #endregion PartyMemberInformations

        public override ActorRestrictionsInformations GetActorRestrictionsInformations()
        {
            return new ActorRestrictionsInformations(
                                !Map.AllowAggression || IsGhost(), // cantBeAgressed
                                !Map.AllowChallenge || IsGhost(), // cantBeChallenged
                                !Map.AllowExchangesBetweenPlayers || IsGhost(), // cantTrade
                                IsGhost(), // cantBeAttackedByMutant
                                false, // cantRun
                                false, // forceSlowWalk
                                false, // cantMinimize
                                PlayerLifeStatus == PlayerLifeStatusEnum.STATUS_TOMBSTONE, // cantMove

                                !Map.AllowAggression || IsGhost(), // cantAggress
                                IsGhost(), // cantChallenge
                                IsGhost(), // cantExchange
                                IsGhost(), // cantAttack
                                false, // cantChat
                                IsGhost(), // cantBeMerchant
                                IsGhost(), // cantUseObject
                                IsGhost(), // cantUseTaxCollector

                                IsGhost(), // cantUseInteractive
                                IsGhost(), // cantSpeakToNPC
                                false, // cantChangeZone
                                IsGhost(), // cantAttackMonster
                                false // cantWalk8Directions
                                );
        }

        public override HumanInformations GetHumanInformations()
        {
            var human = base.GetHumanInformations();

            var options = new List<HumanOption>();

            if (Guild != null)
                options.Add(new HumanOptionGuild(Guild.GetGuildInformations()));

            if (SelectedTitle != null)
                options.Add(new HumanOptionTitle(SelectedTitle.Value, string.Empty));

            if (SelectedOrnament != null)
                options.Add(new HumanOptionOrnament(SelectedOrnament.Value));

            if (LastEmoteUsed != null)
                options.Add(new HumanOptionEmote((byte)LastEmoteUsed.First.Id, LastEmoteUsed.Second.GetUnixTimeStampLong()));

            if (LastSkillUsed != null)
                options.Add(new HumanOptionSkillUse(LastSkillUsed.InteractiveObject.Id, (short)LastSkillUsed.SkillTemplate.Id, LastSkillUsed.SkillEndTime.GetUnixTimeStampLong()));

            human.options = options;
            return human;
        }

        #endregion Network

        public CharacterRecord Record => m_record;

        public override bool CanBeSee(WorldObject byObj) => base.CanBeSee(byObj) && (byObj == this || !Invisible);

        protected override void OnDisposed()
        {
            if (FriendsBook != null)
                FriendsBook.Dispose();

            if (Inventory != null)
                Inventory.Dispose();

            base.OnDisposed();
        }

        public override string ToString() => string.Format("{0} ({1})", Name, Id);

    }
}