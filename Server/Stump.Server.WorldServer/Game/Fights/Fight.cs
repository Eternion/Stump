using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Core.Pool;
using Stump.Core.Timers;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.Server.WorldServer.Handlers.Context;
using FightLoot = Stump.Server.WorldServer.Game.Fights.Results.FightLoot;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using TriggerType = Stump.Server.WorldServer.Game.Fights.Triggers.TriggerType;

namespace Stump.Server.WorldServer.Game.Fights
{
    public delegate void FightWinnersDelegate(IFight fight, FightTeam winners, FightTeam losers, bool draw);

    public interface IFight : ICharacterContainer
    {
        int Id
        {
            get;
        }

        Map Map
        {
            get;
        }

        Cell[] Cells
        {
            get;
        }

        FightTypeEnum FightType
        {
            get;
        }

        bool IsPvP
        {
            get;
        }

        bool IsMultiAccountRestricted
        {
            get;
        }

        FightState State
        {
            get;
        }

        bool IsStarted
        {
            get;
        }

        DateTime CreationTime
        {
            get;
        }

        DateTime StartTime
        {
            get;
        }

        short AgeBonus
        {
            get;
        }

        FightTeam ChallengersTeam
        {
            get;
        }

        FightTeam DefendersTeam
        {
            get;
        }

        FightTeam Winners
        {
            get;
        }

        FightTeam Losers
        {
            get;
        }

        bool Draw
        {
            get;
        }

        TimeLine TimeLine
        {
            get;
        }
        ReadOnlyCollection<FightActor> Fighters
        {
            get;
        }

        ReadOnlyCollection<FightActor> Leavers
        {
            get;
        }

        ReadOnlyCollection<FightSpectator> Spectators
        {
            get;
        }

        FightActor FighterPlaying
        {
            get;
        }

        DateTime TurnStartTime
        {
            get;
        }

        ReadyChecker ReadyChecker
        {
            get;
        }

        bool SpectatorClosed
        {
            get;
        }

        bool BladesVisible
        {
            get;
        }

        FightLoot TaxCollectorLoot
        {
            get;
        }

        SequenceTypeEnum Sequence
        {
            get;
        }

        bool IsSequencing
        {
            get;
        }

        bool WaitAcknowledgment
        {
            get;
        }

        bool IsDeathTemporarily
        {
            get;
        }

        bool CanKickPlayer
        {
            get;
        }

        /// <summary>
        /// Do not modify, just read
        /// </summary>
        WorldClientCollection Clients
        {
            get;
        }

        /// <summary>
        /// Do not modify, just read
        /// </summary>
        WorldClientCollection SpectatorClients
        {
            get;
        }

        event Action FightStarted;
        event Action FightEnded;

        void Initialize();
        void StartFighting();
        bool CheckFightEnd();
        void CancelFight();
        void EndFight();
        event FightWinnersDelegate WinnersDetermined;
        void StartPlacement();
        void ShowBlades();
        void HideBlades();
        void UpdateBlades(FightTeam team);
        bool FindRandomFreeCell(FightActor fighter, out Cell cell, bool placement = true);
        bool RandomnizePosition(FightActor fighter);
        void RandomnizePositions(FightTeam team);
        DirectionsEnum FindPlacementDirection(FightActor fighter);
        bool KickFighter(FightActor kicker, FightActor fighter);

        /// <summary>
        ///   Check if a character can change position (before the fight is started).
        /// </summary>
        /// <param name = "fighter"></param>
        /// <param name="cell"></param>
        /// <returns>If change is possible</returns>
        bool CanChangePosition(FightActor fighter, Cell cell);

        void ToggleSpectatorClosed(bool state);
        bool CanSpectatorJoin(Character spectator);
        bool AddSpectator(FightSpectator spectator);
        void RemoveSpectator(FightSpectator spectator);
        void RemoveAllSpectators();
        void StartTurn();
        event Action<IFight, FightActor> TurnStarted;
        void StopTurn();
        void SwitchFighters(FightActor fighter1, FightActor fighter2);
        IEnumerable<Buff> GetBuffs();
        void UpdateBuff(Buff buff);
        bool StartSequence(SequenceTypeEnum sequenceType);
        bool EndSequence(SequenceTypeEnum sequenceType, bool force = false);
        void EndAllSequences();
        void AcknowledgeAction();
        IEnumerable<MarkTrigger> GetTriggers();
        bool ShouldTriggerOnMove(Cell cell);
        bool ShouldTriggerOnMove(Cell cell, FightActor actor);
        MarkTrigger[] GetTriggers(Cell cell);
        void AddTriger(MarkTrigger trigger);
        void RemoveTrigger(MarkTrigger trigger);
        void TriggerMarks(Cell cell, FightActor trigger, TriggerType triggerType);
        void DecrementGlyphDuration(FightActor caster);
        int PopNextTriggerId();
        void FreeTriggerId(int id);
        IEnumerable<Character> GetAllCharacters();
        IEnumerable<Character> GetAllCharacters(bool withSpectators = false);
        void ForEach(Action<Character> action);
        void ForEach(Action<Character> action, bool withSpectators = false);
        void ForEach(Action<Character> action, Character except, bool withSpectators = false);
        bool IsCellFree(Cell cell);
        int GetFightDuration();
        int GetTurnTimeLeft();
        sbyte GetNextContextualId();
        void FreeContextualId(sbyte id);
        FightActor GetOneFighter(int id);
        FightActor GetOneFighter(Cell cell);
        FightActor GetOneFighter(Predicate<FightActor> predicate);
        T GetOneFighter<T>(int id) where T : FightActor;
        T GetOneFighter<T>(Cell cell) where T : FightActor;
        T GetOneFighter<T>(Predicate<T> predicate) where T : FightActor;
        T GetFirstFighter<T>(int id) where T : FightActor;
        T GetFirstFighter<T>(Cell cell) where T : FightActor;
        T GetFirstFighter<T>(Predicate<T> predicate) where T : FightActor;
        ReadOnlyCollection<FightActor> GetAllFighters();
        ReadOnlyCollection<FightActor> GetLeavers();
        CharacterFighter GetLeaver(int characterId);
        ReadOnlyCollection<FightSpectator> GetSpectators();
        IEnumerable<Character> GetCharactersAndSpectators();
        IEnumerable<FightActor> GetFightersAndLeavers();
        IEnumerable<FightActor> GetAllFighters(Cell[] cells);
        IEnumerable<FightActor> GetAllFighters(IEnumerable<Cell> cells);
        IEnumerable<FightActor> GetAllFighters(Predicate<FightActor> predicate);
        IEnumerable<T> GetAllFighters<T>() where T : FightActor;
        IEnumerable<T> GetAllFighters<T>(Predicate<T> predicate) where T : FightActor;
        IEnumerable<int> GetDeadFightersIds();
        IEnumerable<int> GetAliveFightersIds();
        FightCommonInformations GetFightCommonInformations();
        FightExternalInformations GetFightExternalInformations();
        bool CanBeSeen(Cell from, Cell to, bool throughEntities = false);
        int GetPlacementTimeLeft();
    }

    // this is necessary since we can't read static field dynamically in a generic class
    public static class FightConfiguration
    {
        
        [Variable]
        public static int PlacementPhaseTime = 30000;

        /// <summary>
        ///   Delay for player's turn
        /// </summary>
        [Variable]
        public static int TurnTime = 35000;

        /// <summary>
        ///   Delay before force turn to end
        /// </summary>
        [Variable]
        public static int TurnEndTimeOut = 5000;

        /// <summary>
        ///   Delay before force turn to end
        /// </summary>
        [Variable]
        public static int EndFightTimeOut = 10000;
    }

