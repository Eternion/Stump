using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.Core.Pool;
using Stump.Core.Timers;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Worlds.Actors;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Fights.Buffs;
using Stump.Server.WorldServer.Worlds.Fights.Results;
using Stump.Server.WorldServer.Worlds.Fights.Triggers;
using Stump.Server.WorldServer.Worlds.Items;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Maps.Pathfinding;
using Stump.Server.WorldServer.Worlds.Spells;
using TriggerType = Stump.Server.WorldServer.Worlds.Fights.Triggers.TriggerType;

namespace Stump.Server.WorldServer.Worlds.Fights
{
    public class Fight : ICharacterContainer, IDisposable
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        #region Config

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

        #endregion

        #region Events

        #region Delegates

        public delegate void FightEndedDelegate(Fight fight);

        #endregion

        public Action<Fight, FightActor, Cell> CellShown;

        public event Action<Fight> FightStarted;

        private void NotifyFightStarted()
        {
            Action<Fight> handler = FightStarted;
            if (handler != null)
                handler(this);
        }

        public event Action<Fight, FightState> StateChanged;

        private void NotifyStateChanged()
        {
            OnStateChanged();

            Action<Fight, FightState> handler = StateChanged;
            if (handler != null)
                handler(this, State);
        }

        private void OnStateChanged()
        {
            UnBindFightersEvents();
            BindFightersEvents();

            if (State != FightState.Placement)
                HideBlades();
        }

        private void NotifyCellShown(FightActor fighter, Cell cell)
        {
            Action<Fight, FightActor, Cell> handler = CellShown;
            if (handler != null)
                CellShown(this, fighter, cell);
        }

        public event Action<Fight> RequestTurnReady;

        private void NotifyRequestTurnReady()
        {
            // if there is no player more there is nothing to check
            if (GetAllCharacters().Count() == 0)
                CheckTurnReady();

            Action<Fight> handler = RequestTurnReady;
            if (handler != null)
                handler(this);
        }

        public event FightEndedDelegate FightEnded;

        private void NotifyFightEnded()
        {
            FightEndedDelegate handler = FightEnded;
            if (handler != null)
                handler(this);
        }

        public event FightEndedDelegate FightCanceled;

        private void NotifyFightCanceled()
        {
            FightEndedDelegate handler = FightCanceled;
            if (handler != null)
                handler(this);
        }

        #endregion

        #region Constructor

        public Fight(int id, FightTypeEnum fightType, Map fightMap, FightTeam blueTeam, FightTeam redTeam)
        {
            Id = id;
            FightType = fightType;
            Map = fightMap;
            BlueTeam = blueTeam;
            BlueTeam.Fight = this;
            RedTeam = redTeam;
            RedTeam.Fight = this;
            m_teams = new[] {RedTeam, BlueTeam};

            TimeLine = new TimeLine(this, m_fighters);
            TimeLine.TurnStarted += OnTurnStarted;
            TimeLine.TurnEndRequest += OnTurnEndRequest;
            TimeLine.TurnEnded += OnTurnEnded;

            BlueTeam.FighterAdded += OnFighterAdded;
            BlueTeam.FighterRemoved += OnFighterRemoved;
            RedTeam.FighterAdded += OnFighterAdded;
            RedTeam.FighterRemoved += OnFighterRemoved;

            CreationTime = DateTime.Now;
        }

        #endregion

        #region Properties

        private readonly ReversedUniqueIdProvider m_contextualIdProvider = new ReversedUniqueIdProvider(0);
        private readonly ConcurrentList<FightActor> m_fighters = new ConcurrentList<FightActor>();
        private readonly FightTeam[] m_teams;
        private readonly UniqueIdProvider m_triggerIdProvider = new UniqueIdProvider();
        private TimerEntry m_endFightTimer;
        private TimerEntry m_placementTimer;

        public int Id
        {
            get;
            private set;
        }

        public short AgeBonus
        {
            get;
            private set;
        }

        public Map Map
        {
            get;
            private set;
        }

        public FightTypeEnum FightType
        {
            get;
            private set;
        }

        public FightTeam BlueTeam
        {
            get;
            private set;
        }

        public FightTeam RedTeam
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

        public TimeLine TimeLine
        {
            get;
            private set;
        }

        public AcknowledgmentChecker AcknowledgmentChecker
        {
            get;
            private set;
        }

        public FightState State
        {
            get;
            private set;
        }

        public bool BladesVisible
        {
            get;
            private set;
        }

        public SequenceTypeEnum Sequence
        {
            get;
            private set;
        }

