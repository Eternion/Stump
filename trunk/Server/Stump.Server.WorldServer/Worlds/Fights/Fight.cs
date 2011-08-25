using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

namespace Stump.Server.WorldServer.Worlds.Fights
{
    public class Fight : IContext
    {
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

        #region Events

        #region Delegates

        public delegate void FightEndedDelegate(Fight fight, FightTeam winners, FightTeam losers, bool draw);

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
            foreach (FightActor actor in m_fighters)
            {
                UnBindFighterEvents(actor);
                BindFighterEvents(actor);
            }
        }

        public event FightEndedDelegate FightEnded;

        private void NotifyFightEnded(FightTeam winners, FightTeam losers, bool draw)
        {
            FightEndedDelegate handler = FightEnded;
            if (handler != null)
                handler(this, winners, losers, draw);
        }

        #endregion

        private readonly List<FightActor> m_fighters = new List<FightActor>();
        private readonly FightTeam[] m_teams;

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
        }

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

        public DateTime StartTime
        {
            get;
            private set;
        }

        public FightState State
        {
            get;
            private set;
        }

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

        public void StartPlacementPhase()
        {
            if (State != FightState.NotStarted)
                return;

            SetFightState(FightState.Placement);

            RandomnizePositions(RedTeam);
            RandomnizePositions(BlueTeam);

            RedTeam.Blade.Show();
            BlueTeam.Blade.Show();
        }

        private void UnBindFighterEvents(FightActor actor)
        {
            actor.ReadyStateChanged -= OnSetReady;
            actor.PrePlacementChanged -= OnChangePreplacementPosition;

            var fighter = actor as CharacterFighter;

            if (fighter != null)
            {
                fighter.Character.LoggedOut += OnLoggedOut;
            }
        }

        private void BindFighterEvents(FightActor actor)
        {
            if (State == FightState.Placement)
            {
                actor.ReadyStateChanged += OnSetReady;
                actor.PrePlacementChanged += OnChangePreplacementPosition;
            }

            var fighter = actor as CharacterFighter;

            if (fighter != null)
            {
                fighter.Character.LoggedOut += OnLoggedOut;
            }
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
            return State == FightState.Placement && fighter.Team.PlacementCells.Contains(cell) && GetOneFighter(cell) == null;
        }

        private void OnChangePreplacementPosition(FightActor fighter, ObjectPosition objectPosition)
        {
            ForEach(character =>
                    ContextHandler.SendGameEntitiesDispositionMessage(character.Client,
                                                                      GetAllFighters()));
        }

        public void StartFight()
        {
             // bla

            SetFightState(FightState.Fighting);
        }

        public void SetFightState(FightState state)
        {
            State = state;

            NotifyStateChanged();
        }

        private void OnFighterAdded(FightTeam team, FightActor fighter)
        {
            m_fighters.Add(fighter);

            BindFighterEvents(fighter);

            if (!(fighter is CharacterFighter))
                return;

            Character character = (fighter as CharacterFighter).Character;

            ContextHandler.SendGameFightJoinMessage(character.Client, true, true, false, false, 0, team.Fight.FightType);

            if (State == FightState.Placement || State == FightState.NotStarted)
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

        private void OnFighterRemoved(FightTeam team, FightActor fighter)
        {
            m_fighters.Remove(fighter);
        }

        private void OnLoggedOut(Character character)
        {
            // todo
        }

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
            return m_fighters.Where(entry => predicate(entry)).SingleOrDefault();
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
    }
}