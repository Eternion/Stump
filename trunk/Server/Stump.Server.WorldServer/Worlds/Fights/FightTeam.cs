using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Cache;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;

namespace Stump.Server.WorldServer.Worlds.Fights
{
    public class FightTeam
    {
        #region Events
        public event Action<FightTeam, FightActor> FighterAdded;

        private void NotifyFighterAdded(FightActor fighter)
        {
            Action<FightTeam, FightActor> handler = FighterAdded;
            if (handler != null)
                handler(this, fighter);
        }

        public event Action<FightTeam, FightActor> FighterRemoved;

        private void NotifyFighterRemoved(FightActor fighter)
        {
            Action<FightTeam, FightActor> handler = FighterRemoved;
            if (handler != null)
                handler(this, fighter);
        }

        #endregion

        private readonly List<FightActor> m_fighters = new List<FightActor>();
        private readonly object m_locker = new object();

        public FightTeam(sbyte id, Cell[] placementCells, TeamEnum teamType)
        {
            Id = id;
            PlacementCells = placementCells;
            TeamType = teamType;

            Blade = new FightBlade(this);
        }

        public sbyte Id
        {
            get;
            private set;
        }

        public FightBlade Blade
        {
            get;
            private set;
        }

        public Cell[] PlacementCells
        {
            get;
            private set;
        }

        public TeamEnum TeamType
        {
            get;
            private set;
        }

        public Fight Fight
        {
            get;
            internal set;
        }

        public FightActor Leader
        {
            get
            {
                return m_fighters.Count > 0 ? m_fighters.First() : null;
            }
        }

        public bool AreAllReady()
        {
            return m_fighters.All(entry => entry.IsReady);
        }

        public bool IsFull()
        {
            return Fight.State == FightState.Placement && m_fighters.Count > PlacementCells.Count();
        }

        public bool AddFighter(FightActor actor)
        {
            if (IsFull())
                return false;

            bool firstFighter = Leader == null;

            lock (m_locker)
            {
                m_fighters.Add(actor);

                if (firstFighter)
                    Blade.Move(actor.MapPosition);

                NotifyFighterAdded(actor);
                return true;
            }
        }

        public bool RemoveFighter(FightActor actor)
        {
            if (IsFull())
                return false;

            lock (m_locker)
            {
                m_fighters.Remove(actor);

                NotifyFighterRemoved(actor);
                return true;
            }
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

        public FightTeamInformations GetFightTeamInformations()
        {
            return new FightTeamInformations(Id,
                Leader != null ? Leader.Id : 0,
                (sbyte) AlignmentSideEnum.ALIGNMENT_WITHOUT,
                (sbyte) TeamType,
                m_fighters.Select(entry => entry.GetFightTeamMemberInformations())
                );
        }
    }
}