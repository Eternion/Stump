using System;
using System.Collections.Generic;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Breeds;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.Server.WorldServer.Handlers.Chat;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Actors.Interfaces;
using Stump.Server.WorldServer.Worlds.Actors.Stats;
using Stump.Server.WorldServer.Worlds.Breeds;
using Stump.Server.WorldServer.Worlds.Dialogs;
using Stump.Server.WorldServer.Worlds.Exchanges;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Items;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Notifications;
using Stump.Server.WorldServer.Worlds.Parties;
using Stump.Server.WorldServer.Worlds.Shortcuts;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters
{
    public sealed class Character : Humanoid,
                                    IStatsOwner, IInventoryOwner
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly CharacterRecord m_record;

        public Character(CharacterRecord record, WorldClient client)
        {
            m_record = record;
            Client = client;

            LoadRecord();
        }

        #region Events

        public event Action<Character> LoggedIn;

        private void NotifyLoggedIn()
        {
            Action<Character> handler = LoggedIn;
            if (handler != null) handler(this);
        }

        public event Action<Character> LoggedOut;

        private void NotifyLoggedOut()
        {
            Action<Character> handler = LoggedOut;
            if (handler != null) handler(this);
        }

        #endregion

        #region Properties

        public WorldClient Client
        {
            get;
            private set;
        }

        public bool InWorld
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

        #endregion

        #region Inventory

        public Inventory Inventory
        {
            get;
            private set;
        }

        #endregion

        #region Position

        public override IContext Context
        {
            get
            {
                if (IsFighting())
                    return Fight;

                return Map;
            }
        }

        public Map Map
        {
            get { return Position.Map; }
            set { Position.Map = value; }
        }

        public Cell Cell
        {
            get { return Position.Cell; }
            set { Position.Cell = value; }
        }

        public DirectionsEnum Direction
        {
            get { return Position.Direction; }
            set { Position.Direction = value; }
        }

        #endregion

        #region Dialog

        public IDialoger Dialoger
        {
            get;
            private set;
        }

        public IDialog Dialog
        {
            get { return Dialoger != null ? Dialoger.Dialog : null; }
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

        public void ResetDialoger()
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
            return Dialoger != null;
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

        public byte Level
        {
            get;
            private set;
        }

        public long Experience
        {
            get { return m_record.Experience; }
            set
            {
                m_record.Experience = value;
                if (value >= UpperBoundExperience)
                {
                    byte lastLevel = Level;

                    Level = ExperienceManager.Instance.GetCharacterLevel(m_record.Experience);

                    LowerBoundExperience = ExperienceManager.Instance.GetCharacterLevelExperience(Level);
                    UpperBoundExperience = ExperienceManager.Instance.GetCharacterNextLevelExperience(Level);

                    int difference = Level - lastLevel;

                    NotifyLevelChanged(Level, difference);
                }
            }
        }

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
            get { return Stats[CaracteristicsEnum.Health].Total; }
        }

        public int MaxLifePoints
        {
            get { return ((StatsHealth) Stats[CaracteristicsEnum.Health]).TotalMax; }
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

        public void NotifyLevelChanged(byte currentlevel, int difference)
        {
            LevelChangedHandler handler = LevelChanged;
            if (handler != null)
                handler(this, currentlevel, difference);
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

        public Fights.Fight Fight
        {
            get { return Fighter.Fight; }
        }

        public FightTeam Team
        {
            get { return Fighter.Team; }
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

        #endregion

        #region Move
        public override bool StartMove(Maps.Pathfinding.MovementPath movementPath)
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
        #endregion

        #region Dialog

        public void DisplayNotification(Notification notification)
        {
            notification.Display();
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

        public FighterRefusedReasonEnum CanRequestFight(Character target)
        {
            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }

        public CharacterFighter CreateFighter(FightTeam team)
        {
            Map.Leave(this);
            StopRegen();

            ContextHandler.SendGameContextDestroyMessage(Client);
            ContextHandler.SendGameContextCreateMessage(Client, 2);

            ContextHandler.SendGameFightStartingMessage(Client, team.Fight.FightType);

            return Fighter = new CharacterFighter(this, team);
        }

        /// <summary>
        /// Rejoin the map after a fight
        /// </summary>
        public void RejoinMap()
        {
            if (!IsFighting())
                return;

            Fighter.NotifyFightLeft();
            Fighter = null;

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
            StartRegen((byte) (Rates.RegenRate*20f));
        }

        public void StartRegen(byte lifePerMinute)
        {
            if (IsRegenActive())
                StopRegen();

            RegenStartTime = DateTime.Now;
            RegenSpeed = lifePerMinute;

            CharacterHandler.SendLifePointsRegenBeginMessage(Client, RegenSpeed);
        }

        public void StopRegen()
        {
            if (!IsRegenActive())
                return;

            var regainedLife = (int) Math.Floor((DateTime.Now - RegenStartTime).Value.TotalMinutes*RegenSpeed);

            if (LifePoints + regainedLife > MaxLifePoints)
                regainedLife = MaxLifePoints - LifePoints;

            ((StatsHealth) Stats[CaracteristicsEnum.Health]).DamageTaken -= (short) regainedLife;
            RegenStartTime = null;
            RegenSpeed = 0;

            CharacterHandler.SendLifePointsRegenEndMessage(Client, regainedLife);
        }

        public void UpdateRegenedLife()
        {
            var regainedLife = (int) Math.Floor((DateTime.Now - RegenStartTime).Value.TotalMinutes*RegenSpeed);

            if (LifePoints + regainedLife > MaxLifePoints)
                regainedLife = MaxLifePoints - LifePoints;

            ((StatsHealth) Stats[CaracteristicsEnum.Health]).DamageTaken -= (short) regainedLife;

            CharacterHandler.SendUpdateLifePointsMessage(Client);

            RegenStartTime = DateTime.Now;
        }

        #endregion

        #endregion

        #region Save & Load

        /// <summary>
        ///   Spawn the character on the map. It can be called once.
        /// </summary>
        public void LogIn()
        {
            if (InWorld)
                return;

            Map.Enter(this);
            World.Instance.Enter(this);

            SendServerMessage(Settings.MOTD);

            InWorld = true;

            NotifyLoggedIn();
        }

        public void LogOut()
        {
            try
            {
                if (InWorld)
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
                }

                NotifyLoggedOut();
            }
            catch(Exception ex)
            {
                logger.Error("Cannot perfom OnLoggout actions, but trying to Save character : {0}", ex);
            }
            finally
            {
                SaveLater();
            }
        }

        public void SaveLater()
        {
            WorldServer.Instance.IOTaskPool.EnqueueTask(SaveNow);
        }

        internal void SaveNow()
        {
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
            m_record.BaseHealth = (ushort) Stats[CaracteristicsEnum.Health].Base;
            m_record.DamageTaken = (ushort) ((StatsHealth) Stats[CaracteristicsEnum.Health]).DamageTaken;


            m_record.Save();
        }

        private void LoadRecord()
        {
            Breed = BreedManager.Instance.GetBreed(BreedId);

            Map map = World.Instance.GetMap(m_record.MapId);
            Position = new ObjectPosition(
                map,
                map.Cells[m_record.CellId],
                m_record.Direction);

            Stats = new StatsFields(this, m_record);
            Level = ExperienceManager.Instance.GetCharacterLevel(m_record.Experience);
            LowerBoundExperience = ExperienceManager.Instance.GetCharacterLevelExperience(Level);
            UpperBoundExperience = ExperienceManager.Instance.GetCharacterNextLevelExperience(Level);

            Inventory = new Inventory(this, m_record.Inventory);
            Spells = new SpellInventory(this);
            Shortcuts = new ShortcutBar(this);
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
                Map.Id);
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
    }
}