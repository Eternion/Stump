using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;

namespace Stump.Server.WorldServer.Game.Fights.History
{
    public class MovementHistory
    {
        [Variable]
        public static readonly int HistoryEntriesLimit = 60;

        private readonly LimitedStack<MovementHistoryEntry> m_underlyingStack = new LimitedStack<MovementHistoryEntry>(HistoryEntriesLimit);

        public MovementHistory(FightActor owner)
        {
            Owner = owner;
            Owner.PositionChanged += OnPositionChanged;
            Owner.StopMoving += OnStopMoving;
        }

        private void OnStopMoving(ContextActor arg1, Path path, bool canceled)
        {
            if (canceled)
                return;

            foreach (var cell in path.GetPath().Skip(1)) // skip the first cell (=start cell)
                RegisterEntry(cell);
        }

        private void OnPositionChanged(ContextActor arg1, ObjectPosition position)
        {
            // compare if the last cell is the same in case the actor moved by itself
            if (m_underlyingStack.Count > 0 && m_underlyingStack.Peek().Cell != position.Cell)
                RegisterEntry(position.Cell);
        }

        public FightActor Owner
        {
            get;
            private set;
        }

        private int CurrentRound
        {
            get { return Owner.Fight.TimeLine.RoundNumber; }
        }

        public void RegisterEntry(Cell cell)
        {
            m_underlyingStack.Push(new MovementHistoryEntry(cell, CurrentRound));
        }

        public MovementHistoryEntry GetPreviousPosition()
        {
            return m_underlyingStack.Count > 0 ? m_underlyingStack.Peek() : null;
        }

        public MovementHistoryEntry PopPreviousPosition()
        {
            return m_underlyingStack.Count > 0 ? m_underlyingStack.Pop() : null;
        }

        public MovementHistoryEntry PopWhile(Predicate<MovementHistoryEntry> predicate)
        {
            MovementHistoryEntry entry = GetPreviousPosition();

            while(entry != null && predicate(entry))
                entry = PopPreviousPosition();

            return entry;
        }

        public MovementHistoryEntry GetPreviousPosition(int lifetime)
        {
            var previous = GetPreviousPosition();

            return CurrentRound - previous?.Round <= lifetime ? previous : null;
        }

        public MovementHistoryEntry PopPreviousPosition(int lifetime)
        {
            var previous = GetPreviousPosition(lifetime);

            if (previous != null)
                PopPreviousPosition();

            return previous;
        }

        public MovementHistoryEntry PopWhile(Predicate<MovementHistoryEntry> predicate, int lifetime)
        {
            MovementHistoryEntry entry = PopPreviousPosition(lifetime);
            
            while (entry != null && predicate(entry) && CurrentRound - entry.Round <= lifetime)
                entry = PopPreviousPosition();

            return entry;
        }

        public IEnumerable<MovementHistoryEntry> GetEntries()
        {
            return m_underlyingStack;
        }

        public IEnumerable<MovementHistoryEntry> GetEntries(Predicate<MovementHistoryEntry> predicate)
        {
            return m_underlyingStack.Where(entry => predicate(entry));
        }

        public IEnumerable<MovementHistoryEntry> GetEntries(int lifetime)
        {
            return GetEntries(x => CurrentRound - x.Round <= lifetime);
        }
    }
}