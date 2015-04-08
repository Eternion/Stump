using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using MongoDB.Bson;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.BaseServer.Logging;
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
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Exchanges;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Items.Player.Custom;
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
using Stump.Server.WorldServer.Handlers.Compass;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Handlers.Context.RolePlay.Party;
using Stump.Server.WorldServer.Handlers.Guilds;
using Stump.Server.WorldServer.Handlers.Moderation;
using Stump.Server.WorldServer.Handlers.Titles;
using GuildMember = Stump.Server.WorldServer.Game.Guilds.GuildMember;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Characters
{
    public sealed class Character : Humanoid, IStatsOwner, IInventoryOwner, ICommandsUser
    {
        [Variable]
        private const ushort HonorLimit = 16000;

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly CharacterRecord m_record;
        private bool m_recordLoaded;

        public Character(CharacterRecord record, WorldClient client)
        {
            m_record = record;
            Client = client;
            SaveSync = new object();
            LoggoutSync = new object();

            LoadRecord();
        }

        #region Events

        public event Action<Character> LoggedIn;

        private void OnLoggedIn()
        {
            if (GuildMember != null)
                GuildMember.OnCharacterConnected(this);

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
                    Inventory.RemoveItem(item, true, false);
            }

            var handler = LoggedIn;
            if (handler != null) handler(this);
        }

        public event Action<Character> LoggedOut;

        private void OnLoggedOut()
        {
            if (GuildMember != null)
                GuildMember.OnCharacterDisconnected(this);

            if (TaxCollectorDefendFight != null)
                TaxCollectorDefendFight.RemoveDefender(this);

            if (ArenaManager.Instance.IsInQueue(this))
                ArenaManager.Instance.RemoveFromQueue(this);

            if (ArenaPopup != null)
                ArenaPopup.Deny();

            var document = new BsonDocument
            {
                { "AcctId", Client.Account.Id },
                { "CharacterId", Id },
                { "IPAddress", Client.IP },
                { "Action", "Loggout" },
                { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
            };

            MongoLogger.Instance.Insert("characters_connections", document);

            var handler = LoggedOut;
            if (handler != null) handler(this);
        }

        public event Action<Character> Saved;
        private bool m_isLocalSaving;

        public void OnSaved()
        {
            UnBlockAccount();

            var handler = Saved;
            if (handler != null) handler(this);
        }

        public event Action<Character, int> LifeRegened;

        private void OnLifeRegened(int regenedLife)
        {
            var handler = LifeRegened;
            if (handler != null) handler(this, regenedLife);
        }

        public event Action<Character> AccountUnblocked;

        private void OnAccountUnblocked()
        {
            Action<Character> handler = AccountUnblocked;
            if (handler != null) handler(this);
        }

        #endregion

        #region Properties

        public WorldClient Client
        {
            get;
            private set;
        }

        public AccountData Account
        {
            get { return Client.Account; }
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

        private int m_earnKamasInMerchant;

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

        #endregion

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

        #endregion

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

        public NpcShopDialogLogger NpcShopDialog
        {
            get { return Dialog as NpcShopDialogLogger; }
        }

        public ZaapDialog ZaapDialog
        {
            get { return Dialog as ZaapDialog; }
        }

        public ZaapiDialog ZaapiDialog
        {
            get { return Dialog as ZaapiDialog; }
        }

        public MerchantShopDialog MerchantShopDialog
        {
            get { return Dialog as MerchantShopDialog; }
        }

        public RequestBox RequestBox
        {
            get;
            private set;
        }

        public void SetDialoger(IDialoger dialoger)
        {
            Dialoger = dialoger;
        }

        public void SetDialog(IDialog dialog)
        {
            if (Dialog != null)
            {
                Dialog.Close();
            }

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

        public bool IsBusy()
        {
            return IsInRequest() || IsDialoging();
        }

        public bool IsDialoging()
        {
            return Dialog != null;
        }

        public bool IsInRequest()
        {
            return RequestBox != null;
        }

        public bool IsRequestSource()
        {
            return IsInRequest() && RequestBox.Source == this;
        }

        public bool IsRequestTarget()
        {
            return IsInRequest() && RequestBox.Target == this;
        }

        public bool IsTalkingWithNpc()
        {
            return Dialog is NpcDialog;
        }

        public bool IsInZaapDialog()
        {
            return Dialog is ZaapDialog;
        }

        public bool IsInZaapiDialog()
        {
            return Dialog is ZaapiDialog;
        }

        #endregion

        #region Party

        private readonly Dictionary<int, PartyInvitation> m_partyInvitations
            = new Dictionary<int, PartyInvitation>();

        private Character m_followedCharacter;

        public Party Party
        {
            get;
            private set;
        }

        public ArenaParty ArenaParty
        {
            get;
            private set;
        }

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

        public bool IsPartyLeader(int id)
        {
            return IsInParty(id) && GetParty(id).Leader == this;
        }

        public Party GetParty(int id)
        {
            if (Party != null && Party.Id == id)
                return Party;

            if (ArenaParty != null && ArenaParty.Id == id)
                return ArenaParty;

            return null;
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
                    ArenaParty = (ArenaParty) party;
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
            }        }

        #endregion

        #region Trade

        public IExchange Exchange
        {
            get { return Dialog as IExchange; }
        }
        public Exchanger Exchanger
        {
            get { return Dialoger as Exchanger; }
        }
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

        #endregion

        #region Titles & Ornaments

        public ReadOnlyCollection<short> Titles
        {
            get { return Record.Titles.AsReadOnly(); }
        }

        public ReadOnlyCollection<short> Ornaments
        {
            get
            {
                return Record.Ornaments.AsReadOnly();
            }
        }


        public short? SelectedTitle
        {
            get { return Record.TitleId; }
            private set { Record.TitleId = value; }
        }

        public bool HasTitle(short title)
        {
            return Record.Titles.Contains(title);
        }

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

        #endregion

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

        public ActorLook RealLook
        {
            get { return m_record.EntityLook; }
            private set
            {
                m_record.EntityLook = value;
                base.Look = value;
            }
        }

        public override ActorLook Look
        {
            get
            {
                var playerLook = CustomLookActivated && CustomLook != null ? CustomLook.Clone() : RealLook.Clone();

                var equipedMount = GetEquipedMount();
                if (equipedMount != -1)
                {
                    var mountLook = new ActorLook { BonesID = (short)equipedMount };

                    //KramKram
                    if (equipedMount == 1792)
                    {
                        Color color1;
                        Color color2;

                        playerLook.Colors.TryGetValue(3, out color1);
                        playerLook.Colors.TryGetValue(4, out color2);

                        mountLook.AddColor(1, color1);
                        mountLook.AddColor(2, color2);
                    }

                    playerLook.BonesID = 2;
                    mountLook.SetRiderLook(playerLook);

                    playerLook = mountLook;
                }
                else if (IsRiding())
                {
                    var mountLook = Mount.Model.EntityLook.Clone();

                    if (Mount.Behaviors.Contains(MountBehaviorEnum.Caméléone))
                    {
                        Color color1;
                        Color color2;
                        Color color3;

                        playerLook.Colors.TryGetValue(3, out color1);
                        playerLook.Colors.TryGetValue(4, out color2);
                        playerLook.Colors.TryGetValue(5, out color3);

                        mountLook.SetColors(color1, color2, color3);
                    }

                    playerLook.BonesID = 2;
                    mountLook.SetRiderLook(playerLook);

                    playerLook = mountLook;
                }

                if (!IsInMovement && Direction == DirectionsEnum.DIRECTION_SOUTH && Level >= 100)
                {
                    var auraLook = new ActorLook
                    {
                        BonesID = Level == 200 ? (short)170 : (short)169
                    };

                    playerLook.AddSubLook(new SubActorLook(0, SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_BASE_FOREGROUND, auraLook));
                }

                return playerLook;
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
            private set;
        }

        public bool Invisible
        {
            get;
            private set;
        }

        public bool IsAway
        {
            get;
            private set;
        }

        public bool ToggleAway()
        {
            IsAway = !IsAway;
            return IsAway;
        }

        public bool ToggleInvisibility(bool toggle)
        {
            Invisible = toggle;

            if (!IsInFight())
                Map.Refresh(this);

            SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, toggle ? (short) 236 : (short) 237);

            return Invisible;
        }

        public bool ToggleInvisibility()
        {
            return ToggleInvisibility(!Invisible);
        }

        public void UpdateLook(bool send = true)
        {
            var skins = new List<short>(Breed.GetLook(Sex).Skins);
            skins.AddRange(Head.Skins);
            skins.AddRange(Inventory.GetItemsSkins());

            RealLook.SetSkins(skins.ToArray());

            var petSkin = Inventory.GetPetSkin();

            if (petSkin != null && petSkin.Item1.HasValue && petSkin.Item2)
                RealLook.SetPetSkin(petSkin.Item1.Value);
            else
                RealLook.RemovePets();
                
            if (send)
                RefreshActor();
        }

        public void RefreshActor()
        {
            if (Map != null)
            {
                Map.Area.ExecuteInContext(() =>
                    Map.Refresh(this)
                    );
            }
        }

        #endregion

        #region Stats

        #region Delegates

        public delegate void LevelChangedHandler(Character character, byte currentLevel, int difference);

        public delegate void GradeChangedHandler(Character character, sbyte currentGrade, int difference);

        #endregion

        #region Levels

        public byte Level
        {
            get;
            private set;
        }

        public long Experience
        {
            get { return RealExperience - PrestigeRank*ExperienceManager.Instance.HighestCharacterExperience; }
            private set
            {
                RealExperience = PrestigeRank*ExperienceManager.Instance.HighestCharacterExperience + value;
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
                level = (byte) (levelAdded + Level);

            var experience = ExperienceManager.Instance.GetCharacterLevelExperience(level);

            Experience = experience;
        }

        public void LevelDown(byte levelRemoved)
        {
            byte level;

            if (Level - levelRemoved < 1)
                level = 1;
            else
                level = (byte) (Level - levelRemoved);

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
            Experience += (long) amount;
        }

        #endregion

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
            set { m_record.Energy = value; }
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

        #region Restat

        public short PermanentAddedStrength
        {
            get { return m_record.PermanentAddedStrength; }
            set { m_record.PermanentAddedStrength = value; }
        }

        public short PermanentAddedChance
        {
            get { return m_record.PermanentAddedChance; }
            set { m_record.PermanentAddedChance = value; }
        }

        public short PermanentAddedVitality
        {
            get { return m_record.PermanentAddedVitality; }
            set { m_record.PermanentAddedVitality = value; }
        }

        public short PermanentAddedWisdom
        {
            get { return m_record.PermanentAddedWisdom; }
            set { m_record.PermanentAddedWisdom = value; }
        }

        public short PermanentAddedIntelligence
        {
            get { return m_record.PermanentAddedIntelligence; }
            set { m_record.PermanentAddedIntelligence = value; }
        }

        public short PermanentAddedAgility
        {
            get { return m_record.PermanentAddedAgility; }
            set { m_record.PermanentAddedAgility = value; }
        }

        public bool CanRestat
        {
            get { return m_record.CanRestat; }
            set { m_record.CanRestat = value; }
        }

        #endregion

        public event LevelChangedHandler LevelChanged;

        private void OnLevelChanged(byte currentLevel, int difference)
        {
            if (difference > 0)
            {
                SpellsPoints += (ushort) difference;
                StatsPoints += (ushort) (difference*5);
            }

            Stats.Health.Base += (short) (difference*5);
            Stats.Health.DamageTaken = 0;

            if (currentLevel >= 100 && currentLevel - difference < 100)
            {
                Stats.AP.Base++;
                AddOrnament((short) OrnamentEnum.NIVEAU_100);
            }
            else if (currentLevel < 100 && currentLevel - difference >= 100)
            {
                Stats.AP.Base--;
                RemoveOrnament((short) OrnamentEnum.NIVEAU_100);
            }

            if (currentLevel >= 160 && currentLevel - difference < 160)
                AddOrnament((short) OrnamentEnum.NIVEAU_160);
            else if (currentLevel < 160 && currentLevel - difference >= 160)
                RemoveOrnament((short) OrnamentEnum.NIVEAU_160);

            if (currentLevel >= 200 && currentLevel - difference < 200)
                AddOrnament((short) OrnamentEnum.NIVEAU_200);
            else if (currentLevel < 200 && currentLevel - difference >= 200)
                RemoveOrnament((short) OrnamentEnum.NIVEAU_200);


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
                        (short) spell.Spell);
                }
            }

            RefreshStats();

            if (currentLevel > 1)
            {
                if (difference > 0)
                    CharacterHandler.SendCharacterLevelUpMessage(Client, currentLevel);
                CharacterHandler.SendCharacterLevelUpInformationMessage(Map.Clients, this, currentLevel);
            }

            var handler = LevelChanged;

            if (handler != null)
                handler(this, currentLevel, difference);
        }

        public void ResetStats()
        {
            Stats.Agility.Base = PermanentAddedAgility;
            Stats.Strength.Base = PermanentAddedStrength;
            Stats.Vitality.Base = PermanentAddedVitality;
            Stats.Wisdom.Base = PermanentAddedWisdom;
            Stats.Intelligence.Base = PermanentAddedIntelligence;
            Stats.Chance.Base = PermanentAddedChance;

            var newPoints = (Level-1)*5;
            StatsPoints = (ushort)newPoints;

            RefreshStats();
            SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 15, newPoints);
        }

        public void RefreshStats()
        {
            if (IsRegenActive())
                UpdateRegenedLife();

            CharacterHandler.SendCharacterStatsListMessage(Client);
        }

        public void ToggleGodMode(bool state)
        {
            GodMode = state;
        }

        public bool IsGameMaster()
        {
            return UserGroup.IsGameMaster;
        }

        #endregion

        #region Mount

        public Mount Mount
        {
            get;
            set;
        }

        public int GetEquipedMount()
        {
            var petSkin = Inventory.GetPetSkin();
            return (petSkin != null && petSkin.Item1.HasValue && !petSkin.Item2) ? petSkin.Item1.Value : -1;
        }

        public bool HasEquipedMount()
        {
            return Mount != null;
        }

        public bool IsRiding()
        {
            return HasEquipedMount() && Mount.IsRiding;
        }

        #endregion

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

        #endregion

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
                m_record.Honor = value > ExperienceManager.Instance.HighestGradeHonor ? ExperienceManager.Instance.HighestGradeHonor : value;
                if ((value > LowerBoundHonor && value < UpperBoundHonor))
                    return;

                var lastGrade = AlignmentGrade;

                AlignmentGrade = (sbyte) ExperienceManager.Instance.GetAlignementGrade(m_record.Honor);

                LowerBoundHonor = ExperienceManager.Instance.GetAlignementGradeHonor((byte) AlignmentGrade);
                UpperBoundHonor = ExperienceManager.Instance.GetAlignementNextGradeHonor((byte) AlignmentGrade);

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
            Honor += (Honor + amount) >= HonorLimit ? HonorLimit : amount;
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

            var handler = GradeChanged;

            if (handler != null)
                handler(this, currentLevel, difference);
        }

        public event Action<Character, bool> PvPToggled;

        private void OnPvPToggled()
        {
            foreach (var item in Inventory.GetItems(CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD).Where(item => !item.AreConditionFilled(this)))
            {
                Inventory.MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
            }

            Map.Refresh(this);
            RefreshStats();

            var handler = PvPToggled;

            if (handler != null)
                handler(this, PvPEnabled);
        }

        public event Action<Character, AlignmentSideEnum> AligmenentSideChanged;

        private void OnAligmenentSideChanged()
        {
            TogglePvPMode(false);
            Map.Refresh(this);

            Honor = 0;
            Dishonor = 0;

            var handler = AligmenentSideChanged;

            if (handler != null)
                handler(this, AlignmentSide);
        }

        #endregion

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

        #endregion

        #region Shortcuts

        public ShortcutBar Shortcuts
        {
            get;
            private set;
        }

        #endregion

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

        #endregion

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
                (int) time.TotalMinutes);
        }

        public void Mute(TimeSpan time)
        {
            MuteUntil = DateTime.Now + time;
            // Le principe de précaution vous a rendu muet pour %1 seconde(s).
            SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 123, (int) time.TotalSeconds);
        }

        public void UnMute()
        {
            MuteUntil = null;
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

        #endregion

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
        
        public bool IsPrestigeMax()
        {
            return PrestigeRank == PrestigeManager.PrestigeTitles.Length;
        }

        public PrestigeItem GetPrestigeItem()
        {
            return Inventory.TryGetItem(PrestigeManager.BonusItem) as PrestigeItem;
        }

        public PrestigeItem CreatePrestigeItem()
        {
            return (PrestigeItem) Inventory.AddItem(PrestigeManager.BonusItem, 1, false);
        }

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
                "les bonus s'appliquent sans équiper l'objet. \r\nVous devez vous reconnecter pour actualiser votre niveau.");

            foreach (var equippedItem in Inventory.ToArray())
                Inventory.MoveItem(equippedItem, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

            var points = (Spells.CountSpentBoostPoint() + SpellsPoints) - (Level-1);

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
                else Inventory.RemoveItem(item, true, false);
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
                Inventory.RemoveItem(item, true, false);
            }
        }

        #endregion

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

            if (Fight is ArenaFight)
            {
                if (send)
                    //Vous êtes déjà en combat de Kolizéum.
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 334);

                return false;
            }

            if (Fight is FightAgression || Fight is FightPvT)
                return false;

            return true;
        }

        public void CheckArenaDailyProperties()
        {
            if (m_record.ArenaDailyDate.Day == DateTime.Now.Day || ArenaDailyMaxRank <= 0)
                return;

            var amountToken = (int)Math.Floor(ArenaDailyMaxRank/10d);
            var amountKamas = (ArenaDailyMaxRank * 10);

            m_record.ArenaDailyDate = DateTime.Now;
            ArenaDailyMaxRank = 0;
            ArenaDailyMatchsCount = 0;
            ArenaDailyMatchsWon = 0;

            Inventory.AddItem(ArenaManager.Instance.TokenItemTemplate, amountToken);
            Inventory.AddKamas(amountKamas);
            
            //SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 276, amountKamas, amountToken);
            DisplayNotification(NotificationEnum.KOLIZÉUM, amountKamas, amountToken);
        }

        public int ComputeWonArenaTokens(int rank)
        {
            var result = (int) Math.Floor(rank/100d);
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

        #endregion
        #endregion

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

            if (m_earnKamasInMerchant > 0)
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 226, m_earnKamasInMerchant, 1);
        }

        public void SendServerMessage(string message)
        {
            BasicHandler.SendTextInformationMessage(Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 0, message);
        }

        public void SendServerMessage(string message, Color color)
        {
            SendServerMessage(string.Format("<font col" +
                                            "or=\"#{0}\">{1}</font>", color.ToArgb().ToString("X"), message));
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

        #endregion

        #region Move

        public override void OnEnterMap(Map map)
        {
            ContextRoleplayHandler.SendCurrentMapMessage(Client, map.Id);

            if (map.Fights.Count > 0)
                ContextRoleplayHandler.SendMapFightCountMessage(Client, (short) map.Fights.Count);

            // send actor actions     
            foreach (var actor in map.Actors)
            {
                if (!actor.IsMoving())
                    continue;

                var moveKeys = actor.MovementPath.GetServerPathKeys();
                var actorMoving = actor;

                ContextHandler.SendGameMapMovementMessage(Client, moveKeys, actorMoving);
                BasicHandler.SendBasicNoOperationMessage(Client);
            }

            BasicHandler.SendBasicTimeMessage(Client);

            if (map.Zaap != null && !KnownZaaps.Contains(map))
                DiscoverZaap(map);

            if (MustBeJailed() && !IsInJail())
                TeleportToJail();
            else if (!MustBeJailed() && IsInJail())
                Teleport(Breed.GetStartPosition());

            if (IsRiding() && !map.Outdoor)
                Mount.Dismount(this);

            base.OnEnterMap(map);
        }

        public override bool CanMove()
        {
            return base.CanMove() && !IsDialoging();
        }

        public override bool StartMove(Path movementPath)
        {
            if (IsFighting() || MustBeJailed() || !IsInJail())
                return IsFighting() ? (Fighter.IsSlaveTurn() ? Fighter.GetSlave().StartMove(movementPath) : Fighter.StartMove(movementPath)) : base.StartMove(movementPath);

            Teleport(Breed.GetStartPosition());
            return false;
        }

        public override bool StopMove()
        {
            return IsFighting() ? Fighter.StopMove() : base.StopMove();
        }

        public override bool MoveInstant(ObjectPosition destination)
        {
            return IsFighting() ? Fighter.MoveInstant(destination) : base.MoveInstant(destination);
        }

        public override bool StopMove(ObjectPosition currentObjectPosition)
        {
            return IsFighting() ? Fighter.StopMove(currentObjectPosition) : base.StopMove(currentObjectPosition);
        }

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

        #endregion
        protected override void OnTeleported(ObjectPosition position)
        {
            base.OnTeleported(position);

            UpdateRegenedLife();

            if (Dialog != null)
                Dialog.Close();
        }

        public override bool CanChangeMap()
        {
            return base.CanChangeMap() && !IsFighting() && !Account.IsJailed;
        }

        #endregion

        #region Dialog

        public void DisplayNotification(string text, NotificationEnum notification = NotificationEnum.INFORMATION)
        {
            Client.Send(new NotificationByServerMessage((ushort) notification, new []{text}, true));
        }

        public void DisplayNotification(NotificationEnum notification, params object[] parameters)
        {
            Client.Send(new NotificationByServerMessage((ushort)notification, parameters.Select(entry => entry.ToString()), true));
        }

        public void DisplayNotification(Notification notification)
        {
            notification.Display();
        }

        public void ResetNotification()
        {
            Client.Send(new NotificationResetMessage());
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

            ((NpcDialog) Dialog).Reply(replyId);
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

        #endregion

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

            PartyJoinErrorEnum error;
            if (!party.CanInvite(target, out error, this))
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
            CompassHandler.SendCompassUpdatePartyMemberMessage(Client, Party, character);
        }

        public void UnfollowMember()
        {
            if (m_followedCharacter == null)
                return;

            m_followedCharacter.EnterMap -= OnFollowedMemberEnterMap;

            PartyHandler.SendPartyFollowStatusUpdateMessage(Client, Party, true, 0);

            m_followedCharacter = null;
        }

        private void OnFollowedMemberEnterMap(RolePlayActor actor, Map map)
        {
            if (!(actor is Character))
                return;

            CompassHandler.SendCompassUpdatePartyMemberMessage(Client, Party, (Character) actor);
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
            var dest = GetSpawnPoint() ?? Breed.GetStartPosition();

            NextMap = dest.Map;
            Cell = dest.Cell ?? dest.Map.GetRandomFreeCell();
            Direction = dest.Direction;

            // energy lost go here
            Stats.Health.DamageTaken = (short) (Stats.Health.TotalMax - 1);

            var handler = Died;
            if (handler != null) handler(this);
        }

        private void OnFightEnded(CharacterFighter fighter)
        {
            var handler = FightEnded;
            if (handler != null) handler(this, fighter);
        }

        private void OnCharacterContextChanged(bool inFight)
        {
            var handler = ContextChanged;
            if (handler != null) handler(this, inFight);
        }

        public FighterRefusedReasonEnum CanRequestFight(Character target)
        {
            if (!target.IsInWorld || target.IsFighting() || target.IsSpectator() || target.IsBusy() ||
                target.IsAway)
                return FighterRefusedReasonEnum.OPPONENT_OCCUPIED;

            if (!IsInWorld || IsFighting() || IsSpectator() || IsBusy() )
                return FighterRefusedReasonEnum.IM_OCCUPIED;

            if (target == this)
                return FighterRefusedReasonEnum.FIGHT_MYSELF;

            if (target.Map != Map || !Map.AllowFightChallenges)
                return FighterRefusedReasonEnum.WRONG_MAP;

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }

        public FighterRefusedReasonEnum CanAgress(Character target)
        {
            if (target == this)
                return FighterRefusedReasonEnum.FIGHT_MYSELF;

            if (!target.PvPEnabled || !PvPEnabled)
                return FighterRefusedReasonEnum.INSUFFICIENT_RIGHTS;

            if (!target.IsInWorld || target.IsFighting() || target.IsSpectator() || target.IsBusy())
                return FighterRefusedReasonEnum.OPPONENT_OCCUPIED;

            if (!IsInWorld || IsFighting() || IsSpectator() || IsBusy())
                return FighterRefusedReasonEnum.IM_OCCUPIED;

            if (AlignmentSide <= AlignmentSideEnum.ALIGNMENT_NEUTRAL || target.AlignmentSide <= AlignmentSideEnum.ALIGNMENT_NEUTRAL)
                return FighterRefusedReasonEnum.WRONG_ALIGNMENT;

            if (target.AlignmentSide == AlignmentSide)
                return FighterRefusedReasonEnum.WRONG_ALIGNMENT;

            if (target.Map != Map || !Map.AllowAggression)
                return FighterRefusedReasonEnum.WRONG_MAP;

            if (string.Equals(target.Client.IP, Client.IP))
                return FighterRefusedReasonEnum.MULTIACCOUNT_NOT_ALLOWED;

            if (Level - target.Level > 20)
                return FighterRefusedReasonEnum.INSUFFICIENT_RIGHTS;

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }

        public FighterRefusedReasonEnum CanAttack(TaxCollectorNpc target)
        {
            if (GuildMember != null && target.IsTaxCollectorOwner(GuildMember))
                return FighterRefusedReasonEnum.WRONG_GUILD;

            if (target.IsBusy() || IsFighting() || IsSpectator() || !IsInWorld)
                return FighterRefusedReasonEnum.OPPONENT_OCCUPIED;

            if (target.Map != Map)
                return FighterRefusedReasonEnum.WRONG_MAP;

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }        
        
        public FighterRefusedReasonEnum CanAttack(MonsterGroup group)
        {
            if (IsFighting() || IsSpectator() || !IsInWorld)
                return FighterRefusedReasonEnum.OPPONENT_OCCUPIED;

            if (group.Map != Map)
                return FighterRefusedReasonEnum.WRONG_MAP;

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }

        public CharacterFighter CreateFighter(FightTeam team)
        {
            if (IsFighting() || IsSpectator() || !IsInWorld)
                throw new Exception(string.Format("{0} is already in a fight", this));

            NextMap = Map; // we do not leave the map
            Map.Leave(this);
            StopRegen();

            ContextHandler.SendGameContextDestroyMessage(Client);
            ContextHandler.SendGameContextCreateMessage(Client, 2);

            ContextHandler.SendGameFightStartingMessage(Client, team.Fight.FightType);

            Fighter = new CharacterFighter(this, team);

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

            ContextHandler.SendGameContextDestroyMessage(Client);
            ContextHandler.SendGameContextCreateMessage(Client, 2);

            ContextHandler.SendGameFightStartingMessage(Client, fight.FightType);

            Spectator = new FightSpectator(this, fight);

            OnCharacterContextChanged(true);

            return Spectator;
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
            else if (Fighter != null && (Fighter.HasLeft() || Fight.Losers == Fighter.Team) && !Fight.IsDeathTemporarily)
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

            NextMap.Area.ExecuteInContext(() =>
            {
                LastMap = Map;
                Map = NextMap;
                Map.Enter(this);
                NextMap = null;
            });
        }

        #endregion

        #region Regen

        public bool IsRegenActive()
        {
            return RegenStartTime.HasValue;
        }

        public void StartRegen()
        {
            StartRegen((byte) (20f/Rates.RegenRate));
        }

        public void StartRegen(byte timePerHp)
        {
            if (IsRegenActive())
                StopRegen();

            RegenStartTime = DateTime.Now;
            RegenSpeed = timePerHp;

            CharacterHandler.SendLifePointsRegenBeginMessage(Client, RegenSpeed);
        }

        public void StopRegen()
        {
            if (!IsRegenActive())
                return;

            var regainedLife = (int) Math.Floor((DateTime.Now - RegenStartTime).Value.TotalSeconds/(RegenSpeed/10f));

            if (LifePoints + regainedLife > MaxLifePoints)
                regainedLife = MaxLifePoints - LifePoints;

            if (regainedLife > 0)
            {
                Stats.Health.DamageTaken -= (short) regainedLife;
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

            var regainedLife = (int) Math.Floor((DateTime.Now - RegenStartTime).Value.TotalSeconds/(RegenSpeed/10f));

            if (LifePoints + regainedLife > MaxLifePoints)
                regainedLife = MaxLifePoints - LifePoints;


            if (regainedLife > 0)
            {
                Stats.Health.DamageTaken -= (short) regainedLife;
                CharacterHandler.SendUpdateLifePointsMessage(Client);
            }

            RegenStartTime = DateTime.Now;

            OnLifeRegened(regainedLife);
        }

        #endregion

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

        #endregion

        #region Emotes

        public void PlayEmote(EmotesEnum emote)
        {
            ContextRoleplayHandler.SendEmotePlayMessage(Map.Clients, this, emote);
        }

        #endregion

        #region Friend & Ennemies

        public FriendsBook FriendsBook
        {
            get;
            private set;
        }

        #endregion

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

            if (Kamas <= MerchantBag.GetMerchantTax())
            {
                if (sendError)
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 76);
                return false;
            }

            return true;
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
                merchant.Save();

                if (merchant.Record.CharacterId != Id)
                    continue;
                if (merchant.KamasEarned > 0)
                {
                    Inventory.AddKamas((int) merchant.KamasEarned);
                    m_earnKamasInMerchant = (int) merchant.KamasEarned;
                }
                MerchantBag.LoadMerchantBag(merchant.Bag);

                MerchantManager.Instance.RemoveMerchantSpawn(merchant.Record);
            }

            // if the merchant wasn't active
            var record = MerchantManager.Instance.GetMerchantSpawn(Id);
            if (record == null)
                return;

            Inventory.AddKamas((int) record.KamasEarned);
            m_earnKamasInMerchant = (int) record.KamasEarned;
            MerchantManager.Instance.RemoveMerchantSpawn(record);
        }

        #endregion
        
        #region Bank

        public Bank Bank
        {
            get;
            private set;
        }

        public bool CheckBankIsLoaded(Action callBack)
        {
            if (Bank.IsLoaded)
                return true;

            WorldServer.Instance.IOTaskPool.AddMessage(() => { 
                Bank.LoadRecord();
                callBack();
            });

            return false;
        }

        #endregion

        #region Drop Items

        public void GetDroppedItem(WorldObjectItem objectItem)
        {
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

            if(item.Stack < quantity)
            {
                //Vous ne possédez pas l'objet en quantité suffisante.
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 252);
                return;
            }

            Inventory.RemoveItem(item, quantity);

            var objectItem = new WorldObjectItem(item.Guid, Map, Map.Cells[cell.CellId], item.Template, item.Effects, quantity);

            Map.Enter(objectItem);
        }
        
        #endregion

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

        #endregion

        #endregion

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

            Map.Area.AddMessage(() =>
            {
                Map.Enter(this);

                StartRegen();
            });

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
                    WorldServer.Instance.IOTaskPool.AddMessage(
                        () =>
                        {
                            try
                            {
                                BlockAccount();
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
            WorldServer.Instance.IOTaskPool.EnsureContext();

            if (!m_recordLoaded)
                return;

            lock (SaveSync)
            {
                using (var transaction = WorldServer.Instance.DBAccessor.Database.GetTransaction())
                {
                    // do something better here
                    Inventory.Save();
                    if (Bank.IsLoaded)
                        Bank.Save();
                    MerchantBag.Save();
                    Spells.Save();
                    Shortcuts.Save();
                    FriendsBook.Save();

                    if (GuildMember != null && GuildMember.IsDirty)
                        GuildMember.Save(WorldServer.Instance.DBAccessor.Database);

                    if (Mount != null)
                        Mount.Save(WorldServer.Instance.DBAccessor.Database);

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
                    m_record.BaseHealth = Stats.Health.Base;
                    m_record.DamageTaken = Stats.Health.DamageTaken;

                    WorldServer.Instance.DBAccessor.Database.Update(m_record);
                    WorldServer.Instance.DBAccessor.Database.Update(Client.WorldAccount);

                    transaction.Complete();
                }
            }

            if (IsAuthSynced)
                OnSaved();
        }

        private void LoadRecord()
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

            AlignmentGrade = (sbyte) ExperienceManager.Instance.GetAlignementGrade(m_record.Honor);
            LowerBoundHonor = ExperienceManager.Instance.GetAlignementGradeHonor((byte) AlignmentGrade);
            UpperBoundHonor = ExperienceManager.Instance.GetAlignementNextGradeHonor((byte) AlignmentGrade);

            Inventory = new Inventory(this);
            Inventory.LoadInventory();
            Inventory.LoadPresets();

            UpdateLook(false);

            Bank = new Bank(this); // lazy loading here !

            MerchantBag = new CharacterMerchantBag(this);
            CheckMerchantModeReconnection();
            MerchantBag.LoadMerchantBag();

            GuildMember = GuildManager.Instance.TryGetGuildMember(Id);

            Mount = MountManager.Instance.TryGetMountByCharacterId(Id) != null ? new Mount(this) : null;

            Spells = new SpellInventory(this);
            Spells.LoadSpells();

            Shortcuts = new ShortcutBar(this);
            Shortcuts.Load();

            FriendsBook = new FriendsBook(this);
            FriendsBook.Load();

            ChatHistory = new ChatHistory(this);

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

        #endregion

        #region Exceptions

        private readonly List<KeyValuePair<string, Exception>> m_commandsError =
            new List<KeyValuePair<string, Exception>>();

        public List<KeyValuePair<string, Exception>> CommandsErrors
        {
            get { return m_commandsError; }
        }

        private readonly List<Exception> m_errors = new List<Exception>();

        public List<Exception> Errors
        {
            get { return m_errors; }
        }

        #endregion

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

        #endregion

        #region ActorAlignmentInformations

        public ActorAlignmentInformations GetActorAlignmentInformations()
        {
            return new ActorAlignmentInformations(
                (sbyte) (PvPEnabled ? AlignmentSide : 0),
                (sbyte) (PvPEnabled ? AlignmentValue : 0),
                (sbyte) (PvPEnabled ? AlignmentGrade : 0),
                Dishonor,
                CharacterPower);
        }

        #endregion

        #region ActorExtendedAlignmentInformations

        public ActorExtendedAlignmentInformations GetActorAlignmentExtendInformations()
        {
            return new ActorExtendedAlignmentInformations(
                (sbyte) AlignmentSide,
                AlignmentValue,
                AlignmentGrade,
                Dishonor,
                CharacterPower,
                Honor,
                LowerBoundHonor,
                UpperBoundHonor,
                PvPEnabled
                );
        }

        #endregion

        #region CharacterBaseInformations

        public CharacterBaseInformations GetCharacterBaseInformations()
        {
            return new CharacterBaseInformations(
                Id,
                Level,
                Name,
                Look.GetEntityLook(),
                (sbyte) BreedId,
                Sex == SexTypeEnum.SEX_FEMALE);
        }

        public CharacterMinimalPlusLookInformations GetCharacterMinimalPlusLookInformations()
        {
            return new CharacterMinimalPlusLookInformations(
                Id,
                Level,
                Name,
                Look.GetEntityLook());
        }

        #endregion

        #region PartyMemberInformations

        public PartyInvitationMemberInformations GetPartyInvitationMemberInformations()
        {
            return new PartyInvitationMemberInformations(
                Id,
                Level,
                Name,
                Look.GetEntityLook(),
                (sbyte) BreedId,
                Sex == SexTypeEnum.SEX_FEMALE,
                (short) Map.Position.X,
                (short) Map.Position.Y,
                Map.Id,
                (short) Map.SubArea.Id);
        }

        public PartyMemberInformations GetPartyMemberInformations()
        {
            return new PartyMemberInformations(
                Id,
                Level,
                Name,
                Look.GetEntityLook(),
                (sbyte) BreedId,
                Sex == SexTypeEnum.SEX_FEMALE,
                LifePoints,
                MaxLifePoints,
                (short) Stats[PlayerFields.Prospecting].Total,
                RegenSpeed,
                (short) Stats[PlayerFields.Initiative].Total,
                PvPEnabled,
                (sbyte) AlignmentSide,
                (short) Map.Position.X,
                (short) Map.Position.Y,
                Map.Id,
                (short) SubArea.Id);
        }

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
                (sbyte) BreedId,
                Sex == SexTypeEnum.SEX_FEMALE);
        }

        public PartyMemberArenaInformations GetPartyMemberArenaInformations()
        {
            return new PartyMemberArenaInformations(
                Id,
                Level,
                Name,
                Look.GetEntityLook(),
                (sbyte) BreedId,
                Sex == SexTypeEnum.SEX_FEMALE,
                LifePoints,
                MaxLifePoints,
                (short) Stats[PlayerFields.Prospecting].Total,
                RegenSpeed,
                (short) Stats[PlayerFields.Initiative].Total,
                PvPEnabled,
                (sbyte) AlignmentSide,
                (short) Map.Position.X,
                (short) Map.Position.Y,
                Map.Id,
                (short) SubArea.Id,
                (short)ArenaRank);
        }

        #endregion

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

            human.options = options;
            return human;
        }

        #endregion

        internal CharacterRecord Record
        {
            get { return m_record; }
        }

        public override bool CanBeSee(WorldObject byObj)
        {
            return base.CanBeSee(byObj) && (byObj == this || !Invisible);
        }

        protected override void OnDisposed()
        {            
            if (FriendsBook != null)
                FriendsBook.Dispose();

            if (Inventory != null)
                Inventory.Dispose();

            base.OnDisposed();
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Id);
        }
    }
}