using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Core.Pool;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Worlds.Actors;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Fights.Buffs;
using Stump.Server.WorldServer.Worlds.Fights.Triggers;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Maps.Pathfinding;
using Stump.Server.WorldServer.Worlds.Spells;
using TriggerType = Stump.Server.WorldServer.Worlds.Fights.Triggers.TriggerType;

namespace Stump.Server.WorldServer.Worlds.Fights
{
    public class Fight : IContext, IDisposable
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
        private readonly List<FightActor> m_fighters = new List<FightActor>();
        private readonly FightTeam[] m_teams;
        private readonly UniqueIdProvider m_triggerIdProvider = new UniqueIdProvider();
        private Timer m_endFightTimer;
        private Timer m_placementTimer;

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
                m_placementTimer = new Timer(EndPlacementPhase, null, PlacementPhaseTime, Timeout.Infinite);
        }

        private void EndPlacementPhase(object dummy)
        {
            try
            {
                m_placementTimer.Dispose();

                StartFight();
            }
            catch (Exception ex)
            {
                logger.Error("Unhandled Thread Exception : {0}", ex);
                Dispose();
            }
        }

        public void ShowBlades()
        {
            if (BladesVisible)
                return;

            Map.ForEach(character => ContextHandler.SendGameRolePlayShowChallengeMessage(character.Client, this));

            RedTeam.TeamOptionsChanged += OnTeamOptionsChanged;
            BlueTeam.TeamOptionsChanged += OnTeamOptionsChanged;

            BladesVisible = true;
        }

        public void HideBlades()
        {
            if (!BladesVisible)
                return;

            Map.ForEach(character => ContextHandler.SendGameRolePlayRemoveChallengeMessage(character.Client, this));

            RedTeam.TeamOptionsChanged -= OnTeamOptionsChanged;
            BlueTeam.TeamOptionsChanged -= OnTeamOptionsChanged;

            BladesVisible = false;
        }

        private void OnTeamOptionsChanged(FightTeam team, FightOptionsEnum option)
        {
            ForEach(entry => ContextHandler.SendGameFightOptionStateUpdateMessage(entry.Client, team, option, team.GetOptionState(option)));
            Map.ForEach(entry => ContextHandler.SendGameFightOptionStateUpdateMessage(entry.Client, team, option, team.GetOptionState(option)));
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

        public DirectionsEnum FoundPlacementDirection(FightActor fighter)
        {
            FightTeam team = fighter.Team == RedTeam ? BlueTeam : RedTeam;

            Tuple<Cell, uint> closerCell = null;
            foreach (Cell cell in team.PlacementCells)
            {
                var point = new MapPoint(cell);

                if (closerCell == null)
                    closerCell = Tuple.Create(cell,
                                              fighter.Position.Point.DistanceTo(point));
                else
                {
                    if (fighter.Position.Point.DistanceTo(point) < closerCell.Item2)
                        closerCell = Tuple.Create(cell,
                                                  fighter.Position.Point.DistanceTo(point));
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

            ForEach(
                charac => ContextHandler.SendGameFightHumanReadyStateMessage(charac.Client, fighter));

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
            return State == FightState.Placement &&
                   fighter.Team.PlacementCells.Contains(cell) &&
                   GetOneFighter(cell) == null;
        }

        private void OnChangePreplacementPosition(FightActor fighter, ObjectPosition objectPosition)
        {
            UpdateFightersPlacementDirection();

            ForEach(character =>
                    ContextHandler.SendGameEntitiesDispositionMessage(character.Client,
                                                                      GetAllFighters()));
        }

        private void UpdateFightersPlacementDirection()
        {
            foreach (FightActor fighter in m_fighters)
            {
                fighter.Position.Direction = FoundPlacementDirection(fighter);
            }
        }

        private void OnCellShown(FightActor fighter, Cell cell)
        {
            ForEach(entry => ContextHandler.SendShowCellMessage(entry.Client, fighter, cell));
        }

        public void KickPlayer(CharacterFighter fighter)
        {
            LeaveFight(fighter);
        }

        public void LeaveFight(FightActor fighter)
        {
            if (FightType != FightTypeEnum.FIGHT_TYPE_CHALLENGE)
                fighter.Die();

            ForEach(entry => ContextHandler.SendGameFightLeaveMessage(entry.Client, fighter));

            fighter.Team.RemoveFighter(fighter);

            if (fighter is CharacterFighter)
            {
                Character character = (fighter as CharacterFighter).Character;

                // can be logged out
                if (character.InWorld)
                {
                    ContextHandler.SendGameFightEndMessage(character.Client, this);
                    character.RejoinMap();
                }
            }

            if (!CheckFightEnd() && fighter.IsFighterTurn())
                RequestTurnEnd();
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

            ForEach(character =>
                        {
                            ContextHandler.SendGameEntitiesDispositionMessage(character.Client, GetAllFighters());

                            ContextHandler.SendGameFightStartMessage(character.Client);
                            ContextHandler.SendGameFightTurnListMessage(character.Client, this);

                            ContextHandler.SendGameFightSynchronizeMessage(character.Client, this);
                            CharacterHandler.SendCharacterStatsListMessage(character.Client);
                        });

            TimeLine.Start();
        }

        private void OnSetTurnReady(FightActor fighter, bool isReady)
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

            ForEach(entry => ActionsHandler.SendGameActionFightDeathMessage(entry.Client, fighter));

            EndSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);
        }

        private void OnDamageReducted(FightActor fighter, FightActor source, int reduction)
        {
            ForEach(entry => ActionsHandler.SendGameActionFightReduceDamagesMessage(entry.Client, source, fighter, reduction));
        }

        private void OnStartMoving(ContextActor actor, Path path)
        {
            var fighter = actor as FightActor;

            if (!fighter.IsFighterTurn())
                return;

            var movementsKeys = path.GetServerPathKeys();

            StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
            ForEach(entry => ContextHandler.SendGameMapMovementMessage(entry.Client, movementsKeys, fighter));
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
            ForEach(entry => ActionsHandler.
                                 SendGameActionFightPointsVariationMessage(entry.Client, action, source, target, delta));
        }

        private void OnLifePointsChanged(FightActor actor, int delta, FightActor from)
        {
            ForEach(entry => ActionsHandler.SendGameActionFightLifePointsVariationMessage(entry.Client, from ?? actor, actor, (short) delta));
        }

        private void OnSpellCasting(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical)
        {
            ForEach(entry => ContextHandler.SendGameActionFightSpellCastMessage(entry.Client, ActionsEnum.ACTION_FIGHT_CAST_SPELL,
                                                                                caster, target, critical, false, spell));

            if (critical == FightSpellCastCriticalEnum.CRITICAL_FAIL)
                EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);
        }

        private void OnSpellCasted(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical)
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


        // todo
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

            ForEach(entry => ContextHandler.SendGameFightShowFighterMessage(entry.Client, fighter));

            if (BladesVisible)
                Map.ForEach(entry => ContextHandler.SendGameFightUpdateTeamMessage(entry.Client, this, team));
        }

        private void OnFighterRemoved(FightTeam team, FightActor fighter)
        {
            m_fighters.Remove(fighter);
            UnBindFighterEvents(fighter);

            if (State == FightState.Placement)
            {
                ForEach(entry => ContextHandler.SendGameFightRemoveTeamMemberMessage(entry.Client, fighter));

                if (BladesVisible)
                    Map.ForEach(entry => ContextHandler.SendGameFightRemoveTeamMemberMessage(entry.Client, fighter));
            }
            else if (State == FightState.Fighting)
            {
                ForEach(entry => ContextHandler.SendGameContextRemoveElementMessage(entry.Client, fighter));
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

            ForEach(
                entry =>
                    {
                        ContextHandler.SendGameFightTurnEndMessage(entry.Client, currentFighter);
                        ContextHandler.SendGameFightTurnReadyRequestMessage(entry.Client, currentFighter);
                    });
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
            LastSequenceAction = FindSequenceEndAction(Sequence);
            SequenceLevel++;

            if (IsSequencing)
                return false;

            IsSequencing = true;
            Sequence = sequenceType;

            ForEach(entry => ActionsHandler.SendSequenceStartMessage(entry.Client, TimeLine.Current, sequenceType));

            return true;
        }

        public bool EndSequence(SequenceTypeEnum sequenceType)
        {
            SequenceLevel--;

            if (!IsSequencing || Sequence != sequenceType || SequenceLevel > 0)
                return false;

            IsSequencing = false;
            WaitAcknowledgment = true;

            ForEach(entry => ActionsHandler.SendSequenceEndMessage(entry.Client, TimeLine.Current, LastSequenceAction, sequenceType));

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

            ForEach(entry => ContextHandler.SendGameActionFightUnmarkCellsMessage(entry.Client, trigger));
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
            ForEach(entry => ContextHandler.SendGameActionFightTriggerGlyphTrapMessage(entry.Client, markTrigger, trigger, triggerSpell));
        }

        #endregion

        #endregion

        #region End Fight phase

        public void Dispose()
        {
            UnBindFightersEvents();

            if (TimeLine != null)
                TimeLine.Dispose();

            Map.RemoveFight(this);

            FightManager.Instance.Remove(this);
        }

        public void CancelFight()
        {
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

        public void RequestEndFight()
        {
            SetFightState(FightState.Ended);

            TimeLine.Dispose();

            NotifyRequestTurnReady();
            ForEach(
                entry => ContextHandler.SendGameFightTurnReadyRequestMessage(entry.Client, TimeLine.Current));

            m_endFightTimer = new Timer(ForceEndFight, null, EndFightTimeOut, Timeout.Infinite);
        }

        private void ForceEndFight(object state)
        {
            try
            {
                m_endFightTimer.Dispose();

                EndFight();
            }
            catch (Exception ex)
            {
                logger.Error("Unhandled Thread Exception : {0}", ex);
                Dispose();
            }
        }

        public void EndFight()
        {
            TimeLine.Dispose();

            if (m_endFightTimer != null)
                m_endFightTimer.Dispose();

            SetFightState(FightState.Ended);

            ForEach(character =>
                        {
                            ContextHandler.SendGameFightEndMessage(character.Client, this);
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

        public Map Map
        {
            get { return Fight.Map; }
        }

        #region ICellsInformationProvider Members

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