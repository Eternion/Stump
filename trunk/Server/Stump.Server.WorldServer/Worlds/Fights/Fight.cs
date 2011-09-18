using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Worlds.Actors;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Maps.Pathfinding;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Fights
{
    public class Fight : IContext, IDisposable
    {
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

        public Action<Fight, FightActor, Cell> CellShown;

        private void NotifyCellShown(FightActor fighter, Cell cell)
        {
            var handler = CellShown;
            if (handler != null)
                CellShown(this, fighter, cell);
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

            BlueTeam.FighterAdded += OnFighterAdded;
            BlueTeam.FighterRemoved += OnFighterRemoved;
            RedTeam.FighterAdded += OnFighterAdded;
            RedTeam.FighterRemoved += OnFighterRemoved;

            CreationTime = DateTime.Now;
        }

        #endregion

        #region Properties

        private readonly List<FightActor> m_fighters = new List<FightActor>();
        private readonly FightTeam[] m_teams;
        private Timer m_endFightTimer;

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

        public int PlacementPhaseTimeLeft
        {
            get
            {
                if (FightType == FightTypeEnum.FIGHT_TYPE_CHALLENGE)
                    return 0;

                var timeleft = PlacementPhaseTime - ( DateTime.Now - CreationTime ).TotalMilliseconds;

                if (timeleft < 0)
                    timeleft = 0;

                return (int) timeleft;
            }
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

        #endregion

        #region Methods

        #region IContext Members

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

        #endregion

        #region Events Binders

        private void UnBindFightersEvents()
        {
            foreach (var fighter in m_fighters)
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
            actor.FightPointsVariation -= OnFightPointsVariation;
            actor.LifePointsChanged -= OnLifePointsChanged;
            actor.SpellCasting -= OnSpellCasting;
            actor.SpellCasted -= OnSpellCasted;
            actor.TurnReadyStateChanged -= OnSetTurnReady;
            actor.Dead -= OnDead;
            actor.SequenceStarted -= OnSequenceStarted;
            actor.SequenceEnded -= OnSequenceEnded;


            var fighter = actor as CharacterFighter;

            if (fighter != null)
            {
                fighter.Character.LoggedOut -= OnLoggedOut;
            }
        }

        private void BindFightersEvents()
        {
            foreach (var fighter in m_fighters)
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
                actor.SequenceStarted += OnSequenceStarted;
                actor.SequenceEnded += OnSequenceEnded;

                actor.StartMoving += OnStartMoving;
                actor.StopMoving += OnStopMoving;
                actor.FightPointsVariation += OnFightPointsVariation;
                actor.LifePointsChanged += OnLifePointsChanged;

                actor.SpellCasting += OnSpellCasting;
                actor.SpellCasted += OnSpellCasted;

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

        public void RandomnizePosition(FightActor fighter)
        {
            var availableCells = fighter.Team.PlacementCells.Where(entry => GetOneFighter(entry) == null).ToArray();

            if (availableCells.Length == 0)
                return;

            var random = new Random();

            fighter.ChangePrePlacement(availableCells[random.Next(availableCells.Length)]);
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
            var team = fighter.Team == RedTeam ? BlueTeam : RedTeam;

            Tuple<FightActor, uint> closerOpposant = null;
            foreach (var opposant in team.GetAllFighters())
            {
                if (closerOpposant == null)
                    closerOpposant = Tuple.Create(opposant,
                        fighter.Position.Point.DistanceTo(opposant.Position.Point));
                else
                {
                    if (fighter.Position.Point.DistanceTo(opposant.Position.Point) < closerOpposant.Item2)
                        closerOpposant = Tuple.Create(opposant,
                            fighter.Position.Point.DistanceTo(opposant.Position.Point));
                }
            }

            if (closerOpposant == null)
                return fighter.Position.Direction;

            return fighter.Position.Point.OrientationTo(closerOpposant.Item1.Position.Point);
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
            foreach (var fighter in m_fighters)
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

        public void LeaveFight(CharacterFighter fighter)
        {
            ForEach(entry => ContextHandler.SendGameFightLeaveMessage(entry.Client, fighter));

            fighter.Team.RemoveFighter(fighter);

            ContextHandler.SendGameFightEndMessage(fighter.Character.Client, this);
            CharacterHandler.SendCharactersListMessage(fighter.Character.Client);
            fighter.Character.RejoinMap();

            CheckFightEnd();
        }

        #endregion

        #region Fighting phase

        public void StartFight()
        {
            RedTeam.SetAllTurnReady(false);
            BlueTeam.SetAllTurnReady(false);
            HideBlades();

            ForEach(character =>
                        {
                            ContextHandler.SendGameEntitiesDispositionMessage(character.Client, GetAllFighters());

                            ContextHandler.SendGameFightStartMessage(character.Client);
                            ContextHandler.SendGameFightTurnListMessage(character.Client, this);

                            ContextHandler.SendGameFightSynchronizeMessage(character.Client, this);
                            CharacterHandler.SendCharacterStatsListMessage(character.Client);
                        });

            SetFightState(FightState.Fighting);

            TimeLine = new TimeLine(this, m_fighters);
            TimeLine.TurnStarted += OnTurnStarted;
            TimeLine.TurnEndRequest += OnTurnEndRequest;
            TimeLine.TurnEnded += OnTurnEnded;
            TimeLine.Start();
        }

        #region Timeline Management

        private void OnTurnStarted(TimeLine sender, FightActor currentfighter)
        {
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
            ForEach(
                entry =>
                {
                    ContextHandler.SendGameFightTurnEndMessage(entry.Client, currentFighter);
                    ContextHandler.SendGameFightTurnReadyRequestMessage(entry.Client, currentFighter);
                });
        }

        private void OnTurnEnded(TimeLine sender, FightActor currentFighter)
        {
            RedTeam.SetAllTurnReady(false);
            BlueTeam.SetAllTurnReady(false);

            currentFighter.ResetPoints();
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
            if (killedBy == null || !killedBy.IsSequencing)
                fighter.StartSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH);

            ForEach(entry => ActionsHandler.SendGameActionFightDeathMessage(entry.Client, fighter));

            if (killedBy != null && killedBy.IsSequencing)
                killedBy.LastSequenceAction = FightSequenceAction.Death;
            else
                fighter.EndSequence();
        }

        private void OnStartMoving(ContextActor actor, MovementPath path)
        {
            var fighter = actor as FightActor;

            if (!fighter.IsFighterTurn())
                return;

            var movementsKeys = path.GetServerMovementKeys();

            fighter.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
            ForEach(entry => ContextHandler.SendGameMapMovementMessage(entry.Client, movementsKeys, fighter));
            fighter.UseMP((short) path.MpCost);
            fighter.EndSequence();
        }

        private void OnStopMoving(ContextActor actor, MovementPath path, bool canceled)
        {
            var fighter = actor as FightActor;

            if (!fighter.IsFighterTurn())
                return;

            if (canceled)
            {
                // error, it shouldn't be canceled in a fight.
            }
        }

        private void OnFightPointsVariation(FightActor actor, ActionsEnum action, FightActor source, FightActor target, short delta)
        {
            ForEach(entry => ActionsHandler.
                SendGameActionFightPointsVariationMessage(entry.Client, action, source, target, delta));
        }

        private void OnLifePointsChanged(FightActor actor, int delta, FightActor from)
        {
            ForEach(entry => ActionsHandler.SendGameActionFightLifePointsVariationMessage(entry.Client, from ?? actor, actor, (short)delta));
        }

        private void OnSpellCasting(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical)
        {
            ForEach(entry => ContextHandler.SendGameActionFightSpellCastMessage(entry.Client, ActionsEnum.ACTION_FIGHT_CAST_SPELL,
                caster, target, critical, false, spell));

            if (critical == FightSpellCastCriticalEnum.CRITICAL_FAIL)
                caster.EndSequence();
        }

        private void OnSpellCasted(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical)
        {
            if (caster.IsSequencing)
                caster.EndSequence();

            CheckFightEnd();
        }

        private void OnSequenceStarted(FightActor fighter, SequenceTypeEnum sequenceType)
        {
            ForEach(entry => ActionsHandler.SendSequenceStartMessage(entry.Client, fighter, sequenceType));
        }

        private void OnSequenceEnded(FightActor fighter, SequenceTypeEnum sequenceType, FightSequenceAction sequenceEndAction)
        {
            // todo : find actionid utility
            ForEach(entry => ActionsHandler.SendSequenceEndMessage(entry.Client, fighter, sequenceEndAction, sequenceType));
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

            ForEach(entry => ContextHandler.SendGameFightShowFighterMessage(entry.Client, fighter));

            if (State == FightState.Placement || State == FightState.NotStarted)
            {
                if (State == FightState.Placement)
                    RandomnizePosition(fighter);

                if (fighter is CharacterFighter)
                {
                    Character character = ( (CharacterFighter)fighter ).Character;

                    ContextHandler.SendGameFightJoinMessage(character.Client,
                        fighter.IsTeamLeader() && FightType == FightTypeEnum.FIGHT_TYPE_CHALLENGE,
                        true,
                        false,
                        false,
                        PlacementPhaseTimeLeft,
                        team.Fight.FightType);
                    ContextHandler.SendGameFightPlacementPossiblePositionsMessage(character.Client, this, fighter.Team.Id);

                    foreach (FightActor fightActor in m_fighters)
                    {
                        ContextHandler.SendGameFightShowFighterMessage(character.Client, fightActor);

                        if (fightActor is CharacterFighter)
                        {
                            var characterFighter = fightActor as CharacterFighter;

                            ContextHandler.SendGameFightShowFighterMessage(characterFighter.Character.Client, fighter);
                        }
                    }

                    ContextHandler.SendGameEntitiesDispositionMessage(character.Client, GetAllFighters());

                    ContextHandler.SendGameFightUpdateTeamMessage(character.Client, this, RedTeam);
                    ContextHandler.SendGameFightUpdateTeamMessage(character.Client, this, BlueTeam);
                }
            }

            if (BladesVisible)
                Map.ForEach(entry => ContextHandler.SendGameFightUpdateTeamMessage(entry.Client, this, team));
        }

        private void OnFighterRemoved(FightTeam team, FightActor fighter)
        {
            m_fighters.Remove(fighter);
            UnBindFighterEvents(fighter);

            ForEach(entry => ContextHandler.SendGameFightRemoveTeamMemberMessage(entry.Client, fighter));

            if (BladesVisible)
                Map.ForEach(entry => ContextHandler.SendGameFightRemoveTeamMemberMessage(entry.Client, fighter));
        }

        private void OnLoggedOut(Character character)
        {
            LeaveFight(character.Fighter);
        }

        public void CheckFightEnd()
        {
            if (!RedTeam.AreAllDead() && !BlueTeam.AreAllDead())
                return;

            if (State == FightState.Fighting)
                RequestEndFight();
            else
                EndFight();
        }

        #endregion

        #region End Fight phase

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

            ForEach(
                entry => ContextHandler.SendGameFightTurnReadyRequestMessage(entry.Client, TimeLine.Current));

            m_endFightTimer = new Timer(ForceEndFight, null, EndFightTimeOut, Timeout.Infinite);
        }

        private void ForceEndFight(object state)
        {
            m_endFightTimer.Dispose();

            EndFight();
        }

        public void EndFight()
        {
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

        public void Dispose()
        {
            UnBindFightersEvents();

            if (TimeLine != null)
                TimeLine.Dispose();

            Map.RemoveFight(this);

            FightManager.Instance.Remove(this);
        }
        #endregion

        #region Get Methods

        public int GetFightDuration()
        {
            return (int) (DateTime.Now - StartTime).TotalMilliseconds;
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
            var entries = m_fighters.Where(entry => predicate(entry));

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

        public IEnumerable<FightActor> GetAllFighters()
        {
            return m_fighters;
        }

        public IEnumerable<FightActor> GetAllFighters(Cell[] cells)
        {
            return GetAllFighters<FightActor>(entry => cells.Contains(entry.Position.Cell));
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
                (sbyte)FightType,
                m_teams.Select(entry => entry.GetFightTeamInformations()),
                m_teams.Select(entry => entry.BladePosition.Cell.Id),
                m_teams.Select(entry => entry.GetFightOptionsInformations()));
        }

        #endregion

        #endregion        
    }
}