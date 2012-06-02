using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using Castle.ActiveRecord;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Breeds;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Breeds;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Game.Dialogs.Interactives;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Game.Exchanges;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Items;
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
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Handlers.Moderation;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Characters
{
    public sealed class Character : Humanoid,
                                    IStatsOwner, IInventoryOwner, ICommandsUser
    {
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
            Action<Character> handler = LoggedIn;
            if (handler != null) handler(this);
        }

        public event Action<Character> LoggedOut;

        private void OnLoggedOut()
        {
            Action<Character> handler = LoggedOut;
            if (handler != null) handler(this);
        }

        public event Action<Character, int> LifeRegened;

        private void OnLifeRegened(int regenedLife)
        {
            Action<Character, int> handler = LifeRegened;
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

        public NpcShopDialog NpcShopDialog
        {
            get { return Dialog as NpcShopDialog; }
        }

        public ZaapDialog ZaapDialog
        {
            get { return Dialog as ZaapDialog; }
        }

        public IRequestBox RequestBox
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
            Dialog = dialog;
        }

        public void ResetDialog()
        {
            Dialoger = null;
        }

        public void OpenRequestBox(IRequestBox request)
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

        public PlayerTrader Trader
        {
            get { return Dialoger as PlayerTrader; }
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

        #region Apparence

        public bool CustomLookActivated
        {
            get { return m_record.CustomLookActivated; }
            set { m_record.CustomLookActivated = value; }
        }

        public EntityLook CustomLook
        {
            get { return m_record.CustomEntityLook; }
            set { m_record.CustomEntityLook = value; }
        }

        public EntityLook RealLook
        {
            get { return m_record.EntityLook; }
            private set
            {
                m_record.EntityLook = value;
                base.Look = value;
            }
        }

        public override EntityLook Look
        {
            get { return (CustomLookActivated && CustomLook != null ? CustomLook : RealLook); }
        }

        public SexTypeEnum Sex
        {
            get { return m_record.Sex; }
            private set { m_record.Sex = value; }
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

        public void UpdateLook(bool send = true)
        {
            var skins = new List<short>(Breed.GetLook(Sex).skins);
            skins.AddRange(Inventory.GetItemsSkins());

            RealLook.skins = skins;

            var pets = Inventory.GetPetsSkins();
            var subentities = pets.Select((t, i) => new SubEntity(1, (sbyte) i, new EntityLook(t, new short[0], new int[0], new short[] {75}, new SubEntity[0]))).ToList();

            RealLook.subentities = subentities;

            if (send)
                Map.Refresh(this);
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
                if (value >= UpperBoundExperience && Level < ExperienceManager.Instance.HighestCharacterLevel ||
                    value < LowerBoundExperience)
                {
                    byte lastLevel = Level;

                    Level = ExperienceManager.Instance.GetCharacterLevel(m_record.Experience);

                    LowerBoundExperience = ExperienceManager.Instance.GetCharacterLevelExperience(Level);
                    UpperBoundExperience = ExperienceManager.Instance.GetCharacterNextLevelExperience(Level);

                    int difference = Level - lastLevel;

                    OnLevelChanged(Level, difference);
                }
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
                level = (byte)( Level - levelRemoved );

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

            Stats.Health.Base += (short)( difference * 5 );
            Stats.Health.DamageTaken = 0;

            if (currentLevel >= 100 && currentLevel - difference < 100)
            {
                Stats.AP.Base++;
            }
            else if (currentLevel < 100 && currentLevel - difference >= 100)
            {
                Stats.AP.Base--;
            }

            foreach (LearnableSpell spell in Breed.LearnableSpells)
            {
                if (spell.ObtainLevel > currentLevel && Spells.HasSpell(spell.SpellId))
                    Spells.UnLearnSpell(spell.SpellId);
                else if (spell.ObtainLevel <= currentLevel && !Spells.HasSpell(spell.SpellId))
                {
                    Spells.LearnSpell(spell.SpellId);
                    Shortcuts.AddSpellShortcut(Shortcuts.GetNextFreeSlot(), (short) spell.SpellId);
                }
            }

            RefreshStats();
            CharacterHandler.SendCharacterLevelUpMessage(Client, currentLevel);
            CharacterHandler.SendCharacterLevelUpInformationMessage(Map.Clients, this, currentLevel);

            LevelChangedHandler handler = LevelChanged;

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
        #endregion

        #region Alignment

        public AlignmentSideEnum AlignmentSide
        { 
            get { return m_record.AlignmentSide; }
            private set { m_record.AlignmentSide = value; }
        }

        private sbyte m_alignmentGrade;

        public sbyte AlignmentGrade
        {
            get { return m_alignmentGrade; }
            private set { m_alignmentGrade = value; }
        }

        public sbyte AlignmentValue
        {
            get { return m_record.AlignmentValue; }
            private set { m_record.AlignmentValue = value; }
        }

        public ushort Honor
        {
            get { return m_record.Honor; }
            private set
            {
                m_record.Honor = value; 
                if (value >= UpperBoundHonor && AlignmentGrade < ExperienceManager.Instance.HighestGrade)
                {
                    sbyte lastGrade = AlignmentGrade;

                    AlignmentGrade = (sbyte) ExperienceManager.Instance.GetAlignementGrade(m_record.Honor);

                    LowerBoundHonor = ExperienceManager.Instance.GetAlignementGradeHonor((byte) AlignmentGrade);
                    UpperBoundHonor = ExperienceManager.Instance.GetAlignementNextGradeHonor((byte) AlignmentGrade);

                    int difference = AlignmentGrade - lastGrade;

                    OnGradeChanged(AlignmentGrade, difference);
                }
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
            if (amount < 0)
                SubHonor((ushort) (-amount));
            else
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

        public Fights.Fight Fight
        {
            get
            {
                return Fighter == null ? (Spectator != null ? Spectator.Fight : null ) : Fighter.Fight;
            }
        }

        public FightTeam Team
        {
            get
            {
                return Fighter != null ? Fighter.Team : null;
            }
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

        #endregion

        #region Actions

        #region Chat
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

        #endregion

        #region Move

        public override bool StartMove(Path movementPath)
        {
            if (IsFighting())
                return Fighter.StartMove(movementPath);

            return base.StartMove(movementPath);
        }

        public override bool StopMove()
        {
            if (IsFighting())
                return Fighter.StopMove();

            return base.StopMove();
        }

        public override bool MoveInstant(ObjectPosition destination)
        {
            if (IsFighting())
                return Fighter.MoveInstant(destination);

            return base.MoveInstant(destination);
        }

        public override bool StopMove(ObjectPosition currentObjectPosition)
        {
            if (IsFighting())
                return Fighter.StopMove(currentObjectPosition);

            return base.StopMove(currentObjectPosition);
        }

        public override bool Teleport(MapNeighbour mapNeighbour)
        {
            bool success = base.Teleport(mapNeighbour);

            if (!success)
                SendServerMessage("Unknown map transition");

            return success;
        }

        public override bool Teleport(ObjectPosition destination)
        {
            LeaveDialog();

            return base.Teleport(destination);
        }

        protected override void OnTeleported(ObjectPosition position)
        {
            base.OnTeleported(position); 

            UpdateRegenedLife();
        }

        public override bool CanChangeMap()
        {
            return base.CanChangeMap() && !IsDialoging() && !IsFighting();
        }

        #endregion

        #region Dialog

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

            ( (NpcDialog)Dialog ).Reply(replyId);
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
            var dest = GetSpawnPoint();

            // use nextmap to update correctly the areas changements
            NextMap = dest.Map;
            Cell = dest.Cell;
            Direction = dest.Direction;

            // energy lost go here
            Stats.Health.DamageTaken = (short) (Stats.Health.TotalMax - 1);

            CharacterDiedHandler handler = Died;
            if (handler != null) handler(this);
        }

        private void OnFightEnded(CharacterFighter fighter)
        {
            CharacterFightEndedHandler handler = FightEnded;
            if (handler != null) handler(this, fighter);
        }

        private void OnCharacterContextChanged(bool inFight)
        {
            CharacterContextChangedHandler handler = ContextChanged;
            if (handler != null) handler(this, inFight);
        }

        public FighterRefusedReasonEnum CanRequestFight(Character target)
        {
            if (!target.IsInWorld || target.IsFighting() || target.IsSpectator() || target.IsBusy())
                return FighterRefusedReasonEnum.OPPONENT_OCCUPIED;

            if (!IsInWorld || IsFighting() || IsSpectator() || IsBusy())
                return FighterRefusedReasonEnum.IM_OCCUPIED;

            if (target == this)
                return FighterRefusedReasonEnum.FIGHT_MYSELF;

            if (target.Map != Map)
                return FighterRefusedReasonEnum.WRONG_MAP;

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }


        public FighterRefusedReasonEnum CanAgress(Character target)
        {
            if (target == this)
                return FighterRefusedReasonEnum.FIGHT_MYSELF;

            if (!PvPEnabled)
                return FighterRefusedReasonEnum.WRONG_ALIGNMENT;

            if (!target.IsInWorld || target.IsFighting() || target.IsSpectator() || target.IsBusy())
                return FighterRefusedReasonEnum.OPPONENT_OCCUPIED;

            if (!IsInWorld || IsFighting() || IsSpectator() || IsBusy())
                return FighterRefusedReasonEnum.IM_OCCUPIED;

            if (AlignmentSide <= AlignmentSideEnum.ALIGNMENT_NEUTRAL)
                return FighterRefusedReasonEnum.WRONG_ALIGNMENT;

            if (target.AlignmentSide == AlignmentSide)
                return FighterRefusedReasonEnum.WRONG_ALIGNMENT;

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
            else if (Fighter != null && (Fighter.HasLeft() || Fight.Losers == Fighter.Team))
                OnDied();

            Fighter = null;
            Spectator = null;

            ContextHandler.SendGameContextDestroyMessage(Client);
            ContextHandler.SendGameContextCreateMessage(Client, 1);

            RefreshStats();

            LastMap = Map;
            Map = NextMap;
            NextMap.Enter(this);
            LastMap = null;
            NextMap = null;

            OnCharacterContextChanged(false);
            StartRegen();
        }

        #endregion

        #region Regen

        public bool IsRegenActive()
        {
            return RegenStartTime.HasValue;
        }

        public void StartRegen()
        {
            StartRegen((byte)( 20f / Rates.RegenRate ));
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

            var regainedLife = (int) Math.Floor((DateTime.Now - RegenStartTime).Value.TotalSeconds / (RegenSpeed / 10f));

            if (LifePoints + regainedLife > MaxLifePoints)
                regainedLife = MaxLifePoints - LifePoints;

            if (regainedLife > 0)
            {
                Stats.Health.DamageTaken -= (short) regainedLife;
                CharacterHandler.SendLifePointsRegenEndMessage(Client, regainedLife);
            }

            RegenStartTime = null;
            RegenSpeed = 0;
            OnLifeRegened(regainedLife);
        }

        public void UpdateRegenedLife()
        {
            if (!IsRegenActive())
                return;

            var regainedLife = (int)Math.Floor(( DateTime.Now - RegenStartTime ).Value.TotalSeconds / (RegenSpeed / 10f));

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

            BasicHandler.SendTextInformationMessage(Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 24); // new zaap
        }

        public void SetSpawnPoint(Map map)
        {
            Record.SpawnMap = map;
            m_spawnPoint = null;

            BasicHandler.SendTextInformationMessage(Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 6); // pos saved
        }

        public ObjectPosition GetSpawnPoint()
        {
            if (Record.SpawnMap != null)
            {
                if (m_spawnPoint != null)
                    return m_spawnPoint;

                var map = Record.SpawnMap;

                if (map.Zaap != null)
                {
                    var cell = map.GetRandomAdjacentFreeCell(map.Zaap.Position.Point);
                    var direction = map.Zaap.Position.Point.OrientationTo(new MapPoint(cell));

                    return new ObjectPosition(map, cell, direction);
                }
                else
                {
                    return new ObjectPosition(map, map.GetRandomFreeCell(), Direction);
                }
            }
            else
            {
                return Breed.GetStartPosition();
            }
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

            Map.Enter(this);
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

                    if (IsInWorld)
                    {
                        DenyAllInvitations();

                        if (IsInRequest())
                            CancelRequest();

                        if (IsDialoging())
                            Dialog.Close();

                        if (IsInParty())
                            LeaveParty();

                        if (Map != null && !IsFighting())
                            Map.Leave(this);

                        World.Instance.Leave(this);

                        m_inWorld = false;
                    }
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
            if (!m_recordLoaded)
                return;

            lock (SaveSync)
            {
                using (var session = new SessionScope(FlushAction.Never))
                {
                    Inventory.Save();
                    Spells.Save();
                    Shortcuts.Save();
                    FriendsBook.Save();

                    m_record.MapId = Map.Id;
                    m_record.CellId = Cell.Id;
                    m_record.Direction = Direction;

                    m_record.AP = (ushort) Stats[PlayerFields.AP].Base;
                    m_record.MP = (ushort) Stats[PlayerFields.MP].Base;
                    m_record.Strength = Stats[PlayerFields.Strength].Base;
                    m_record.Agility = Stats[PlayerFields.Agility].Base;
                    m_record.Chance = Stats[PlayerFields.Chance].Base;
                    m_record.Intelligence = Stats[PlayerFields.Intelligence].Base;
                    m_record.Wisdom = Stats[PlayerFields.Wisdom].Base;
                    m_record.Vitality = Stats[PlayerFields.Vitality].Base;
                    m_record.BaseHealth = (ushort) Stats.Health.Base;
                    m_record.DamageTaken = (ushort)Stats.Health.DamageTaken;

                    m_record.Update();

                    session.Flush();
                }
            }
        }

        private void LoadRecord()
        {
            Breed = BreedManager.Instance.GetBreed(BreedId);
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

            AlignmentGrade = (sbyte)ExperienceManager.Instance.GetAlignementGrade(m_record.Honor);
            LowerBoundHonor = (ushort) ExperienceManager.Instance.GetAlignementGradeHonor((byte) AlignmentGrade);
            UpperBoundHonor = (ushort) ExperienceManager.Instance.GetAlignementNextGradeHonor((byte) AlignmentGrade);

            Inventory = new Inventory(this);
            Inventory.LoadInventory();
            Spells = new SpellInventory(this);
            Spells.LoadSpells();
            Shortcuts = new ShortcutBar(this);
            Shortcuts.Load();
            FriendsBook = new FriendsBook(this);
            FriendsBook.Load();

            m_recordLoaded = true;
        }

        private void UnLoadRecord()
        {
            if (!m_recordLoaded)
                return;

            m_recordLoaded = false;
            Dispose();
        }

        #endregion

        #region Exceptions

        private List<KeyValuePair<string, Exception>> m_commandsError = new List<KeyValuePair<string, Exception>>();
        public List<KeyValuePair<string, Exception>> CommandsErrors
        {
            get
            {
                return m_commandsError;
            }
        }

        private List<Exception> m_errors = new List<Exception>(); 
        public List<Exception> Errors
        {
            get { return m_errors; }
        }

        #endregion

        #region Network

        #region GameRolePlayCharacterInformations

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameRolePlayCharacterInformations(
                Id,
                Look,
                GetEntityDispositionInformations(),
                Name,
                GetHumanInformations(),
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
                Look,
                (sbyte) BreedId,
                Sex == SexTypeEnum.SEX_FEMALE);
        }

        #endregion

        #region PartyMemberInformations

        public PartyInvitationMemberInformations GetPartyInvitationMemberInformations()
        {
            return new PartyInvitationMemberInformations(
                Id,
                Level,
                Name,
                Look,
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
                Look,
                LifePoints,
                MaxLifePoints,
                (short) Stats[PlayerFields.Prospecting].Total,
                0,
                (short) Stats[PlayerFields.Initiative].Total,
                false,
                0);
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
                Look);
        }

        #endregion


        #endregion

        internal CharacterRecord Record
        {
            get { return m_record; }
        }

        public override void Dispose()
        {
            if (FriendsBook != null)
                FriendsBook.Dispose();

            if (Inventory != null)
                Inventory.Dispose();

            base.Dispose();
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Id);
        }
    }
}