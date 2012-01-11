using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.Core.Collections;
using Stump.Core.Timers;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Worlds.Actors.Fight;

namespace Stump.Server.WorldServer.Worlds.Fights
{
    public enum TimeLineState
    {
        Stopped,
        Paused,
        TurnInProgress,
        TurnEndRequested,
        TurnSwitching,
    }

    public class TimeLine : IDisposable
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        #region Events

        #region Delegates

        public delegate void TimeLineEventHandler(TimeLine sender, FightActor currentFighter);

        #endregion

        public event TimeLineEventHandler TurnStarted;

        private void NotifyTurnStarted(FightActor currentfighter)
        {
            TimeLineEventHandler handler = TurnStarted;
            if (handler != null)
                handler(this, currentfighter);
        }

        public event TimeLineEventHandler TurnEndRequest;

        private void NotifyTurnEndRequested(FightActor currentfighter)
        {
            TimeLineEventHandler handler = TurnEndRequest;
            if (handler != null)
                handler(this, currentfighter);
        }

        public event TimeLineEventHandler TurnEnded;

        private void NotifyTurnEnded(FightActor currentfighter)
        {
            TimeLineEventHandler handler = TurnEnded;
            if (handler != null)
                handler(this, currentfighter);
        }

        #endregion

        private readonly ConcurrentList<FightActor> m_fighters;
        private readonly object m_locker = new object();
        private TimerEntry m_turnEndTimer;
        private TimerEntry m_turnTimer;

        public TimeLine(Fight fight, ConcurrentList<FightActor> fighters)
        {
            Fight = fight;
            Index = -1;
            State = TimeLineState.Stopped;
            m_fighters = fighters;
        }

        public Fight Fight
        {
            get;
            private set;
        }

        public bool IsStarted
        {
            get { return State != TimeLineState.Stopped; }
        }

        public TimeLineState State
        {
            get;
            private set;
        }


        public int RoundNumber
        {
            get;
            private set;
        }

        public int Index
        {
            get;
            private set;
        }

        public int Count
        {
            get { return m_fighters.Count; }
        }

        public FightActor Current
        {
            get
            {
                if (Index < 0 || Index >= m_fighters.Count)
                    return null;

                return m_fighters[Index];
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Stop();

            if (m_turnEndTimer != null)
                m_turnEndTimer.Dispose();

            if (m_turnTimer != null)
                m_turnTimer.Dispose();
        }

        #endregion

        public void Start()
        {
            if (IsStarted)
                return;

            StartTurn();
        }

        public void Stop()
        {
            State = TimeLineState.Stopped;
        }

        public void RequestTurnEnd()
        {
            EndTurnRequested();
        }

        public void ConfirmTurnEnd()
        {
            ForceEndTurn();
        }

        public IEnumerable<FightActor> GetTimeLine()
        {
            return m_fighters;
        }

        /*public void InsertAfterCurrent(FightActor fighter)
        {
            Insert(Index + 1, fighter);
        }

        public void Insert(int index, FightActor fighter)
        {
            lock (m_locker)
            {
                Fighters.Insert(index, fighter);
            }
        }*/

        public void ReorganizeTimeLine()
        {
            if (IsStarted)
                return;

            IOrderedEnumerable<FightActor> redFighters = Fight.RedTeam.GetAllFighters().
                OrderByDescending(entry => entry.Stats[CaracteristicsEnum.Initiative].Total);
            IOrderedEnumerable<FightActor> blueFighters = Fight.BlueTeam.GetAllFighters().
                OrderByDescending(entry => entry.Stats[CaracteristicsEnum.Initiative].Total);

            bool redFighterFirst = redFighters.First().Stats[CaracteristicsEnum.Initiative].Total >
                                   blueFighters.First().Stats[CaracteristicsEnum.Initiative].Total;

            IEnumerator<FightActor> redEnumerator = redFighters.GetEnumerator();
            IEnumerator<FightActor> blueEnumerator = blueFighters.GetEnumerator();
            var timeLine = new List<FightActor>();

            bool hasRed;
            bool hasBlue = false;
            while ((hasRed = redEnumerator.MoveNext()) | (hasBlue = blueEnumerator.MoveNext()))
            {
                if (redFighterFirst)
                {
                    if (hasRed)
                        timeLine.Add(redEnumerator.Current);

                    if (hasBlue)
                        timeLine.Add(blueEnumerator.Current);
                }
                else
                {
                    if (hasBlue)
                        timeLine.Add(blueEnumerator.Current);

                    if (hasRed)
                        timeLine.Add(redEnumerator.Current);
                }
            }

            // a bit ugly :/
            // update the list from Fight instance
            m_fighters.Clear();
            m_fighters.AddRange(timeLine);

            Index = 0;
            RoundNumber = 1;
        }

        private void StartTurn()
        {
            if (Fight.State != FightState.Fighting)
                return;

            m_turnTimer = Fight.Map.Area.CallDelayed(Fight.TurnTime, EndTurnRequested);
            State = TimeLineState.TurnInProgress;

            NotifyTurnStarted(Current);
        }

        private void EndTurnRequested()
        {
            lock (m_locker)
            {
                if (State != TimeLineState.TurnInProgress)
                    return;

                m_turnTimer.Dispose();
                State = TimeLineState.TurnEndRequested;

                NotifyTurnEndRequested(Current);

                m_turnEndTimer = Fight.Map.Area.CallDelayed(Fight.TurnEndTimeOut, ForceEndTurn);
            }
        }

        private void ForceEndTurn()
        {
            if (m_turnEndTimer != null)
                m_turnEndTimer.Dispose();

            EndTurn();
        }

        private void EndTurn()
        {
            lock (m_locker)
            {
                State = TimeLineState.TurnSwitching;

                NotifyTurnEnded(Current);
                if (SwitchFighter())
                    StartTurn();
            }
        }

        private bool SwitchFighter()
        {
            if (m_fighters.Count == 0)
                return false;

            int counter = 0;
            int index = (Index + 1) < m_fighters.Count ? Index + 1 : 0;

            while (!m_fighters[index].CanPlay() && counter < m_fighters.Count)
            {
                index = (index + 1) < m_fighters.Count ? index + 1 : 0;
                counter++;
            }

            if (!m_fighters[index].CanPlay()) // no fighter can play
                return false;

            if (index == 0)
                RoundNumber++;

            Index = index;

            return true;
        }
    }
}