using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Runtime.CompilerServices;
using Castle.ActiveRecord;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Breeds;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Breeds;
using Stump.Server.WorldServer.Game.Dialogs;
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
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.Server.WorldServer.Handlers.Chat;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Characters
{
    public sealed class Character : Humanoid,
                                    IStatsOwner, IInventoryOwner
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly CharacterRecord m_record;
        private bool m_recordLoaded;

        public Character(CharacterRecord record, WorldClient client)
        {
            m_record = record;
            Client = client;
            SaveSync = new object();

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

        public object SaveSync
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
            get;
            set;
        }

        public EntityLook CustomLook
        {
            get;
            set;
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

        #endregion

        #region Stats

        #region Delegates

        public delegate void LevelChangedHandler(Character character, byte currentLevel, int difference);

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
                if (value >= UpperBoundExperience && Level < ExperienceManager.Instance.HighestLevel)
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
            var level = (byte) (Level + levelAdded);

            if (level > ExperienceManager.Instance.HighestLevel)
                level = ExperienceManager.Instance.HighestLevel;

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

        public event LevelChangedHandler LevelChanged;

        private void OnLevelChanged(byte currentLevel, int difference)
        {
            SpellsPoints += (ushort)difference;
            StatsPoints += (ushort)(difference * 5);
            Stats.Health.Base += (short)( difference * 5 );
            Stats.Health.DamageTaken = 0;

            for (int i = 0; i < Breed.LearnableSpells.Count; i++)
            {
                if (Breed.LearnableSpells[i].ObtainLevel > currentLevel)
                    continue;

                if (!Spells.HasSpell(Breed.LearnableSpells[i].SpellId))
                {
                    Spells.LearnSpell(Breed.LearnableSpells[i].SpellId);
                }
            }

            CharacterHandler.SendCharacterStatsListMessage(Client);
            CharacterHandler.SendCharacterLevelUpMessage(Client, currentLevel);
            Map.ForEach(entry => CharacterHandler.SendCharacterLevelUpInformationMessage(entry.Client, this, currentLevel));

            LevelChangedHandler handler = LevelChanged;

            if (handler != null)
                handler(this, currentLevel, difference);
        }

        #endregion

        #region Alignment

        public AlignmentSideEnum AlignmentSide
        {
            get;
            private set;
        }

        public sbyte AlignmentValue
        {
            get;
            private set;
        }

        public sbyte AlignmentGrade
        {
            get;
            private set;
        }

        public ushort Dishonor
        {
            get;
            private set;
        }

        public int CharacterPower
        {
            get { return Id + Level; }
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
            ChatHandler.SendChatServerMessage(Client, message);
        }

        public void SendServerMessage(string message, Color color)
        {
            SendServerMessage(string.Format("<font color=\"#{0}\">{1}</font>", color.ToArgb().ToString("X"), message));
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

        protected override void OnTeleported(ObjectPosition position)
        {
            base.OnTeleported(position); 

            UpdateRegenedLife();
        }

        public override bool CanChangeMap()
        {
            return base.CanChangeMap() && !IsFighting();
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

            else if (IsDialoging())
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

            Contract.Assume(RequestBox != null);

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
            foreach (var partyInvitation in m_partyInvitations)
            {
                Contract.Assume(partyInvitation.Value != null);
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

            Contract.Assume(Party != null);

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

        public FighterRefusedReasonEnum CanRequestFight(Character target)
        {
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

            return Fighter = new CharacterFighter(this, team);
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

            return Spectator = new FightSpectator(this, fight);
        }

        /// <summary>
        /// Rejoin the map after a fight
        /// </summary>
        public void RejoinMap()
        {
            if (!IsFighting() && !IsSpectator())
                return;

            if (Fighter != null)
            {
                Fighter.OnRejoinMap();
            }
            
            Fighter = null;
            Spectator = null;

            ContextHandler.SendGameContextDestroyMessage(Client);
            ContextHandler.SendGameContextCreateMessage(Client, 1);

            CharacterHandler.SendCharacterStatsListMessage(Client);

            Map.Enter(this);

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

            var regainedLife = (int) Math.Floor((DateTime.Now - RegenStartTime).Value.TotalSeconds / (RegenSpeed / 10));

            if (LifePoints + regainedLife > MaxLifePoints)
                regainedLife = MaxLifePoints - LifePoints;

            Stats.Health.DamageTaken -= (short) regainedLife;
            RegenStartTime = null;
            RegenSpeed = 0;

            CharacterHandler.SendLifePointsRegenEndMessage(Client, regainedLife); 
            OnLifeRegened(regainedLife);
        }

        public void UpdateRegenedLife()
        {
            if (!IsRegenActive())
                return;

            var regainedLife = (int)Math.Floor(( DateTime.Now - RegenStartTime ).Value.TotalSeconds / (RegenSpeed / 10));

            if (LifePoints + regainedLife > MaxLifePoints)
                regainedLife = MaxLifePoints - LifePoints;

            Stats.Health.DamageTaken -= (short) regainedLife;

            CharacterHandler.SendUpdateLifePointsMessage(Client);

            RegenStartTime = DateTime.Now;

            OnLifeRegened(regainedLife);
        }

        #endregion

        #endregion

        #region Save & Load

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

            SendServerMessage(Settings.MOTD);

            OnLoggedIn();
        }

        public void LogOut()
        {
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
            catch(Exception ex)
            {
                logger.Error("Cannot perfom OnLoggout actions, but trying to Save character : {0}", ex);
            }
            finally
            {
                WorldServer.Instance.IOTaskPool.AddMessage(
                    () =>
                        {
                            SaveNow();
                            UnLoadRecord();
                        });
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

                    m_record.MapId = Map.Id;
                    m_record.CellId = Cell.Id;
                    m_record.Direction = Direction;

                    m_record.AP = (ushort) Stats[CaracteristicsEnum.AP].Base;
                    m_record.MP = (ushort) Stats[CaracteristicsEnum.MP].Base;
                    m_record.Strength = Stats[CaracteristicsEnum.Strength].Base;
                    m_record.Agility = Stats[CaracteristicsEnum.Agility].Base;
                    m_record.Chance = Stats[CaracteristicsEnum.Chance].Base;
                    m_record.Intelligence = Stats[CaracteristicsEnum.Intelligence].Base;
                    m_record.Wisdom = Stats[CaracteristicsEnum.Wisdom].Base;
                    m_record.BaseHealth = (ushort) Stats.Health.Base;
                    m_record.DamageTaken = (ushort) Stats.Health.DamageTaken;

                    m_record.EntityLook = Look;

                    m_record.Save();

                    session.Flush();
                }
            }
        }

        private void LoadRecord()
        {
            Breed = BreedManager.Instance.GetBreed(BreedId);

            Contract.Assume(Breed != null);

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

            Stats = new StatsFields(this, m_record);
            Level = ExperienceManager.Instance.GetCharacterLevel(m_record.Experience);
            LowerBoundExperience = ExperienceManager.Instance.GetCharacterLevelExperience(Level);
            UpperBoundExperience = ExperienceManager.Instance.GetCharacterNextLevelExperience(Level);

            Inventory = new Inventory(this);
            Inventory.LoadInventory();
            Spells = new SpellInventory(this);
            Spells.LoadSpells();
            Shortcuts = new ShortcutBar(this);
            Shortcuts.Load();

            m_recordLoaded = true;
        }

        private void UnLoadRecord()
        {
            if (!m_recordLoaded)
                return;

            m_recordLoaded = false;
            Inventory.Dispose();
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
                (sbyte) AlignmentSide,
                AlignmentValue,
                AlignmentGrade,
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
                0,
                0,
                0,
                false
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
                Sex == SexTypeEnum.SEX_MALE);
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
                Sex == SexTypeEnum.SEX_MALE,
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
                (short) Stats[CaracteristicsEnum.Prospecting].Total,
                0,
                (short) Stats[CaracteristicsEnum.Initiative].Total,
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

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Id);
        }
    }
}