        public FightSequenceAction LastSequenceAction
        {
            get;
            set;
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

        public int SequenceLevel
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        private readonly WorldClientCollection m_clients = new WorldClientCollection();

        /// <summary>
        /// Do not modify, just read
        /// </summary>
        public WorldClientCollection Clients
        {
            get { return m_clients; }
        }

        public IEnumerable<Character> GetAllCharacters()
        {
            return m_fighters.OfType<CharacterFighter>().Select(entry => entry.Character);
        }

        public void ForEach(Action<Character> action)
        {
            foreach (Character character in GetAllCharacters())
            {
                action(character);
            }
        }

        public void ForEach(Action<Character> action, Character expect)
        {
            foreach (Character character in GetAllCharacters())
            {
                if (character == expect)
                    continue;

                action(character);
            }
        }

        #region Events Binders

        private void UnBindFightersEvents()
        {
            foreach (FightActor fighter in m_fighters)
            {
                UnBindFighterEvents(fighter);
            }
        }

        private void UnBindFighterEvents(FightActor actor)
        {
            actor.CellShown -= OnCellShown;

            actor.ReadyStateChanged -= OnSetReady;
            actor.PrePlacementChanged -= OnChangePreplacementPosition;

            actor.StartMoving -= OnStartMoving;
            actor.StopMoving -= OnStopMoving;
            actor.PositionChanged -= OnPositionChanged;
            actor.FightPointsVariation -= OnFightPointsVariation;
            actor.LifePointsChanged -= OnLifePointsChanged;
            actor.DamageReducted -= OnDamageReducted;
            actor.SpellCasting -= OnSpellCasting;
            actor.SpellCasted -= OnSpellCasted;
            actor.BuffAdded -= OnBuffAdded;
            actor.BuffRemoved -= OnBuffRemoved;
            actor.TurnReadyStateChanged -= OnSetTurnReady;
            actor.Dead -= OnDead;


            var fighter = actor as CharacterFighter;

            if (fighter != null)
            {
                fighter.Character.LoggedOut -= OnLoggedOut;
            }
        }

        private void BindFightersEvents()
        {
            foreach (FightActor fighter in m_fighters)
            {
                BindFighterEvents(fighter);
            }
        }

        private void BindFighterEvents(FightActor actor)
        {
            actor.CellShown += OnCellShown;

            if (State == FightState.Placement)
            {
                actor.ReadyStateChanged += OnSetReady;
                actor.PrePlacementChanged += OnChangePreplacementPosition;
            }

            if (State == FightState.Fighting)
            {
                actor.StartMoving += OnStartMoving;
                actor.StopMoving += OnStopMoving;
                actor.PositionChanged += OnPositionChanged;
                actor.FightPointsVariation += OnFightPointsVariation;
                actor.LifePointsChanged += OnLifePointsChanged;
                actor.DamageReducted += OnDamageReducted;

                actor.SpellCasting += OnSpellCasting;
                actor.SpellCasted += OnSpellCasted;

                actor.BuffAdded += OnBuffAdded;
                actor.BuffRemoved += OnBuffRemoved;

                actor.TurnReadyStateChanged += OnSetTurnReady;
                actor.Dead += OnDead;
            }

            if (State == FightState.Ended)
            {
                actor.TurnReadyStateChanged += OnSetTurnReady;
            }

            var fighter = actor as CharacterFighter;

            if (fighter != null)
            {
                fighter.Character.LoggedOut += OnLoggedOut;
            }
        }

        #endregion

        #region Placement phase

        public void StartPlacementPhase()
        {
            if (State != FightState.NotStarted)
                return;

            SetFightState(FightState.Placement);

            RandomnizePositions(RedTeam);
            RandomnizePositions(BlueTeam);

            ShowBlades();
            Map.AddFight(this);

            if (FightType != FightTypeEnum.FIGHT_TYPE_CHALLENGE)
                m_placementTimer = Map.Area.CallDelayed(PlacementPhaseTime, EndPlacementPhase);
        }

        private void EndPlacementPhase()
        {
            m_placementTimer.Dispose();

            StartFight();
        }

        private void FindBladesPlacement()
        {
            if (RedTeam.Leader.MapPosition.Cell.Id != BlueTeam.Leader.MapPosition.Cell.Id)
            {
                RedTeam.BladePosition = RedTeam.Leader.MapPosition.Clone();
                BlueTeam.BladePosition = BlueTeam.Leader.MapPosition.Clone();
            }
            else
            {
                var cell = Map.GetRandomAdjacentFreeCell(RedTeam.Leader.MapPosition.Point);

                // if cell not found we superpose both blades
                if (cell.Equals(Cell.Null))
                {
                    RedTeam.BladePosition = RedTeam.Leader.MapPosition.Clone();
                }
                else // else we take an adjacent cell
                {
                    var pos = RedTeam.Leader.MapPosition.Clone();
                    pos.Cell = cell;
                    RedTeam.BladePosition = pos;
                }

                BlueTeam.BladePosition = BlueTeam.Leader.MapPosition.Clone();
            }
        }

        public void ShowBlades()
        {
            if (BladesVisible)
                return;

            if (RedTeam.BladePosition == null ||
                BlueTeam.BladePosition == null)
                FindBladesPlacement();

            ContextHandler.SendGameRolePlayShowChallengeMessage(Map.Clients, this);

            RedTeam.TeamOptionsChanged += OnTeamOptionsChanged;
            BlueTeam.TeamOptionsChanged += OnTeamOptionsChanged;

            BladesVisible = true;
        }

        public void HideBlades()
        {
            if (!BladesVisible)
                return;

            ContextHandler.SendGameRolePlayRemoveChallengeMessage(Map.Clients, this);

            RedTeam.TeamOptionsChanged -= OnTeamOptionsChanged;
            BlueTeam.TeamOptionsChanged -= OnTeamOptionsChanged;

            BladesVisible = false;
        }

        private void OnTeamOptionsChanged(FightTeam team, FightOptionsEnum option)
        {
            ContextHandler.SendGameFightOptionStateUpdateMessage(Clients, team, option, team.GetOptionState(option));
            ContextHandler.SendGameFightOptionStateUpdateMessage(Map.Clients, team, option, team.GetOptionState(option));
        }

        public bool FindRandomFreeCell(FightActor fighter, out Cell cell, bool placement = true)
        {
            Cell[] availableCells = fighter.Team.PlacementCells.Where(entry => GetOneFighter(entry) == null || GetOneFighter(entry) == fighter).ToArray();

            var random = new Random();

            if (availableCells.Length == 0 && placement)
            {
                cell = default(Cell);
                return false;
            }

            // if not in placement phase, get a random free cell on the map
            if (availableCells.Length == 0 && !placement)
            {
                List<int> cells = Enumerable.Range(0, (int) MapPoint.MapSize).ToList();
                foreach (FightActor actor in GetAllFighters())
                {
                    if (cells.Contains(actor.Cell.Id))
                        cells.Remove(actor.Cell.Id);
                }

                cell = Map.Cells[cells[random.Next(cells.Count)]];

                return true;
            }

            cell = availableCells[random.Next(availableCells.Length)];

            return true;
        }

        public void RandomnizePosition(FightActor fighter)
        {
            Cell cell;
            if (!FindRandomFreeCell(fighter, out cell))
                LeaveFight(fighter);
            else
                fighter.ChangePrePlacement(cell);
        }

        public void RandomnizePositions(FightTeam team)
        {
            IEnumerable<Cell> shuffledCells = team.PlacementCells.Shuffle();
            IEnumerator<Cell> enumerator = shuffledCells.GetEnumerator();
            foreach (FightActor fighter in team.GetAllFighters())
            {
                enumerator.MoveNext();

                fighter.ChangePrePlacement(enumerator.Current);
            }
            enumerator.Dispose();
        }

        public DirectionsEnum FindPlacementDirection(FightActor fighter)
        {
            FightTeam team = fighter.Team == RedTeam ? BlueTeam : RedTeam;

            Tuple<Cell, uint> closerCell = null;
            foreach (Cell cell in team.PlacementCells)
            {
                var point = new MapPoint(cell);

                if (closerCell == null)
                    closerCell = Tuple.Create(cell,
                                              fighter.Position.Point.DistanceToCell(point));
                else
                {
                    if (fighter.Position.Point.DistanceToCell(point) < closerCell.Item2)
                        closerCell = Tuple.Create(cell,
                                                  fighter.Position.Point.DistanceToCell(point));
                }
            }

            if (closerCell == null)
                return fighter.Position.Direction;

            return fighter.Position.Point.OrientationTo(new MapPoint(closerCell.Item1), false);
        }

        /// <summary>
        ///   Set the ready state of a character
        /// </summary>
        private void OnSetReady(FightActor fighter, bool isReady)
        {
            if (State != FightState.Placement)
                return;

            ContextHandler.SendGameFightHumanReadyStateMessage(Clients, fighter);

            if (RedTeam.AreAllReady() && BlueTeam.AreAllReady())
                StartFight();
        }


        /// <summary>
        ///   Check if a character can change position (before the fight is started).
        /// </summary>
        /// <param name = "fighter"></param>
        /// <param name="cell"></param>
        /// <returns>If change is possible</returns>
        public bool CanChangePosition(FightActor fighter, Cell cell)
        {
            FightActor figtherOnCell = GetOneFighter(cell);

            return State == FightState.Placement &&
                   fighter.Team.PlacementCells.Contains(cell) &&
                   (figtherOnCell == fighter || figtherOnCell == null);
        }

        private void OnChangePreplacementPosition(FightActor fighter, ObjectPosition objectPosition)
        {
            UpdateFightersPlacementDirection();

            ContextHandler.SendGameEntitiesDispositionMessage(Clients, GetAllFighters());
        }

        private void UpdateFightersPlacementDirection()
        {
            foreach (FightActor fighter in m_fighters)
            {
                fighter.Position.Direction = FindPlacementDirection(fighter);
            }
        }

        private void OnCellShown(FightActor fighter, Cell cell)
        {
            ContextHandler.SendShowCellMessage(Clients, fighter, cell);
        }

        public void KickPlayer(CharacterFighter fighter)
        {
            LeaveFight(fighter);
        }

        public void LeaveFight(FightActor fighter)
        {
            Contract.Requires(fighter != null);
            Contract.Assert(m_fighters.Contains(fighter));

            if (FightType != FightTypeEnum.FIGHT_TYPE_CHALLENGE)
                fighter.Die();

            ContextHandler.SendGameFightLeaveMessage(Clients, fighter);

            Contract.Assert(fighter.Team != null);
            fighter.Team.RemoveFighter(fighter);

            if (fighter is CharacterFighter)
            {
                Character character = (fighter as CharacterFighter).Character;

                // can be logged out
                if (character.IsInWorld)
                {
                    ContextHandler.SendGameFightEndMessage(character.Client, this);
                    character.RejoinMap();
                }
            }

            if (fighter.IsFighterTurn())
                RequestTurnEnd();

            CheckFightEnd();
        }

        #endregion

        #region Fighting phase

        public void StartFight()
        {
            if (m_placementTimer != null)
                m_placementTimer.Dispose();

            RedTeam.SetAllTurnReady(false);
            BlueTeam.SetAllTurnReady(false);
            HideBlades();

            SetFightState(FightState.Fighting);

            TimeLine.ReorganizeTimeLine();


            ContextHandler.SendGameEntitiesDispositionMessage(Clients, GetAllFighters());

            ContextHandler.SendGameFightStartMessage(Clients);
            ContextHandler.SendGameFightTurnListMessage(Clients, this);

            ContextHandler.SendGameFightSynchronizeMessage(Clients, this);

            ForEach(character => CharacterHandler.SendCharacterStatsListMessage(character.Client));

            TimeLine.Start();
        }

        private void OnSetTurnReady(FightActor fighter, bool isReady)
        {
            CheckTurnReady();
        }

        private void CheckTurnReady()
        {
            if (!RedTeam.AreAllTurnReady() || !BlueTeam.AreAllTurnReady())
                return;

            if (State == FightState.Ended)
                EndFight();
            else if (State == FightState.Fighting)
                ConfirmTurnEnd();
        }

        private void OnDead(FightActor fighter, FightActor killedBy)
        {
            StartSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);

            ActionsHandler.SendGameActionFightDeathMessage(Clients, fighter);

            EndSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);
        }

        private void OnDamageReducted(FightActor fighter, FightActor source, int reduction)
        {
            ActionsHandler.SendGameActionFightReduceDamagesMessage(Clients, source, fighter, reduction);
        }

        private void OnStartMoving(ContextActor actor, Path path)
        {
            var fighter = actor as FightActor;

            if (!fighter.IsFighterTurn())
                return;

            IEnumerable<short> movementsKeys = path.GetServerPathKeys();

            StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
            ContextHandler.SendGameMapMovementMessage(Clients, movementsKeys, fighter);
            actor.StopMove();
            EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
        }

        private void OnStopMoving(ContextActor actor, Path path, bool canceled)
        {
            var fighter = actor as FightActor;

            if (!fighter.IsFighterTurn())
                return;

            if (canceled)
                return; // error, mouvement shouldn't be canceled in a fight.

            fighter.UseMP((short) path.MPCost);
            fighter.TriggerBuffs(Buffs.TriggerType.MOVE);
        }

        private void OnPositionChanged(ContextActor actor, ObjectPosition objectPosition)
        {
            var fighter = actor as FightActor;

            TriggerMarks(fighter.Cell, fighter, TriggerType.MOVE);
        }

        private void OnFightPointsVariation(FightActor actor, ActionsEnum action, FightActor source, FightActor target, short delta)
        {
            ActionsHandler.SendGameActionFightPointsVariationMessage(Clients, action, source, target, delta);
        }

        private void OnLifePointsChanged(FightActor actor, int delta, FightActor from)
        {
            // todo : not managed
            short permanentDamages = 0;
            var loss = (short) (-delta);

            ActionsHandler.SendGameActionFightLifePointsLostMessage(Clients, from ?? actor, actor, loss, permanentDamages);
        }

        private void OnSpellCasting(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            ContextHandler.SendGameActionFightSpellCastMessage(Clients, ActionsEnum.ACTION_FIGHT_CAST_SPELL,
                                                               caster, target, critical, silentCast, spell);

            if (critical == FightSpellCastCriticalEnum.CRITICAL_FAIL)
                EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);
        }

        private void OnSpellCasted(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            CheckFightEnd();
        }

        private void OnBuffRemoved(FightActor target, Buff buff)
        {
        }

        private void OnBuffAdded(FightActor target, Buff buff)
        {
            ForEach(entry => ContextHandler.SendGameActionFightDispellableEffectMessage(entry.Client, buff));
        }


        // todo : who is damn lagging ?
        private void OnCheckerTimeout(AcknowledgmentChecker checker, FightSequenceAction action, CharacterFighter[] laggers)
        {
            // todo : define as lagger

            CheckFightEnd();
        }

        private void OnCheckerSuccess(AcknowledgmentChecker checker, FightSequenceAction action)
        {
            CheckFightEnd();
        }

        public void SetFightState(FightState state)
        {
            if (State == state)
                return;

            State = state;

            NotifyStateChanged();
        }

        private void OnFighterAdded(FightTeam team, FightActor fighter)
        {
            m_fighters.Add(fighter);

            BindFighterEvents(fighter);

            if (State == FightState.Placement || State == FightState.NotStarted)
            {
                if (State == FightState.Placement)
                    RandomnizePosition(fighter);

                if (fighter is CharacterFighter)
                {
                    Character character = ((CharacterFighter) fighter).Character;

                    Clients.Add(character.Client);

                    ContextHandler.SendGameFightJoinMessage(character.Client,
                                                            fighter.IsTeamLeader() && FightType == FightTypeEnum.FIGHT_TYPE_CHALLENGE,
                                                            true,
                                                            false,
                                                            false,
                                                            GetPlacementPhaseTimeLeft(),
                                                            team.Fight.FightType);

                    ContextHandler.SendGameFightPlacementPossiblePositionsMessage(character.Client, this, fighter.Team.Id);

                    foreach (FightActor fightMember in GetAllFighters())
                        ContextHandler.SendGameFightShowFighterMessage(character.Client, fightMember);

                    ContextHandler.SendGameEntitiesDispositionMessage(character.Client, GetAllFighters());

                    ContextHandler.SendGameFightUpdateTeamMessage(character.Client, this, RedTeam);
                    ContextHandler.SendGameFightUpdateTeamMessage(character.Client, this, BlueTeam);
                }
            }

            ContextHandler.SendGameFightShowFighterMessage(Clients, fighter);

            if (BladesVisible)
                Map.ForEach(entry => ContextHandler.SendGameFightUpdateTeamMessage(entry.Client, this, team));
        }

        private void OnFighterRemoved(FightTeam team, FightActor fighter)
        {
            m_fighters.Remove(fighter);

            if (fighter is CharacterFighter)
                Clients.Remove(((CharacterFighter) fighter).Character.Client);

            UnBindFighterEvents(fighter);

            if (State == FightState.Placement)
            {
                ContextHandler.SendGameFightRemoveTeamMemberMessage(Clients, fighter);

                if (BladesVisible)
                    ContextHandler.SendGameFightRemoveTeamMemberMessage(Map.Clients, fighter);
            }
            else if (State == FightState.Fighting)
            {
                ContextHandler.SendGameContextRemoveElementMessage(Clients, fighter);
            }
        }

        private void OnLoggedOut(Character character)
        {
            LeaveFight(character.Fighter);
        }

        public bool CheckFightEnd()
        {
            if (!RedTeam.AreAllDead() && !BlueTeam.AreAllDead() || State == FightState.Ended)
                return false;

            if (State == FightState.Fighting)
                RequestEndFight();
            else
                EndFight();

            return true;
        }

        #region Timeline Management

        private void OnTurnStarted(TimeLine sender, FightActor currentfighter)
        {
            DecrementGlyphDuration(currentfighter);
            currentfighter.DecrementAllCastedBuffsDuration();
            currentfighter.TriggerBuffs(Buffs.TriggerType.TURN_BEGIN);
            TriggerMarks(currentfighter.Cell, currentfighter, TriggerType.TURN_BEGIN);

            ForEach(entry =>
                        {
                            ContextHandler.SendGameFightSynchronizeMessage(entry.Client, this);
                            CharacterHandler.SendCharacterStatsListMessage(entry.Client);

                            if (TimeLine.Index == 0)
                                ContextHandler.SendGameFightNewRoundMessage(entry.Client, TimeLine.RoundNumber);

                            ContextHandler.SendGameFightTurnStartMessage(entry.Client, currentfighter.Id,
                                                                         TurnTime);
                        });
        }

        private void OnTurnEndRequest(TimeLine sender, FightActor currentFighter)
        {
            NotifyRequestTurnReady();

            ContextHandler.SendGameFightTurnEndMessage(Clients, currentFighter);
            ContextHandler.SendGameFightTurnReadyRequestMessage(Clients, currentFighter);
        }

        private void OnTurnEnded(TimeLine sender, FightActor currentFighter)
        {
            if (IsSequencing)
                EndSequence(Sequence);

            if (WaitAcknowledgment)
                AcknowledgeAction();

            currentFighter.TriggerBuffs(Buffs.TriggerType.TURN_END);

            RedTeam.SetAllTurnReady(false);
            BlueTeam.SetAllTurnReady(false);

            currentFighter.ResetPoints();

            CheckFightEnd();
        }

        public void RequestTurnEnd(FightActor fighter)
        {
            if (fighter != TimeLine.Current)
                return;

            RequestTurnEnd();
        }

        public void RequestTurnEnd()
        {
            TimeLine.RequestTurnEnd();
        }

        public void ConfirmTurnEnd()
        {
            TimeLine.ConfirmTurnEnd();
        }

        #endregion

        #region Sequences

        public bool StartSequence(SequenceTypeEnum sequenceType)
        {
            LastSequenceAction = FindSequenceEndAction(sequenceType);
            SequenceLevel++;

            if (IsSequencing)
                return false;

            IsSequencing = true;
            Sequence = sequenceType;

            ActionsHandler.SendSequenceStartMessage(Clients, TimeLine.Current, sequenceType);

            return true;
        }

        public bool EndSequence(SequenceTypeEnum sequenceType)
        {
            SequenceLevel--;

            if (!IsSequencing || Sequence != sequenceType || SequenceLevel > 0)
                return false;

            IsSequencing = false;
            WaitAcknowledgment = true;

            ActionsHandler.SendSequenceEndMessage(Clients, TimeLine.Current, LastSequenceAction, sequenceType);

            return true;
        }

        public virtual void AcknowledgeAction()
        {
            WaitAcknowledgment = false;

            // todo : find the right usage
        }

        private static FightSequenceAction FindSequenceEndAction(SequenceTypeEnum sequenceTypeEnum)
        {
            switch (sequenceTypeEnum)
            {
                case SequenceTypeEnum.SEQUENCE_MOVE:
                    return FightSequenceAction.Move;
                case SequenceTypeEnum.SEQUENCE_SPELL:
                    return FightSequenceAction.Spell;
                case SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH:
                    return FightSequenceAction.Death;
                default:
                    return FightSequenceAction.None;
            }
        }

        #endregion

        #region Triggers

        private readonly List<MarkTrigger> m_triggers = new List<MarkTrigger>();

        public void AddTriger(MarkTrigger trigger)
        {
            trigger.Triggered += OnMarkTriggered;
            m_triggers.Add(trigger);

            foreach (CharacterFighter fighter in GetAllFighters<CharacterFighter>())
            {
                ContextHandler.SendGameActionFightMarkCellsMessage(fighter.Character.Client, trigger, trigger.DoesSeeTrigger(fighter));
            }
        }

        public void RemoveTrigger(MarkTrigger trigger)
        {
            trigger.Triggered -= OnMarkTriggered;
            m_triggers.Remove(trigger);

            ContextHandler.SendGameActionFightUnmarkCellsMessage(Clients, trigger);
        }

        public void TriggerMarks(Cell cell, FightActor trigger, TriggerType triggerType)
        {
            var triggersToRemove = new List<MarkTrigger>();

            foreach (MarkTrigger markTrigger in m_triggers)
            {
                if (markTrigger.TriggerType == triggerType && markTrigger.ContainsCell(cell))
                {
                    StartSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP);
                    markTrigger.Trigger(trigger);

                    if (markTrigger is Trap)

                        triggersToRemove.Add(markTrigger);

                    EndSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP);
                }
            }

            foreach (MarkTrigger markTrigger in triggersToRemove)
            {
                RemoveTrigger(markTrigger);
            }

            CheckFightEnd();
        }

        public void DecrementGlyphDuration(FightActor caster)
        {
            var triggersToRemove = new List<MarkTrigger>();
            foreach (MarkTrigger trigger in m_triggers)
            {
                if (trigger is Glyph && (trigger as Glyph).Caster == caster)
                {
                    if ((trigger as Glyph).DecrementDuration())
                        triggersToRemove.Add(trigger);
                }
            }

            if (triggersToRemove.Count == 0)
                return;

            StartSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP);
            foreach (MarkTrigger trigger in triggersToRemove)
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

        #endregion

        #region End Fight phase

        public static readonly double[] GroupCoefficients =
            new[]
                {
                    1,
                    1.1,
                    1.5,
                    2.3,
                    3.1,
                    3.6,
                    4.2,
                    4.7
                };

        private bool m_disposed;

        public void Dispose()
        {
            m_disposed = true;

            UnBindFightersEvents();

            if (TimeLine != null)
                TimeLine.Dispose();

            if (m_placementTimer != null)
                m_placementTimer.Dispose();

            Map.RemoveFight(this);

            FightManager.Instance.Remove(this);
            GC.SuppressFinalize(this);
        }

        private int CalculateWinExp(CharacterFighter fighter)
        {
            IEnumerable<MonsterFighter> monsters = fighter.OpposedTeam.GetAllFighters<MonsterFighter>(entry => entry.IsDead());
            IEnumerable<CharacterFighter> players = fighter.Team.GetAllFighters<CharacterFighter>();

            if (!monsters.Any() || !players.Any())
                return 0;

            int sumPlayersLevel = players.Sum(entry => entry.Level);
            byte maxPlayerLevel = players.Max(entry => entry.Level);
            int sumMonstersLevel = monsters.Sum(entry => entry.Level);
            byte maxMonsterLevel = monsters.Max(entry => entry.Level);
            int sumMonsterXp = monsters.Sum(entry => entry.Monster.Grade.GradeXp);

            double levelCoeff = 1;
            if (sumPlayersLevel - 5 > sumMonstersLevel)
                levelCoeff = (double) sumMonstersLevel/sumPlayersLevel;
            else if (sumPlayersLevel + 10 < sumMonstersLevel)
                levelCoeff = (sumPlayersLevel + 10)/(double) sumMonstersLevel;

            double xpRatio = Math.Min(fighter.Level, Math.Truncate(2.5d*maxMonsterLevel))/sumPlayersLevel*100d;

            int regularGroupRatio = players.Where(entry => entry.Level >= maxPlayerLevel/3).Sum(entry => 1);

            if (regularGroupRatio <= 0)
                regularGroupRatio = 1;

            double baseXp = Math.Truncate(xpRatio/100*Math.Truncate(sumMonsterXp*GroupCoefficients[regularGroupRatio - 1]*levelCoeff));
            double multiplicator = AgeBonus <= 0 ? 1 : 1 + AgeBonus/100d;
            var xp = (int) Math.Truncate(Math.Truncate(baseXp*(100 + fighter.Stats[CaracteristicsEnum.Wisdom].Total)/100d)*multiplicator*Rates.XpRate);

            return xp;
        }

        private void ShareLoots()
        {
            foreach (FightTeam team in m_teams)
            {
                IEnumerable<FightActor> droppers = (team == RedTeam ? BlueTeam : RedTeam).GetAllFighters(entry => entry.IsDead());
                IOrderedEnumerable<CharacterFighter> looters = team.GetAllFighters<CharacterFighter>().OrderByDescending(entry => entry.Stats[CaracteristicsEnum.Prospecting].Total);
                int teamPP = team.GetAllFighters().Sum(entry => entry.Stats[CaracteristicsEnum.Prospecting].Total);
                long kamas = droppers.Sum(entry => entry.GetDroppedKamas());

                foreach (CharacterFighter looter in looters)
                {
                    int looterPP = looter.Stats[CaracteristicsEnum.Prospecting].Total;

                    looter.Loot.Kamas = (int) (kamas*((double) looterPP/teamPP)*Rates.KamasRate);

                    foreach (FightActor dropper in droppers)
                    {
                        foreach (DroppedItem item in dropper.RollLoot(looter))
                            looter.Loot.AddItem(item);
                    }
                }
            }
        }

        private void ForceEndFight()
        {
            m_endFightTimer.Dispose();

            EndFight();
        }

        public void RequestEndFight()
        {
            SetFightState(FightState.Ended);

            TimeLine.Dispose();

            m_endFightTimer = Map.Area.CallDelayed(EndFightTimeOut, ForceEndFight);

            NotifyRequestTurnReady();

            if (TimeLine.Current != null)
                ContextHandler.SendGameFightTurnReadyRequestMessage(Clients, TimeLine.Current);
            else
                logger.Debug("TimeLine.Current = null, Index = " + TimeLine.Index + ", Count = " + TimeLine.Count);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CancelFight()
        {
            if (m_disposed)
                return;

            if (State != FightState.Placement)
                return;

            SetFightState(FightState.Ended);

            ForEach(character =>
                        {
                            ContextHandler.SendGameFightEndMessage(character.Client, this);
                            character.RejoinMap();
                        });

            NotifyFightCanceled();
            Dispose();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void EndFight()
        {
            if (m_disposed)
                return;

            TimeLine.Dispose();

            if (m_endFightTimer != null)
                m_endFightTimer.Dispose();

            SetFightState(FightState.Ended);

            ShareLoots();

            foreach (CharacterFighter fighter in GetAllFighters<CharacterFighter>())
                fighter.SetEarnedExperience(CalculateWinExp(fighter));

            IEnumerable<IFightResult> results = GetAllFighters().Select(entry => entry.GetFightResult());

            foreach (IFightResult fightResult in results)
                fightResult.Apply();

            ContextHandler.SendGameFightEndMessage(Clients, this, results.Select(entry => entry.GetFightResultListEntry()));

            ForEach(character =>
                        {
                            character.RejoinMap();
                        });

            NotifyFightEnded();
            Dispose();
        }

        #endregion

        #region Get Methods

        public bool IsCellFree(Cell cell)
        {
            return cell.Walkable && !cell.NonWalkableDuringFight && GetOneFighter(cell) == null;
        }

        public int GetPlacementPhaseTimeLeft()
        {
            if (FightType == FightTypeEnum.FIGHT_TYPE_CHALLENGE)
                return 0;

            double timeleft = PlacementPhaseTime - (DateTime.Now - CreationTime).TotalMilliseconds;

            if (timeleft < 0)
                timeleft = 0;

            return (int) timeleft;
        }

        public int GetFightDuration()
        {
            return (int) (DateTime.Now - StartTime).TotalMilliseconds;
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
            return m_fighters.Where(entry => entry.Id == id).SingleOrDefault();
        }

        public FightActor GetOneFighter(Cell cell)
        {
            return m_fighters.Where(entry => Equals(entry.Position.Cell, cell)).SingleOrDefault();
        }

        public FightActor GetOneFighter(Predicate<FightActor> predicate)
        {
            IEnumerable<FightActor> entries = m_fighters.Where(entry => predicate(entry));

            if (entries.Count() != 0)
                return null;

            return entries.SingleOrDefault();
        }

        public T GetOneFighter<T>(int id) where T : FightActor
        {
            return m_fighters.OfType<T>().Where(entry => entry.Id == id).SingleOrDefault();
        }

        public T GetOneFighter<T>(Cell cell) where T : FightActor
        {
            return m_fighters.OfType<T>().Where(entry => Equals(entry.Position.Cell, cell)).SingleOrDefault();
        }

        public T GetOneFighter<T>(Predicate<T> predicate) where T : FightActor
        {
            return m_fighters.OfType<T>().Where(entry => predicate(entry)).SingleOrDefault();
        }

        public T GetFirstFighter<T>(int id) where T : FightActor
        {
            return m_fighters.OfType<T>().Where(entry => entry.Id == id).FirstOrDefault();
        }

        public T GetFirstFighter<T>(Cell cell) where T : FightActor
        {
            return m_fighters.OfType<T>().Where(entry => Equals(entry.Position.Cell, cell)).FirstOrDefault();
        }

        public T GetFirstFighter<T>(Predicate<T> predicate) where T : FightActor
        {
            return m_fighters.OfType<T>().Where(entry => predicate(entry)).FirstOrDefault();
        }

        public IEnumerable<FightActor> GetAllFighters()
        {
            return m_fighters;
        }

        public IEnumerable<FightActor> GetAllFighters(Cell[] cells)
        {
            return GetAllFighters<FightActor>(entry => cells.Contains(entry.Position.Cell));
        }

        public IEnumerable<FightActor> GetAllFighters(IEnumerable<Cell> cells)
        {
            return GetAllFighters(cells.ToArray());
        }

        public IEnumerable<FightActor> GetAllFighters(Predicate<FightActor> predicate)
        {
            return m_fighters.Where(entry => predicate(entry));
        }

        public IEnumerable<T> GetAllFighters<T>() where T : FightActor
        {
            return m_fighters.OfType<T>();
        }

        public IEnumerable<T> GetAllFighters<T>(Predicate<T> predicate) where T : FightActor
        {
            return m_fighters.OfType<T>().Where(entry => predicate(entry));
        }

        public IEnumerable<int> GetDeadFightersIds()
        {
            return GetAllFighters<FightActor>(entry => entry.IsDead()).Select(entry => entry.Id);
        }

        public IEnumerable<int> GetAliveFightersIds()
        {
            return GetAllFighters<FightActor>(entry => entry.IsAlive()).Select(entry => entry.Id);
        }

        public FightCommonInformations GetFightCommonInformations()
        {
            return new FightCommonInformations(Id,
                                               (sbyte) FightType,
                                               m_teams.Select(entry => entry.GetFightTeamInformations()),
                                               m_teams.Select(entry => entry.BladePosition.Cell.Id),
                                               m_teams.Select(entry => entry.GetFightOptionsInformations()));
        }

        #endregion

        #endregion
    }

    public class FigthCellsInformationProvider : ICellsInformationProvider
    {
        public FigthCellsInformationProvider(Fight fight)
        {
            Fight = fight;
        }

        public Fight Fight
        {
            get;
            private set;
        }

        #region ICellsInformationProvider Members

        public Map Map
        {
            get { return Fight.Map; }
        }

        public bool IsCellWalkable(short cell)
        {
            return Fight.IsCellFree(Fight.Map.Cells[cell]);
        }

        public virtual CellInformation GetCellInformation(short cell)
        {
            return new CellInformation(Fight.Map.Cells[cell], IsCellWalkable(cell), true);
        }

        #endregion
    }
}