    public abstract class Fight<TBlueTeam,TRedTeam> : WorldObjectsContext, IFight
        where TRedTeam : FightTeam
        where TBlueTeam : FightTeam
    {
        protected readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Config


        #endregion

        #region Events

        #endregion

        #region Constructor

        protected Fight(int id, Map fightMap, TBlueTeam defendersTeam, TRedTeam challengersTeam)
        {
            Id = id;
            Map = fightMap;
            DefendersTeam = defendersTeam;
            DefendersTeam.Fight = this;
            ChallengersTeam = challengersTeam;
            ChallengersTeam.Fight = this;
            m_teams = new[] {(FightTeam)ChallengersTeam, DefendersTeam};

            TimeLine = new TimeLine(this);
            m_leavers = new List<FightActor>();
            m_spectators = new List<FightSpectator>();

            DefendersTeam.FighterAdded += OnFighterAdded;
            DefendersTeam.FighterRemoved += OnFighterRemoved;
            ChallengersTeam.FighterAdded += OnFighterAdded;
            ChallengersTeam.FighterRemoved += OnFighterRemoved;

            CreationTime = DateTime.Now;
            TaxCollectorLoot = new FightLoot();
        }

        #endregion

        #region Properties

        protected readonly ReversedUniqueIdProvider m_contextualIdProvider = new ReversedUniqueIdProvider(0);
        protected readonly UniqueIdProvider m_triggerIdProvider = new UniqueIdProvider();

        protected readonly List<Buff> m_buffs = new List<Buff>();

        protected TimedTimerEntry m_placementTimer;
        protected TimedTimerEntry m_turnTimer;

        private bool m_isInitialized;
        private bool m_disposed;

        protected FightTeam[] m_teams;

        public int Id
        {
            get;
            private set;
        }

        public Map Map
        {
            get;
            private set;
        }

        public override Cell[] Cells
        {
            get
            {
                return Map.Cells;
            }
        }

        protected override IReadOnlyCollection<WorldObject> Objects
        {
            get
            {
                return Fighters;
            }
        }

        public abstract FightTypeEnum FightType
        {
            get;
        }

        public abstract bool IsPvP
        {
            get;
        }

        public virtual bool IsMultiAccountRestricted
        {
            get { return false; }
        }

        public FightState State
        {
            get;
            private set;
        }

        public bool IsStarted
        {
            get;
            private set;
        }

        public DateTime CreationTime
        {
            get;
            private set;
        }

        public DateTime StartTime
        {
            get;
            private set;
        }

        public short AgeBonus
        {
            get;
            protected set;
        }

        FightTeam IFight.ChallengersTeam
        {
            get { return ChallengersTeam; }
        }
        FightTeam IFight.DefendersTeam
        {
            get { return DefendersTeam; }
        }

        public TRedTeam ChallengersTeam
        {
            get;
            private set;
        }

        public TBlueTeam DefendersTeam
        {
            get;
            private set;
        }

        public FightTeam Winners
        {
            get;
            protected set;
        }

        public FightTeam Losers
        {
            get;
            protected set;
        }

        public bool Draw
        {
            get;
            protected set;
        }

        public TimeLine TimeLine
        {
            get;
            private set;
        }

        public FightActor FighterPlaying
        {
            get { return TimeLine.Current; }
        }

        public DateTime TurnStartTime
        {
            get;
            protected set;
        }

        public ReadyChecker ReadyChecker
        {
            get;
            protected set;
        }

        public ReadOnlyCollection<FightActor> Fighters
        {
            get { return TimeLine.Fighters.AsReadOnly(); }
        }

        public ReadOnlyCollection<FightActor> Leavers
        {
            get { return m_leavers.AsReadOnly(); }
        }

        public ReadOnlyCollection<FightSpectator> Spectators
        {
            get { return m_spectators.AsReadOnly(); }
        }

        public bool SpectatorClosed
        {
            get;
            private set;
        }

        public bool BladesVisible
        {
            get;
            private set;
        }

        public FightLoot TaxCollectorLoot
        {
            get;
            private set;
        }

        public virtual bool IsDeathTemporarily
        {
            get { return false; }
        }

        public virtual bool CanKickPlayer
        {
            get { return true; }
        }

        #endregion

        #region Phases

        protected void SetFightState(FightState state)
        {
            State = state;

            UnBindFightersEvents();
            BindFightersEvents();

            OnStateChanged();
        }

        protected virtual void OnStateChanged()
        {
            if (State != FightState.Placement && BladesVisible)
                HideBlades();
        }

        public void Initialize()
        {
            if (m_isInitialized)
                return;

            ProcessInitialization();

            m_isInitialized = true;
        }

        protected virtual void ProcessInitialization()
        {
        }

        public virtual void StartFighting()
        {
            if (State != FightState.Placement &&
                State != FightState.NotStarted) // we can imagine a fight without placement phase
                return;

            SetFightState(FightState.Fighting);
            StartTime = DateTime.Now;
            IsStarted = true;

            HideBlades();

            TimeLine.OrderLine();

            ContextHandler.SendGameEntitiesDispositionMessage(Clients, GetAllFighters());
            ContextHandler.SendGameFightStartMessage(Clients);
            ContextHandler.SendGameFightTurnListMessage(Clients, this);
            ForEach(entry => ContextHandler.SendGameFightSynchronizeMessage(entry.Client, this), true);
            OnFightStarted();

            StartTurn();
        }

        #region EndFight

        public bool CheckFightEnd()
        {
            if (!ChallengersTeam.AreAllDead() && !DefendersTeam.AreAllDead() && Clients.Count > 0)
                return false;

            EndFight();
            return true;
        }

        public void CancelFight()
        {
            if (!CanCancelFight())
                return;

            if (State != FightState.Placement)
            {
                EndFight();
                return;
            }

            SetFightState(FightState.Ended);

            ContextHandler.SendGameFightEndMessage(Clients, this);

            foreach (var character in GetCharactersAndSpectators())
            {
                character.RejoinMap();
            }

            Dispose();
        }

        public void EndFight()
        {
            if (State == FightState.Placement)
                CancelFight();

            if (State == FightState.Ended)
                return;

            SetFightState(FightState.Ended);

            if (m_turnTimer != null)
                m_turnTimer.Dispose();

            EndAllSequences();

            if (ReadyChecker != null)
            {
                ReadyChecker.Cancel();
            }

            ReadyChecker = ReadyChecker.RequestCheck(this, OnFightEnded, actors => OnFightEnded());
        }

        public event Action FightStarted;

        protected virtual void OnFightStarted()
        {
            foreach (var fighter in Fighters)
            {
                fighter.FightStartPosition = fighter.Position.Clone();
            }

            var handler = FightStarted;
            if (handler != null)
                handler();
        }

        public event Action FightEnded;
        protected virtual void OnFightEnded()
        {
            ReadyChecker = null;
            DeterminsWinners();

            var results = GenerateResults().ToList();

            ApplyResults(results);

            ContextHandler.SendGameFightEndMessage(Clients, this, results.Select(entry => entry.GetFightResultListEntry()));

            ResetFightersProperties();
            foreach (var character in GetCharactersAndSpectators())
            {
                character.RejoinMap();
            }

            Dispose();

            var handler = FightEnded;
            if (handler != null)
                handler();
        }

        public event FightWinnersDelegate WinnersDetermined;

        protected virtual void OnWinnersDetermined(FightTeam winners, FightTeam losers, bool draw)
        {
            var handler = WinnersDetermined;
            if (handler != null) handler(this, winners, losers, draw);
        }

        protected virtual void DeterminsWinners()
        {
            if (DefendersTeam.AreAllDead() && !ChallengersTeam.AreAllDead())
            {
                Winners = ChallengersTeam;
                Losers = DefendersTeam;
                Draw = false;
            }
            else if (!DefendersTeam.AreAllDead() && ChallengersTeam.AreAllDead())
            {
                Winners = DefendersTeam;
                Losers = ChallengersTeam;
                Draw = false;
            }

            else Draw = true;

            OnWinnersDetermined(Winners, Losers, Draw);
        }

        protected void ResetFightersProperties()
        {
            foreach (var fighter in Fighters)
            {
                fighter.ResetFightProperties();
            }
        }

        protected abstract IEnumerable<IFightResult> GenerateResults();

        protected virtual IEnumerable<IFightResult> GenerateLeaverResults(CharacterFighter leaver,
            out IFightResult leaverResult)
        {
            leaverResult = null;
            var list = new List<IFightResult>();
            foreach (var fighter in GetFightersAndLeavers().Where(entry => !(entry is SummonedFighter) && !(entry is SummonedBomb)))
            {
                var result =
                    fighter.GetFightResult(fighter.Team == leaver.Team
                        ? FightOutcomeEnum.RESULT_LOST
                        : FightOutcomeEnum.RESULT_VICTORY);

                if (fighter == leaver)
                    leaverResult = result;

                list.Add(result);
            }

            return list;
        }

        protected void ApplyResults(IEnumerable<IFightResult> results)
        {
            foreach (var fightResult in results.Where(fightResult => !fightResult.HasLeft))
            {
                fightResult.Apply();
            }
        }

        protected void Dispose()
        {
            if (m_disposed)
                return;

            m_disposed = true;

            foreach (var fighter in Fighters)
            {
                fighter.Delete();
            }

            OnDisposed();

            UnBindFightersEvents();

            Map.RemoveFight(this);
            FightManager.Instance.Remove(this);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDisposed()
        {
            if (ReadyChecker != null)
                ReadyChecker.Cancel();

            if (m_placementTimer != null)
                m_placementTimer.Dispose();

            if (m_turnTimer != null)
                m_turnTimer.Dispose();
        }

        #endregion

        #region Placement

        public virtual void StartPlacement()
        {
            if (State != FightState.NotStarted)
                return;

            SetFightState(FightState.Placement);

            RandomnizePositions(ChallengersTeam);
            RandomnizePositions(DefendersTeam);

            TimeLine.OrderLine();
            ContextHandler.SendGameFightTurnListMessage(Clients, this);

            ShowBlades();
            Map.AddFight(this);
        }

        #region Blades

        private void FindBladesPlacement()
        {
            if (ChallengersTeam.Leader.MapPosition.Cell.Id != DefendersTeam.Leader.MapPosition.Cell.Id)
            {
                ChallengersTeam.BladePosition = ChallengersTeam.Leader.MapPosition.Clone();
                DefendersTeam.BladePosition = DefendersTeam.Leader.MapPosition.Clone();
            }
            else
            {
                var cell = Map.GetRandomAdjacentFreeCell(ChallengersTeam.Leader.MapPosition.Point);

                // if cell not found we superpose both blades
                if (cell == null)
                {
                    ChallengersTeam.BladePosition = ChallengersTeam.Leader.MapPosition.Clone();
                }
                else // else we take an adjacent cell
                {
                    var pos = ChallengersTeam.Leader.MapPosition.Clone();
                    pos.Cell = cell;
                    ChallengersTeam.BladePosition = pos;
                }

                DefendersTeam.BladePosition = DefendersTeam.Leader.MapPosition.Clone();
            }
        }

        public void ShowBlades()
        {
            if (BladesVisible || State != FightState.Placement)
                return;

            if (ChallengersTeam.BladePosition == null ||
                DefendersTeam.BladePosition == null)
                FindBladesPlacement();

            ContextHandler.SendGameRolePlayShowChallengeMessage(Map.Clients, this);

            ChallengersTeam.TeamOptionsChanged += OnTeamOptionsChanged;
            DefendersTeam.TeamOptionsChanged += OnTeamOptionsChanged;

            BladesVisible = true;
        }

        public void HideBlades()
        {
            if (!BladesVisible)
                return;

            ContextHandler.SendGameRolePlayRemoveChallengeMessage(Map.Clients, this);

            ChallengersTeam.TeamOptionsChanged -= OnTeamOptionsChanged;
            DefendersTeam.TeamOptionsChanged -= OnTeamOptionsChanged;

            BladesVisible = false;
        }

        public void UpdateBlades(FightTeam team)
        {
            if (!BladesVisible)
                return;

            ContextHandler.SendGameFightUpdateTeamMessage(Map.Clients, this, team);
        }

        private void OnTeamOptionsChanged(FightTeam team, FightOptionsEnum option)
        {
            ContextHandler.SendGameFightOptionStateUpdateMessage(Clients, team, option, team.GetOptionState(option));
            ContextHandler.SendGameFightOptionStateUpdateMessage(Map.Clients, team, option, team.GetOptionState(option));
        }

        #endregion
        
        public virtual int GetPlacementTimeLeft()
        {
            return 0;
        }

        #region Placement methods

        

        public bool FindRandomFreeCell(FightActor fighter, out Cell cell, bool placement = true)
        {
            var availableCells = fighter.Team.PlacementCells.Where(entry => GetOneFighter(entry) == null || GetOneFighter(entry) == fighter).ToArray();

            var random = new Random();

            if (availableCells.Length == 0 && placement)
            {
                cell = null;
                return false;
            }

            // if not in placement phase, get a random free cell on the map
            if (availableCells.Length == 0 && !placement)
            {
                var cells = Enumerable.Range(0, (int) MapPoint.MapSize).ToList();
                foreach (var actor in GetAllFighters(actor => cells.Contains(actor.Cell.Id)))
                {
                    cells.Remove(actor.Cell.Id);
                }

                cell = Map.Cells[cells[random.Next(cells.Count)]];

                return true;
            }

            cell = availableCells[random.Next(availableCells.Length)];

            return true;
        }


        public bool RandomnizePosition(FightActor fighter)
        {
            if (State != FightState.Placement)
                throw new Exception("State != Placement, cannot random placement position");

            Cell cell;
            if (!FindRandomFreeCell(fighter, out cell))
            {
                fighter.LeaveFight(); // no place more than we kick the actor to avoid bugs
                return false;
            }

            fighter.ChangePrePlacement(cell);
            return true;
        }

        public void RandomnizePositions(FightTeam team)
        {
            if (State != FightState.Placement)
                throw new Exception("State != Placement, cannot random placement position");

            var shuffledCells = team.PlacementCells.Shuffle();
            var enumerator = shuffledCells.GetEnumerator();
            foreach (var fighter in team.GetAllFighters())
            {
                enumerator.MoveNext();

                fighter.ChangePrePlacement(enumerator.Current);
            }
            enumerator.Dispose();
        }

        public DirectionsEnum FindPlacementDirection(FightActor fighter)
        {
            if (State != FightState.Placement)
                throw new Exception("State != Placement, cannot give placement direction");

            var team = fighter.OpposedTeam;

            Tuple<Cell, uint> closerCell = null;
            foreach (var opposant in team.GetAllFighters())
            {
                var point = opposant.Position.Point;

                if (closerCell == null)
                    closerCell = Tuple.Create(opposant.Cell,
                                              fighter.Position.Point.DistanceToCell(point));
                else
                {
                    if (fighter.Position.Point.DistanceToCell(point) < closerCell.Item2)
                        closerCell = Tuple.Create(opposant.Cell,
                                                  fighter.Position.Point.DistanceToCell(point));
                }
            }

            return closerCell == null ? fighter.Position.Direction : fighter.Position.Point.OrientationTo(new MapPoint(closerCell.Item1), false);
        }

        protected virtual bool CanKickFighter(FightActor kicker, FightActor kicked)
        {
            return State == FightState.Placement && kicker.IsTeamLeader() && kicked.Team == kicker.Team;
        }

        public bool KickFighter(FightActor kicker, FightActor fighter)
        {
            if (!Fighters.Contains(fighter))
                return false;

            if (!CanKickFighter(kicker, fighter))
                return false;

            fighter.Team.RemoveFighter(fighter);

            var characterFighter = fighter as CharacterFighter;
            if (characterFighter != null)
            {
                characterFighter.Character.RejoinMap();
            }

            CheckFightEnd();

            return true;
        }

        /// <summary>
        ///   Set the ready state of a character
        /// </summary>
        protected virtual void OnSetReady(FightActor fighter, bool isReady)
        {
            if (State != FightState.Placement)
                return;

            ContextHandler.SendGameFightHumanReadyStateMessage(Clients, fighter);

            if (ChallengersTeam.AreAllReady() && DefendersTeam.AreAllReady())
                StartFighting();
        }


        /// <summary>
        ///   Check if a character can change position (before the fight is started).
        /// </summary>
        /// <param name = "fighter"></param>
        /// <param name="cell"></param>
        /// <returns>If change is possible</returns>
        public virtual bool CanChangePosition(FightActor fighter, Cell cell)
        {
            var figtherOnCell = GetOneFighter(cell);

            return State == FightState.Placement &&
                   fighter.Team.PlacementCells.Contains(cell) &&
                   (figtherOnCell == fighter || figtherOnCell == null);
        }

        protected virtual void OnChangePreplacementPosition(FightActor fighter, ObjectPosition objectPosition)
        {
            UpdateFightersPlacementDirection();

            ContextHandler.SendGameEntitiesDispositionMessage(Clients, GetAllFighters());
        }

        protected void UpdateFightersPlacementDirection()
        {
            foreach (FightActor fighter in Fighters)
            {
                fighter.Position.Direction = FindPlacementDirection(fighter);
            }
        }

        #endregion

        #region Kick



        #endregion

        #endregion

        #endregion

        #region Add/Remove Fighter

        protected virtual void OnFighterAdded(FightTeam team, FightActor actor)
        {
            if (State == FightState.Ended)
            {
                throw new Exception("Fight ended");
            }

            if (actor is SummonedFighter)
            {
                OnSummonAdded(actor as SummonedFighter);
                return;
            }

            if (actor is SummonedBomb)
            {
                OnBombAdded(actor as SummonedBomb);
                return;
            }

            TimeLine.Fighters.Add(actor);
            BindFighterEvents(actor);

            if (State == FightState.Placement)
            {
                TimeLine.OrderLine();
                if (!RandomnizePosition(actor))
                    return;
            }

            if (actor is CharacterFighter)
                OnCharacterAdded(actor as CharacterFighter);

            ForEach(entry => ContextHandler.SendGameFightShowFighterMessage(entry.Client, actor), true);

            // update blades if shown
            if (BladesVisible)
                UpdateBlades(team);
            
            ContextHandler.SendGameFightTurnListMessage(Clients, this);
        }

        protected virtual void OnSummonAdded(SummonedFighter fighter)
        {
            TimeLine.InsertFighter(fighter, TimeLine.Fighters.IndexOf(fighter.Summoner) + 1);
            BindFighterEvents(fighter);

            ContextHandler.SendGameFightTurnListMessage(Clients, this);
        }
        protected virtual void OnBombAdded(SummonedBomb bomb)
        {
            TimeLine.InsertFighter(bomb, TimeLine.Fighters.IndexOf(bomb.Summoner) + 1);
            BindFighterEvents(bomb);

            ContextHandler.SendGameFightTurnListMessage(Clients, this);
        }

        protected virtual void OnCharacterAdded(CharacterFighter fighter)
        {
            var character = fighter.Character;

            character.RealLook.RemoveAuras();
            character.RefreshActor();

            if (character.ArenaPopup != null)
                character.ArenaPopup.Deny();

            Clients.Add(character.Client);

            SendGameFightJoinMessage(fighter);

            if (State == FightState.Placement || State == FightState.NotStarted)
            {
                ContextHandler.SendGameFightPlacementPossiblePositionsMessage(character.Client, this, (sbyte)fighter.Team.Id);
            }

            foreach (var fightMember in GetAllFighters())
                ContextHandler.SendGameFightShowFighterMessage(character.Client, fightMember);

            ContextHandler.SendGameEntitiesDispositionMessage(character.Client, GetAllFighters());

            ContextHandler.SendGameFightUpdateTeamMessage(character.Client, this, ChallengersTeam);
            ContextHandler.SendGameFightUpdateTeamMessage(character.Client, this, DefendersTeam);

            ContextHandler.SendGameFightUpdateTeamMessage(Clients, this, fighter.Team);
        }


        protected virtual void OnFighterRemoved(FightTeam team, FightActor actor)
        {
            if (actor is SummonedFighter)
            {
                OnSummonRemoved(actor as SummonedFighter);
                return;
            }

            if (actor is SummonedBomb)
            {
                OnBombRemoved(actor as SummonedBomb);
                return;
            }

            TimeLine.RemoveFighter(actor);
            UnBindFighterEvents(actor);

            if (actor is CharacterFighter)
                OnCharacterRemoved(actor as CharacterFighter);

            switch (State)
            {
                case FightState.Placement:
                    ContextHandler.SendGameFightRemoveTeamMemberMessage(Clients, actor);
                    break;
                case FightState.Fighting:
                    ContextHandler.SendGameContextRemoveElementMessage(Clients, actor);
                    break;
            }

            if (BladesVisible)
                UpdateBlades(team);
        }

        protected virtual void OnSummonRemoved(SummonedFighter fighter)
        {
            TimeLine.RemoveFighter(fighter);
            UnBindFighterEvents(fighter);

            ContextHandler.SendGameFightTurnListMessage(Clients, this);
        }
        protected virtual void OnBombRemoved(SummonedBomb bomb)
        {
            TimeLine.RemoveFighter(bomb);
            UnBindFighterEvents(bomb);

            ContextHandler.SendGameFightTurnListMessage(Clients, this);
        }

        protected virtual void OnCharacterRemoved(CharacterFighter fighter)
        {
            Clients.Remove(fighter.Character.Client);
        }


        #endregion

        #region Spectators

        public void ToggleSpectatorClosed(bool state)
        {
            SpectatorClosed = state;

            // Spectator mode Activated/Disabled
            BasicHandler.SendTextInformationMessage(Clients, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, (short)( SpectatorClosed ? 40 : 39 ));

            if (state)
                RemoveAllSpectators();

            ContextHandler.SendGameFightOptionStateUpdateMessage(Clients, ChallengersTeam, 0, SpectatorClosed);
            ContextHandler.SendGameFightOptionStateUpdateMessage(Clients, DefendersTeam, 0, SpectatorClosed);
        }

        public virtual bool CanSpectatorJoin(Character spectator)
        {
            return !SpectatorClosed && State == FightState.Fighting;
        }

        public bool AddSpectator(FightSpectator spectator)
        {
            if (!CanSpectatorJoin(spectator.Character))
                return false;

            m_spectators.Add(spectator);
            spectator.JoinTime = DateTime.Now;
            spectator.Left += OnSpectectorLeft;
            spectator.Character.LoggedOut += OnSpectatorLoggedOut;

            Clients.Add(spectator.Client);
            SpectatorClients.Add(spectator.Client);

            OnSpectatorAdded(spectator);

            return true;
        }

        protected virtual void OnSpectatorAdded(FightSpectator spectator)
        {
            SendGameFightJoinMessage(spectator);

            foreach (var fighter in GetAllFighters())
            {
                ContextHandler.SendGameFightShowFighterMessage(spectator.Client, fighter);
            }

            ContextHandler.SendGameFightTurnListMessage(spectator.Client, this);
            ContextHandler.SendGameFightSpectateMessage(spectator.Client, this);
            ContextHandler.SendGameFightNewRoundMessage(spectator.Client, TimeLine.RoundNumber);

            CharacterHandler.SendCharacterStatsListMessage(spectator.Client);

            // Spectator 'X' joined
            BasicHandler.SendTextInformationMessage(Clients, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 36, spectator.Character.Name);

            if (TimeLine.Current != null)
            {
                ContextHandler.SendGameFightTurnResumeMessage(spectator.Client, FighterPlaying, GetTurnTimeLeft());
            }
        }

        protected virtual void OnSpectatorLoggedOut(Character character)
        {
            if (!character.IsSpectator())
                return;

            OnSpectectorLeft(character.Spectator);
        }

        protected virtual void OnSpectectorLeft(FightSpectator spectator)
        {
            RemoveSpectator(spectator);
        }

        public void RemoveSpectator(FightSpectator spectator)
        {
            m_spectators.Remove(spectator);

            Clients.Remove(spectator.Character.Client);
            SpectatorClients.Remove(spectator.Client);

            spectator.Left -= OnSpectectorLeft;
            spectator.Character.LoggedOut -= OnSpectatorLoggedOut;

            OnSpectatorRemoved(spectator);
        }

        protected virtual void OnSpectatorRemoved(FightSpectator spectator)
        {
            spectator.Character.RejoinMap();
        }

        public void RemoveAllSpectators()
        {
            foreach (var spectator in m_spectators.GetRange(0, Spectators.Count))
            {
                RemoveSpectator(spectator);
            }
        }

        #endregion

        #region Turn Management

        public void StartTurn()
        {
            if (State != FightState.Fighting)
                return;

            if (!CheckFightEnd())
            {
                OnTurnStarted();
            }
        }

        public event Action<IFight, FightActor> TurnStarted;

        protected virtual void OnTurnStarted()
        {
            StartSequence(SequenceTypeEnum.SEQUENCE_TURN_END);
            FighterPlaying.TriggerBuffs(BuffTriggerType.TURN_BEGIN);
            FighterPlaying.DecrementAllCastedBuffsDuration();
            DecrementGlyphDuration(FighterPlaying);
            TriggerMarks(FighterPlaying.Cell, FighterPlaying, TriggerType.TURN_BEGIN);
            EndSequence(SequenceTypeEnum.SEQUENCE_TURN_END);

            // can die with triggers
            if (CheckFightEnd())
                return;
            
            if (TimeLine.NewRound)
                ContextHandler.SendGameFightNewRoundMessage(Clients, TimeLine.RoundNumber);

            if (FighterPlaying.IsDead() || FighterPlaying.MustSkipTurn())
            {
                FighterPlaying.ResetUsedPoints();
                PassTurn();
                return;
            }

            ContextHandler.SendGameFightTurnStartMessage(Clients, FighterPlaying.Id,
                                                         FightConfiguration.TurnTime);

            ForEach(entry => ContextHandler.SendGameFightSynchronizeMessage(entry.Client, this), true);
            ForEach(entry => entry.RefreshStats());

            FighterPlaying.TurnStartPosition = FighterPlaying.Position.Clone();

            TurnStartTime = DateTime.Now;
            m_turnTimer = Map.Area.CallDelayed(FightConfiguration.TurnTime, StopTurn);

            var evnt = TurnStarted;
            if (evnt != null)
                evnt(this, FighterPlaying);
        }

        public void StopTurn()
        {
            if (State != FightState.Fighting)
                return;

            if (m_turnTimer != null)
                m_turnTimer.Dispose();

            if (ReadyChecker != null)
            {
                logger.Debug("Last ReadyChecker was not disposed. (Stop Turn)");
                ReadyChecker.Cancel();
                ReadyChecker = null;
            }
            
            if (CheckFightEnd())
                return;

            OnTurnStopped();

            ReadyChecker = ReadyChecker.RequestCheck(this, PassTurn, LagAndPassTurn);
        }

        protected virtual void OnTurnStopped()
        {
            StartSequence(SequenceTypeEnum.SEQUENCE_TURN_END);
            FighterPlaying.TriggerBuffs(BuffTriggerType.TURN_END);
            FighterPlaying.TriggerBuffsRemovedOnTurnEnd();
            TriggerMarks(FighterPlaying.Cell, FighterPlaying, TriggerType.TURN_END);
            FighterPlaying.ResetUsedPoints();
            EndSequence(SequenceTypeEnum.SEQUENCE_TURN_END);

            // can die with triggers
            if (CheckFightEnd())
                return;

            if (IsSequencing)
                EndSequence(Sequence, true);

            if (WaitAcknowledgment)
                AcknowledgeAction();

            ContextHandler.SendGameFightTurnEndMessage(Clients, FighterPlaying);
        }

        protected void LagAndPassTurn(NamedFighter[] laggers)
        {
            // some guys are lagging !
            OnLaggersSpotted(laggers);

            PassTurn();
        }

        protected void PassTurn()
        {
            if (State != FightState.Fighting)
                return;

            ReadyChecker = null;

            if (CheckFightEnd())
                return;

            if (!TimeLine.SelectNextFighter())
            {
                if (!CheckFightEnd())
                {
                    logger.Error("Something goes wrong : no more actors are available to play but the fight is not ended");
                }

                return;
            }

            OnTurnPassed();

            StartTurn();
        }

        protected virtual void OnTurnPassed()
        {
            if (IsSequencing)
                EndSequence(Sequence, true);

            if (WaitAcknowledgment)
                AcknowledgeAction();
        }

        #endregion

        #region Events Binders

        private void UnBindFightersEvents()
        {
            foreach (var fighter in Fighters)
            {
                UnBindFighterEvents(fighter);
            }
        }

        private void UnBindFighterEvents(FightActor actor)
        {
            actor.ReadyStateChanged -= OnSetReady;
            actor.PrePlacementChanged -= OnChangePreplacementPosition;
            actor.FighterLeft -= OnPlayerLeft;

            actor.StartMoving -= OnStartMoving;
            actor.StopMoving -= OnStopMoving;
            actor.PositionChanged -= OnPositionChanged;
            actor.FightPointsVariation -= OnFightPointsVariation;
            actor.LifePointsChanged -= OnLifePointsChanged;
            actor.DamageReducted -= OnDamageReducted;
            actor.SpellCasting -= OnSpellCasting;
            actor.SpellCasted -= OnSpellCasted;
            actor.WeaponUsed -= OnCloseCombat;
            actor.BuffAdded -= OnBuffAdded;
            actor.BuffRemoved -= OnBuffRemoved;
            actor.Dead -= OnDead;


            var fighter = actor as CharacterFighter;

            if (fighter != null)
            {
                fighter.Character.LoggedOut -= OnPlayerLoggout;
            }
        }

        private void BindFightersEvents()
        {
            foreach (var fighter in Fighters)
            {
                BindFighterEvents(fighter);
            }
        }

        private void BindFighterEvents(FightActor actor)
        {
            if (State == FightState.Placement)
            {
                actor.FighterLeft += OnPlayerLeft;
                actor.ReadyStateChanged += OnSetReady;
                actor.PrePlacementChanged += OnChangePreplacementPosition;
            }

            if (State == FightState.Fighting)
            {
                actor.FighterLeft += OnPlayerLeft;
                actor.StartMoving += OnStartMoving;
                actor.StopMoving += OnStopMoving;
                actor.PositionChanged += OnPositionChanged;
                actor.FightPointsVariation += OnFightPointsVariation;
                actor.LifePointsChanged += OnLifePointsChanged;
                actor.DamageReducted += OnDamageReducted;

                actor.SpellCasting += OnSpellCasting;
                actor.SpellCasted += OnSpellCasted;
                actor.SpellCastFailed += OnSpellCastFailed;
                actor.WeaponUsed += OnCloseCombat;

                actor.BuffAdded += OnBuffAdded;
                actor.BuffRemoved += OnBuffRemoved;

                actor.Dead += OnDead;
            }

            var fighter = actor as CharacterFighter;

            if (fighter != null)
            {
                fighter.Character.LoggedOut += OnPlayerLoggout;
            }
        }


        #endregion

        #region Turn Actions

        #region Death

        protected virtual void OnDead(FightActor fighter, FightActor killedBy)
        {
            StartSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);

            ActionsHandler.SendGameActionFightDeathMessage(Clients, fighter);

            fighter.KillAllSummons();
            fighter.RemoveAndDispellAllBuffs();
            fighter.RemoveAllCastedBuffs();

            EndSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);

            foreach (var trigger in m_triggers.ToArray().Where(trigger => trigger.Caster == fighter))
            {
                RemoveTrigger(trigger);
            }
        }

