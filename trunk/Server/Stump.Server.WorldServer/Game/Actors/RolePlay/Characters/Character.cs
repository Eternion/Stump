using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using NLog;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Database.Breeds;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Breeds;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Game.Dialogs.Interactives;
using Stump.Server.WorldServer.Game.Dialogs.Merchants;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.TaxCollector;
using Stump.Server.WorldServer.Game.Exchanges;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Game.Items.Player;
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
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Handlers.Guilds;
using Stump.Server.WorldServer.Handlers.Moderation;
using Stump.Server.WorldServer.Handlers.Titles;
using GuildMember = Stump.Server.WorldServer.Game.Guilds.GuildMember;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Characters
{
    public sealed class Character : Humanoid, IStatsOwner, IInventoryOwner, ICommandsUser
    {
        private const int AURA_1_SKIN = 170;
        private const int AURA_2_SKIN = 171;


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

            var handler = LoggedOut;
            if (handler != null) handler(this);
        }

        public event Action<Character> Saved;

        private void OnSaved()
        {
            var handler = Saved;
            if (handler != null) handler(this);
        }

        public event Action<Character, int> LifeRegened;

        private void OnLifeRegened(int regenedLife)
        {
            var handler = LifeRegened;
            if (handler != null) handler(this, regenedLife);
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

        public bool IsInMerchantDialog()
        {
            return Dialog is MerchantShopDialog;
        }

        #endregion

        #region Party

        private readonly Dictionary<int, PartyInvitation> m_partyInvitations
            = new Dictionary<int, PartyInvitation>();


        public Party Party
        {
            get;
            private set;
        }

        public bool IsInParty()
        {
            return Party != null;
        }

        public bool IsPartyLeader()
        {
            return IsInParty() && Party.Leader == this;
        }

        #endregion

        #region Trade

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
            get { return (CustomLookActivated && CustomLook != null ? CustomLook : RealLook); }
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

            if (petSkin.HasValue)
                RealLook.SetPetSkin(petSkin.Value);
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
            get { return m_record.Experience; }
            private set
            {
                m_record.Experience = value;
                if ((value < UpperBoundExperience || Level >= ExperienceManager.Instance.HighestCharacterLevel) &&
                    value >= LowerBoundExperience) return;
                var lastLevel = Level;

                Level = ExperienceManager.Instance.GetCharacterLevel(m_record.Experience);

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


            foreach (var spell in Breed.Spells)
            {
                if (spell.ObtainLevel > currentLevel && Spells.HasSpell(spell.Spell))
                    Spells.UnLearnSpell(spell.Spell);
                else if (spell.ObtainLevel <= currentLevel && !Spells.HasSpell(spell.Spell))
                {
                    Spells.LearnSpell(spell.Spell);
                    Shortcuts.AddSpellShortcut(Shortcuts.GetNextFreeSlot(ShortcutBarEnum.SPELL_SHORTCUT_BAR),
                        (short) spell.Spell);
                }
            }

            RefreshStats();
            CharacterHandler.SendCharacterLevelUpMessage(Client, currentLevel);
            CharacterHandler.SendCharacterLevelUpInformationMessage(Map.Clients, this, currentLevel);

            var handler = LevelChanged;

            if (handler != null)
                handler(this, currentLevel, difference);
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
                m_record.Honor = value > 17500 ? (ushort) 17500 : value;
                if (value < UpperBoundHonor || AlignmentGrade >= ExperienceManager.Instance.HighestGrade)
                    return;

                var lastGrade = AlignmentGrade;

                AlignmentGrade = (sbyte) ExperienceManager.Instance.GetAlignementGrade(m_record.Honor);

                LowerBoundHonor = ExperienceManager.Instance.GetAlignementGradeHonor((byte) AlignmentGrade);
                UpperBoundHonor = ExperienceManager.Instance.GetAlignementNextGradeHonor((byte) AlignmentGrade);

                var difference = AlignmentGrade - lastGrade;

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
            Honor += amount;
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
            Map.Refresh(this);
            RefreshStats();

            var handler = PvPToggled;

            if (handler != null)
                handler(this, PvPEnabled);
        }

        public event Action<Character, AlignmentSideEnum> AligmenentSideChanged;

        private void OnAligmenentSideChanged()
        {
            Map.Refresh(this);
            RefreshStats();
            TogglePvPMode(false);

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

        public Fights.Fight Fight
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

                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 193,
                    date.Year,
                    date.Month,
                    date.Day,
                    date.Hour,
                    date.Minute.ToString("00"));
            }

            if (m_earnKamasInMerchant > 0)
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 45, m_earnKamasInMerchant);
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

            base.OnEnterMap(map);
        }

        public override bool CanMove()
        {
            return base.CanMove() && !IsDialoging();
        }

        public override bool StartMove(Path movementPath)
        {
            if (!IsFighting() && !MustBeJailed() && IsInJail())
            {
                Teleport(Breed.GetStartPosition());
                return false;
            }

            return IsFighting() ? Fighter.StartMove(movementPath) : base.StartMove(movementPath);
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

        private readonly int[] JAILS_MAPS = {105121026, 105119744, 105120002};
        private readonly int[][] JAILS_CELLS = {new[] {179, 445, 184, 435}, new[] {314}, new[] {300}};

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

        public void Invite(Character target)
        {
            if (!IsInParty())
            {
                Party party = PartyManager.Instance.Create(this);

                EnterParty(party);
            }

            if (!Party.CanInvite(target))
                return;

            if (target.m_partyInvitations.ContainsKey(Party.Id))
                return; // already invited

            var invitation = new PartyInvitation(Party, this, target);
            target.m_partyInvitations.Add(Party.Id, invitation);

            Party.AddGuest(target);
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

        public void EnterParty(Party party)
        {
            if (IsInParty())
                LeaveParty();

            if (m_partyInvitations.ContainsKey(party.Id))
                m_partyInvitations.Remove(party.Id);

            DenyAllInvitations();
            UpdateRegenedLife();

            Party = party;
            Party.MemberRemoved += OnPartyMemberRemoved;
            Party.PartyDeleted += OnPartyDeleted;

            if (party.IsMember(this))
                return;

            if (!party.PromoteGuestToMember(this))
            {
                Party.MemberRemoved -= OnPartyMemberRemoved;
                Party.PartyDeleted -= OnPartyDeleted;
                Party = null;
            }
        }

        public void LeaveParty()
        {
            if (!IsInParty())
                return;


            Party.MemberRemoved -= OnPartyMemberRemoved;
            Party.PartyDeleted -= OnPartyDeleted;
            Party.RemoveMember(this);
            Party = null;
        }

        private void OnPartyMemberRemoved(Party party, Character member, bool kicked)
        {
            if (member != this)
                return;

            Party.MemberRemoved -= OnPartyMemberRemoved;
            Party.PartyDeleted -= OnPartyDeleted;
            Party = null;
        }

        private void OnPartyDeleted(Party party)
        {
            Party.MemberRemoved -= OnPartyMemberRemoved;
            Party.PartyDeleted -= OnPartyDeleted;
            Party = null;
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

            // use nextmap to update correctly the areas changements
            NextMap = dest.Map;
            Cell = dest.Cell;
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

            if (!IsInWorld || IsFighting() || IsSpectator() || IsBusy())
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

            if (target.Client.IP == Client.IP)
                return FighterRefusedReasonEnum.MULTIACCOUNT_NOT_ALLOWED;

            if (Level - target.Level > 20)
                return FighterRefusedReasonEnum.INSUFFICIENT_RIGHTS;

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }

        public FighterRefusedReasonEnum CanAttack(TaxCollectorNpc target)
        {
            if (GuildMember != null && target.IsTaxCollectorOwner(GuildMember))
                return FighterRefusedReasonEnum.WRONG_GUILD;

            if (target.IsBusy())
                return FighterRefusedReasonEnum.OPPONENT_OCCUPIED;

            if (target.Map != Map)
                return FighterRefusedReasonEnum.WRONG_MAP;

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }

        public CharacterFighter CreateFighter(FightTeam team)
        {
            if (IsFighting() || IsSpectator() || !IsInWorld)
                return null;

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

        public FightSpectator CreateSpectator(Fights.Fight fight)
        {
            if (IsFighting() || IsSpectator() || !IsInWorld)
                return null;

            if (!fight.CanSpectatorJoin(this))
                return null;

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
            else if (Fighter != null && (Fighter.HasLeft() || Fight.Losers == Fighter.Team) && !(Fight is FightDuel))
                OnDied();

            Fighter = null;
            Spectator = null;

            ContextHandler.SendGameContextDestroyMessage(Client);
            ContextHandler.SendGameContextCreateMessage(Client, 1);

            RefreshStats();
            
            OnCharacterContextChanged(false);
            StartRegen();

            NextMap.Area.ExecuteInContext(() =>
            {
                LastMap = Map;
                Map = NextMap;
                NextMap.Enter(this);
                LastMap = null;
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
            var auraSkin = GetAuraSkin(emote);

            if (auraSkin != -1)
            {
                if (RealLook.AuraLook != null && RealLook.AuraLook.BonesID == auraSkin)
                    RealLook.RemoveAuras();
                else
                    RealLook.SetAuraSkin(auraSkin);
                RefreshActor();
            }

            ContextRoleplayHandler.SendEmotePlayMessage(Map.Clients, this, emote);
        }

        public short GetAuraSkin(EmotesEnum auraEmote)
        {
            switch (auraEmote)
            {
                case EmotesEnum.EMOTE_AURA_VAMPYRIQUE:
                    return AURA_1_SKIN;
                case EmotesEnum.EMOTE_AURA_DE_PUISSANCE:
                    return AURA_2_SKIN;
                default:
                    return -1;
            }
        }

        public void ToggleAura(EmotesEnum emote, bool toggle)
        {
            var auraSkin = GetAuraSkin(emote);

            if (auraSkin == -1)
                return;

            var hasAura = (RealLook.AuraLook == null || RealLook.AuraLook.BonesID != GetAuraSkin(emote));

            if (!hasAura && toggle)
                PlayEmote(emote);

            else if (hasAura && !toggle)
                PlayEmote(emote);
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

        #endregion

        #region Save & Load

        public bool IsLoggedIn
        {
            get;
            private set;
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

                    if (IsInParty())
                        LeaveParty();

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
                    MerchantBag.Save();
                    Spells.Save();
                    Shortcuts.Save();
                    FriendsBook.Save();

                    if (GuildMember != null && GuildMember.IsDirty)
                        GuildMember.Save(WorldServer.Instance.DBAccessor.Database);

                    m_record.MapId = Map.Id;
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

                    transaction.Complete();
                }
            }

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
            Level = ExperienceManager.Instance.GetCharacterLevel(m_record.Experience);
            LowerBoundExperience = ExperienceManager.Instance.GetCharacterLevelExperience(Level);
            UpperBoundExperience = ExperienceManager.Instance.GetCharacterNextLevelExperience(Level);

            AlignmentGrade = (sbyte) ExperienceManager.Instance.GetAlignementGrade(m_record.Honor);
            LowerBoundHonor = ExperienceManager.Instance.GetAlignementGradeHonor((byte) AlignmentGrade);
            UpperBoundHonor = ExperienceManager.Instance.GetAlignementNextGradeHonor((byte) AlignmentGrade);

            Inventory = new Inventory(this);
            Inventory.LoadInventory();
            UpdateLook(false);

            MerchantBag = new CharacterMerchantBag(this);
            CheckMerchantModeReconnection();
            MerchantBag.LoadMerchantBag();

            GuildMember = GuildManager.Instance.TryGetGuildMember(Id);

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
            if (Area != null)
                Area.ExecuteInContext(() => Dispose());
            else
                Dispose();
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

            PartyInvitation invitation = m_partyInvitations[party.Id];

            return new PartyGuestInformations(
                Id,
                invitation.Source.Id,
                Name,
                Look.GetEntityLook(),
                (sbyte) BreedId,
                Sex == SexTypeEnum.SEX_FEMALE);
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