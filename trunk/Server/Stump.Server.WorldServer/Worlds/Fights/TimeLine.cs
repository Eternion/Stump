using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NLog;
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

        public delegate void TimeLineEventHandler(TimeLine sender,  FightActor currentFighter);

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

        private Timer m_turnTimer;
        private Timer m_turnEndTimer;

        private List<FightActor> m_fighters;

        public TimeLine(Fight fight, List<FightActor> fighters)
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

        public FightActor Current
        {
            get
            {
                if (Index < 0 || Index >= m_fighters.Count)
                    return null;

                return m_fighters[Index];
            }
        }

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
            EndTurnRequested(null);
        }

        public void ConfirmTurnEnd()
        {
            ForceEndTurn(null);
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

            var redFighters = Fight.RedTeam.GetAllFighters().
                OrderByDescending(entry => entry.Stats[CaracteristicsEnum.Initiative].Total);
            var blueFighters = Fight.BlueTeam.GetAllFighters().
                OrderByDescending(entry => entry.Stats[CaracteristicsEnum.Initiative].Total);

            bool redFighterFirst = redFighters.First().Stats[CaracteristicsEnum.Initiative].Total > 
                blueFighters.First().Stats[CaracteristicsEnum.Initiative].Total;

            var redEnumerator = redFighters.GetEnumerator();
            var blueEnumerator = blueFighters.GetEnumerator();
            var timeLine = new List<FightActor>();

            bool hasRed;
            bool hasBlue = false;
            while ((hasRed = redEnumerator.MoveNext()) || (hasBlue = blueEnumerator.MoveNext()))
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

            m_turnTimer = new Timer(EndTurnRequested, null, Fight.TurnTime, Timeout.Infinite);
            State = TimeLineState.TurnInProgress;

            NotifyTurnStarted(Current);
        }

        private object m_locker = new object();
        private void EndTurnRequested(object dummy)
        {
            try
            {
                lock (m_locker)
                {
                    if (State != TimeLineState.TurnInProgress)
                        return;

                    m_turnTimer.Dispose();
                    State = TimeLineState.TurnEndRequested;

                    NotifyTurnEndRequested(Current);

                    m_turnEndTimer = new Timer(ForceEndTurn, null, Fight.TurnEndTimeOut, Timeout.Infinite);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Unhandled Thread Exception : {0}", ex);
                Dispose();
            }
        }

        private void ForceEndTurn(object dummy)
        {
            try
            {
                m_turnEndTimer.Dispose();

                EndTurn();
            }
            catch(Exception ex)
            {
                logger.Error("Unhandled Thread Exception : {0}", ex);
                Dispose();
            }
        }

        private void EndTurn()
        {
            lock (m_locker)
            {
                if (State != TimeLineState.TurnEndRequested)
                    return;

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
                index = ( index + 1 ) < m_fighters.Count ? index + 1 : 0;
                counter++;
            }

            if (!m_fighters[index].CanPlay()) // no fighter can play
                return false;

            if (index == 0)
                RoundNumber++;

            Index = index;

            return true;
        }

        public void Dispose()
        {
            Stop();

            if (m_turnEndTimer != null)
                m_turnEndTimer.Dispose();

            if (m_turnTimer != null)
                m_turnTimer.Dispose();
        }
    }
}