        #endregion

        #region Movement

        protected virtual void OnStartMoving(ContextActor actor, Path path)
        {
            var fighter = actor as FightActor;
            var character = actor is CharacterFighter ? (actor as CharacterFighter).Character : null;

            if (fighter != null && !fighter.IsFighterTurn())
                return;

            if (path.IsEmpty() || path.MPCost == 0)
                return;

            StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
            if (fighter != null && (fighter.GetTackledMP() > 0 || fighter.GetTackledAP() > 0))
            {
                // tackle
                OnTackled(fighter, path);

                if (path.IsEmpty() || path.MPCost == 0)
                {
                    EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                    return;
                }
            }
            var cells = path.GetPath();
            if (fighter != null)
            {
                var fighterCells = fighter.OpposedTeam.GetAllFighters(entry => entry.CanTackle(fighter)).Select(entry => entry.Cell.Id).ToList();
                var obstaclesCells = GetAllFighters(entry => entry != fighter && entry.Position.Cell != fighter.Cell && entry.IsAlive()).Select(entry => entry.Cell.Id).ToList();

                if (fighter.MP < path.MPCost)
                    path.CutPath(fighter.MP + 1);

                for (var i = 0; i < cells.Length; i++)
                {
                    // if there is a trap on the way we trigger it
                    // or if there is a fighter on a adjacent cell
                    if (i > 0 && ShouldTriggerOnMove(cells[i]))
                    {
                        path.CutPath(i + 1);
                        break;
                    }

                    // fighter adjacent to this cell, ignore first cell
                    // characters only can be tackled
                    if (i > 0 && fighter is CharacterFighter && 
                        fighter.VisibleState == GameActionFightInvisibilityStateEnum.VISIBLE &&
                        new MapPoint(cells[i]).GetAdjacentCells(entry => true).Any(entry => fighterCells.Contains(entry.CellId)))
                    {
                        path.CutPath(i + 1);
                        break;
                    }
                    if (!obstaclesCells.Contains(cells[i].Id))
                        continue;

                    if (character != null)
                    {
                        // "Impossible d'emprunter ce chemin : un obstacle bloque le passage !"
                        character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 276);
                    }

                    path.CutPath(i);
                    break;
                }
            }

            var movementsKeys = path.GetServerPathKeys();

            ForEach(entry =>
                        {
                            if (entry.CanSee(fighter))
                                ContextHandler.SendGameMapMovementMessage(entry.Client, movementsKeys, fighter);
                        }, true);

            actor.StopMove();
            EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
        }

        protected virtual void OnTackled(FightActor actor, Path path)
        {
            var tacklers = actor.GetTacklers();
            var mpTackled = actor.GetTackledMP();
            var apTackled = actor.GetTackledAP();

            if (actor.MP - mpTackled < 0)
            {
                logger.Error("Cannot apply tackle : mp tackled ({0}) > available mp ({1})", mpTackled, actor.MP);
                return;
            }

            ActionsHandler.SendGameActionFightTackledMessage(Clients, actor, tacklers);
            actor.LostAP((short)apTackled);
            actor.LostMP((short)mpTackled);

            if (path.MPCost > actor.MP)
                path.CutPath(actor.MP + 1);
        }

        protected virtual void OnStopMoving(ContextActor actor, Path path, bool canceled)
        {
            var fighter = actor as FightActor;

            if (fighter != null && !fighter.IsFighterTurn())
                return;

            if (canceled)
                return; // error, mouvement shouldn't be canceled in a fight.

            if (fighter == null)
                return;

            fighter.UseMP((short) path.MPCost);
            fighter.TriggerBuffs(BuffTriggerType.MOVE, path);
        }

        protected virtual void OnPositionChanged(ContextActor actor, ObjectPosition objectPosition)
        {
            var fighter = actor as FightActor;

            if (fighter != null)
                TriggerMarks(fighter.Cell, fighter, TriggerType.MOVE);
        }

        public void SwitchFighters(FightActor fighter1, FightActor fighter2)
        {
            // todo
        }

        #endregion

        #region Health & Actions points

        protected virtual void OnLifePointsChanged(FightActor actor, int delta, int permanentDamages, FightActor from)
        {
            var loss = (short) (-delta);
            if (delta == 0 && permanentDamages == 0)
                return;

            ActionsHandler.SendGameActionFightLifePointsLostMessage(Clients, from ?? actor, actor, loss, (short)permanentDamages);
        }

        protected virtual void OnFightPointsVariation(FightActor actor, ActionsEnum action, FightActor source, FightActor target, short delta)
        {
            if (delta == 0)
                return;

            ActionsHandler.SendGameActionFightPointsVariationMessage(Clients, action, source, target, delta);
        }

        protected virtual void OnDamageReducted(FightActor fighter, FightActor source, int reduction)
        {
            if (reduction == 0)
                return;

            ActionsHandler.SendGameActionFightReduceDamagesMessage(Clients, source, fighter, reduction);
        }

        #endregion

        #region Spells

        protected virtual void OnCloseCombat(FightActor caster, WeaponTemplate weapon, Cell targetCell, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            var target = GetOneFighter(targetCell);
            ForEach(entry => ActionsHandler.SendGameActionFightCloseCombatMessage(entry.Client, caster, target, targetCell, critical,
                !caster.IsVisibleFor(entry) || silentCast, weapon), true);
        }


        protected virtual void OnSpellCasting(FightActor caster, Spell spell, Cell targetCell, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            var target = GetOneFighter(targetCell);
            ForEach(entry => ContextHandler.SendGameActionFightSpellCastMessage(entry.Client, ActionsEnum.ACTION_FIGHT_CAST_SPELL,
                                                                                caster, target, targetCell, critical, !caster.IsVisibleFor(entry) || silentCast, spell), true);
        }

        protected virtual void OnSpellCasted(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            CheckFightEnd();
        }

        protected virtual void OnSpellCastFailed(FightActor caster, Spell spell, Cell target)
        {
            ContextHandler.SendGameActionFightNoSpellCastMessage(Clients, spell);
        }

        #endregion

        #region Buffs

        public IEnumerable<Buff> GetBuffs()
        {
            return m_buffs;
        }

        public void UpdateBuff(Buff buff)
        {
            ContextHandler.SendGameActionFightDispellableEffectMessage(Clients, buff, true);
        }

        protected virtual void OnBuffAdded(FightActor target, Buff buff)
        {
            m_buffs.Add(buff);
            ContextHandler.SendGameActionFightDispellableEffectMessage(Clients, buff);
        }

        protected virtual void OnBuffRemoved(FightActor target, Buff buff)
        {
            m_buffs.Remove(buff);

            ActionsHandler.SendGameActionFightDispellEffectMessage(Clients, target, target, buff);
        }

        #endregion

        #region Sequences

        private SequenceTypeEnum m_lastSequenceAction;
        private int m_sequenceLevel;
        private readonly Stack<SequenceTypeEnum> m_sequences = new Stack<SequenceTypeEnum>(); 

        public SequenceTypeEnum Sequence
        {
            get;
            private set;
        }

        public bool IsSequencing
        {
            get;
            private set;
        }

        public bool WaitAcknowledgment
        {
            get;
            private set;
        }

        public bool StartSequence(SequenceTypeEnum sequenceType)
        {
            // even if a sequence is already running we just increase the level variable and notify the last action
            m_lastSequenceAction = sequenceType;
            m_sequenceLevel++;

            if (IsSequencing)
                return false;

            IsSequencing = true;
            Sequence = sequenceType;
            m_sequences.Push(sequenceType);

            ActionsHandler.SendSequenceStartMessage(Clients, TimeLine.Current, sequenceType);

            return true;
        }


        public bool EndSequence(SequenceTypeEnum sequenceType, bool force = false)
        {
            if (!IsSequencing)
                return false;

            m_sequenceLevel--;

            if (m_sequenceLevel > 0 && !force)
                return false;

            IsSequencing = false;
            WaitAcknowledgment = true;

            var poppedSequence = m_sequences.Pop();

            if (poppedSequence != sequenceType)
            {
                logger.Debug("Popped Sequence different ({0} != {1})", poppedSequence, sequenceType);
            }

            ActionsHandler.SendSequenceEndMessage(Clients, TimeLine.Current, sequenceType, m_lastSequenceAction);

            return true;
        }

        public void EndAllSequences()
        {
            m_sequenceLevel = 0;
            IsSequencing = false;
            WaitAcknowledgment = false;

            while (m_sequences.Count > 0)
            {
                var poppedSequence = m_sequences.Pop();

                ActionsHandler.SendSequenceEndMessage(Clients, TimeLine.Current, poppedSequence, m_lastSequenceAction);
            }
        }

        public virtual void AcknowledgeAction()
        {
            WaitAcknowledgment = false;

            // todo : find the right usage
        }

        #endregion


        #endregion

        #region Non Turn Actions

        protected virtual void OnPlayerLeft(FightActor fighter)
        {
            if (State == FightState.Placement)
            {
                var characterFighter = ((CharacterFighter)fighter);

                if (characterFighter != null)
                    characterFighter.ResetFightProperties();

                if (CheckFightEnd())
                    return;

                fighter.Team.RemoveFighter(fighter);

                if (characterFighter == null)
                    return;

                characterFighter.Character.RejoinMap();
            }
            else
            {
                fighter.Die();

                if (fighter is CharacterFighter && (fighter as CharacterFighter).Character.IsLoggedIn)
                {
                    // wait the character to be ready
                    var readyChecker = new ReadyChecker(this, new[] { ( (CharacterFighter)fighter ) });
                    readyChecker.Success += obj => OnPlayerReadyToLeave(fighter as CharacterFighter);
                    readyChecker.Timeout += (obj, laggers) => OnPlayerReadyToLeave(fighter as CharacterFighter);

                    ((CharacterFighter)fighter).PersonalReadyChecker = readyChecker;
                    // Clients.Remove(character.Client); // can be instant so we remove him before to start the checker .. why ???
                    readyChecker.Start();
                }
                else
                {
                    var isfighterTurn = fighter.IsFighterTurn();

                    ContextHandler.SendGameFightLeaveMessage(Clients, fighter);

                    if (!CheckFightEnd() && isfighterTurn)
                        StopTurn();

                    fighter.ResetFightProperties();

                    fighter.Team.RemoveFighter(fighter);
                    fighter.Team.AddLeaver(fighter);
                    m_leavers.Add(fighter);
                } 
            }
        }

        protected virtual void OnPlayerReadyToLeave(CharacterFighter fighter)
        {
            fighter.PersonalReadyChecker = null;
            var isfighterTurn = fighter.IsFighterTurn();

            IFightResult leaverResult;
            var results = GenerateLeaverResults(fighter, out leaverResult);

            leaverResult.Apply();

            ContextHandler.SendGameFightLeaveMessage(Clients, fighter);
            ContextHandler.SendGameFightEndMessage(fighter.Character.Client, this,
                results.Select(x => x.GetFightResultListEntry()));

            var fightend = CheckFightEnd();

            if (!fightend && isfighterTurn)
                StopTurn();   

            fighter.ResetFightProperties();
            fighter.Character.RejoinMap();

            fighter.Team.RemoveFighter(fighter);
            fighter.Team.AddLeaver(fighter);
            m_leavers.Add(fighter);
        }

        protected virtual void OnPlayerLoggout(Character character)
        {
            if (!character.IsFighting() || character.Fight != this)
                return;

            character.Fighter.LeaveFight();
        }

        #endregion

        #region Triggers

        private readonly List<MarkTrigger> m_triggers = new List<MarkTrigger>();

        public IEnumerable<MarkTrigger> GetTriggers()
        {
            return m_triggers;
        }

        public bool ShouldTriggerOnMove(Cell cell)
        {
            return m_triggers.Any(entry => entry.TriggerType.HasFlag(TriggerType.MOVE) && entry.ContainsCell(cell));
        }

        public bool ShouldTriggerOnMove(Cell cell, FightActor actor)
        {
            return m_triggers.Any(entry => entry.TriggerType.HasFlag(TriggerType.MOVE) && entry.ContainsCell(cell) && entry.IsAffected(actor));
        }

        public MarkTrigger[] GetTriggers(Cell cell)
        {
            return m_triggers.Where(entry => entry.CenterCell.Id == cell.Id).ToArray();
        }

        public void AddTriger(MarkTrigger trigger)
        {
            trigger.Triggered += OnMarkTriggered;
            m_triggers.Add(trigger);

            foreach (var fighter in GetAllFighters<CharacterFighter>())
            {
                ContextHandler.SendGameActionFightMarkCellsMessage(fighter.Character.Client, trigger, trigger.DoesSeeTrigger(fighter));
            }

            if (!trigger.TriggerType.HasFlag(TriggerType.CREATION))
                return;

            var fighters = GetAllFighters(trigger.GetCells());
            foreach (var fighter in fighters)
                trigger.Trigger(fighter);
        }

        public void RemoveTrigger(MarkTrigger trigger)
        {
            trigger.Triggered -= OnMarkTriggered;
            m_triggers.Remove(trigger);
            trigger.NotifyRemoved();

            ContextHandler.SendGameActionFightUnmarkCellsMessage(Clients, trigger);
        }

        public void TriggerMarks(Cell cell, FightActor trigger, TriggerType triggerType)
        {
            var triggers = m_triggers.ToArray();

            // we use a copy 'cause a trigger can be deleted when a fighter die with it
            foreach (var markTrigger in triggers.Where(markTrigger => markTrigger.TriggerType.HasFlag(triggerType) && markTrigger.ContainsCell(cell)))
            {
                StartSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP);

                // avoid the trigger to trigger twice
                if (markTrigger is Trap)
                    RemoveTrigger(markTrigger); 
                    
                markTrigger.Trigger(trigger);

                EndSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP);
            }
        }

        public void DecrementGlyphDuration(FightActor caster)
        {
            var triggersToRemove = m_triggers.Where(trigger => trigger.Caster == caster).
                Where(trigger => trigger.DecrementDuration()).ToList();

            if (triggersToRemove.Count == 0)
                return;

            StartSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP);
            foreach (var trigger in triggersToRemove)
            {
                RemoveTrigger(trigger);
            }
            EndSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP);
        }

        public int PopNextTriggerId()
        {
            return m_triggerIdProvider.Pop();
        }

        public void FreeTriggerId(int id)
        {
            m_triggerIdProvider.Push(id);
        }

        private void OnMarkTriggered(MarkTrigger markTrigger, FightActor trigger, Spell triggerSpell)
        {
            ContextHandler.SendGameActionFightTriggerGlyphTrapMessage(Clients, markTrigger, trigger, triggerSpell);
        }

        #endregion

        #region Ready Checker

        protected virtual void OnLaggersSpotted(NamedFighter[] laggers)
        {
            if (laggers.Length == 1)
            {
                BasicHandler.SendTextInformationMessage(Clients, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 28, laggers[0].Name);
            }
            else if (laggers.Length > 1)
            {
                BasicHandler.SendTextInformationMessage(Clients, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 29, string.Join(",", laggers.Select(entry => entry.Name)));
            }
        }

        #endregion

        #region Send Methods

        protected virtual void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            ContextHandler.SendGameFightJoinMessage(fighter.Character.Client, CanCancelFight(), !IsStarted, false, IsStarted, GetPlacementTimeLeft(), FightType);
        }

        protected virtual void SendGameFightJoinMessage(FightSpectator spectator)
        {
            ContextHandler.SendGameFightJoinMessage(spectator.Character.Client, false, false, false, IsStarted, GetPlacementTimeLeft(), FightType);
        }

        #endregion

        #region Get Methods

        private readonly WorldClientCollection m_clients = new WorldClientCollection();
        private readonly WorldClientCollection m_spectatorClients = new WorldClientCollection();
        private readonly List<FightActor> m_leavers;
        private readonly List<FightSpectator> m_spectators;

        /// <summary>
        /// Do not modify, just read
        /// </summary>
        public WorldClientCollection Clients
        {
            get { return m_clients; }
        }

        /// <summary>
        /// Do not modify, just read
        /// </summary>
        public WorldClientCollection SpectatorClients
        {
            get
            {
                return m_spectatorClients;
            }
        }

        public IEnumerable<Character> GetAllCharacters()
        {
            return GetAllCharacters(false);
        }

        public IEnumerable<Character> GetAllCharacters(bool withSpectators = false)
        {
            return withSpectators ? Fighters.OfType<CharacterFighter>().Select(entry => entry.Character).Concat(Spectators.Select(entry => entry.Character)) : Fighters.OfType<CharacterFighter>().Select(entry => entry.Character);
        }

        public void ForEach(Action<Character> action)
        {
            foreach (var character in GetAllCharacters())
            {
                action(character);
            }
        }

        public void ForEach(Action<Character> action, bool withSpectators = false)
        {
            foreach (var character in GetAllCharacters(withSpectators))
            {
                action(character);
            }
        }

        public void ForEach(Action<Character> action, Character except, bool withSpectators = false)
        {
            foreach (var character in GetAllCharacters(withSpectators).Where(character => character != except))
            {
                action(character);
            }
        }

        protected abstract bool CanCancelFight();

        public bool IsCellFree(Cell cell)
        {
            return cell.Walkable && !cell.NonWalkableDuringFight && GetOneFighter(cell) == null;
        }

        public int GetFightDuration()
        {
            return !IsStarted ? 0 : (int) (DateTime.Now - StartTime).TotalMilliseconds;
        }

        public int GetTurnTimeLeft()
        {
            if (TimeLine.Current == null)
                return 0;

            var time = ( DateTime.Now - TurnStartTime ).TotalMilliseconds;

            return time > 0 ? (FightConfiguration.TurnTime - (int)time) : 0;
        }

        public sbyte GetNextContextualId()
        {
            return (sbyte) m_contextualIdProvider.Pop();
        }

        public void FreeContextualId(sbyte id)
        {
            m_contextualIdProvider.Push(id);
        }

        public FightActor GetOneFighter(int id)
        {
            return Fighters.FirstOrDefault(entry => entry.Id == id);
        }

        public FightActor GetOneFighter(Cell cell)
        {
            return Fighters.FirstOrDefault(entry => entry.IsAlive() && entry.Cell.Id == cell.Id);
        }

        public FightActor GetOneFighter(Predicate<FightActor> predicate)
        {
            var entries = Fighters.Where(entry => predicate(entry));

            var fightActors = entries as FightActor[] ?? entries.ToArray();
            return fightActors.Count() != 0 ? null : fightActors.FirstOrDefault();
        }

        public T GetOneFighter<T>(int id) where T : FightActor
        {
            return Fighters.OfType<T>().FirstOrDefault(entry => entry.Id == id);
        }

        public T GetOneFighter<T>(Cell cell) where T : FightActor
        {
            return Fighters.OfType<T>().FirstOrDefault(entry => entry.IsAlive() && Equals(entry.Position.Cell, cell));
        }

        public T GetOneFighter<T>(Predicate<T> predicate) where T : FightActor
        {
            return Fighters.OfType<T>().FirstOrDefault(entry => predicate(entry));
        }

        public T GetFirstFighter<T>(int id) where T : FightActor
        {
            return Fighters.OfType<T>().FirstOrDefault(entry => entry.Id == id);
        }

        public T GetFirstFighter<T>(Cell cell) where T : FightActor
        {
            return Fighters.OfType<T>().FirstOrDefault(entry => entry.IsAlive() && Equals(entry.Position.Cell, cell));
        }

        public T GetFirstFighter<T>(Predicate<T> predicate) where T : FightActor
        {
            return Fighters.OfType<T>().FirstOrDefault(entry => predicate(entry));
        }

        public ReadOnlyCollection<FightActor> GetAllFighters()
        {
            return Fighters;
        }

        public ReadOnlyCollection<FightActor> GetLeavers()
        {
            return Leavers;
        }

        public CharacterFighter GetLeaver(int characterId)
        {
            return Leavers.OfType<CharacterFighter>().FirstOrDefault(x => x.Id == characterId);
        }

        public ReadOnlyCollection<FightSpectator> GetSpectators()
        {
            return Spectators;
        }

        public IEnumerable<Character> GetCharactersAndSpectators()
        {
            return GetAllCharacters().Concat(GetSpectators().Select(entry => entry.Character));
        }

        public IEnumerable<FightActor> GetFightersAndLeavers()
        {
            return Fighters.Concat(Leavers);
        }

        public IEnumerable<FightActor> GetAllFighters(Cell[] cells)
        {
            return GetAllFighters<FightActor>(entry => entry.IsAlive() && cells.Contains(entry.Position.Cell));
        }

        public IEnumerable<FightActor> GetAllFighters(IEnumerable<Cell> cells)
        {
            return GetAllFighters(cells.ToArray());
        }

        public IEnumerable<FightActor> GetAllFighters(Predicate<FightActor> predicate)
        {
            return Fighters.Where(entry => predicate(entry));
        }

        public IEnumerable<T> GetAllFighters<T>() where T : FightActor
        {
            return Fighters.OfType<T>();
        }

        public IEnumerable<T> GetAllFighters<T>(Predicate<T> predicate) where T : FightActor
        {
            return Fighters.OfType<T>().Where(entry => predicate(entry));
        }

        public IEnumerable<int> GetDeadFightersIds()
        {
            return GetFightersAndLeavers().Where(entry => entry.IsDead() && entry.IsVisibleInTimeline).Select(entry => entry.Id);
        }

        public IEnumerable<int> GetAliveFightersIds()
        {
            return GetAllFighters<FightActor>(entry => entry.IsAlive() && entry.IsVisibleInTimeline).Select(entry => entry.Id);
        }

        public FightCommonInformations GetFightCommonInformations()
        {
            return new FightCommonInformations(Id,
                                               (sbyte) FightType,
                                               m_teams.Select(entry => entry.GetFightTeamInformations()),
                                               m_teams.Select(entry => entry.BladePosition.Cell.Id),
                                               m_teams.Select(entry => entry.GetFightOptionsInformations()));
        }

        public FightExternalInformations GetFightExternalInformations()
        {
            return new FightExternalInformations(Id, StartTime.GetUnixTimeStamp(), SpectatorClosed || State != FightState.Fighting, m_teams.Select(entry => entry.GetFightTeamLightInformations()), m_teams.Select(entry => entry.GetFightOptionsInformations()));
        }


        #endregion
    }
}