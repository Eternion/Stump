// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NLog;
using Stump.Server.WorldServer.Groups;

namespace Stump.Server.WorldServer.Fights
{
    public enum TimeLineState
    {
        Idle,
        TurnInProgress,
        TurnEndRequest,
        TurnEnding,
    }

    public class TimeLine : IDisposable
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        #region Events

        #region Delegates

        public delegate void TurnEndedHandler(
            TimeLine sender, FightGroupMember lastFighter, FightGroupMember currentFighter);

        public delegate void TurnEndedRequestHandler(TimeLine sender, FightGroupMember currentFighter);

        public delegate void TurnStartedHandler(TimeLine sender, FightGroupMember currentFighter);

        #endregion

        public event TurnStartedHandler TurnStarted;


        public event TurnEndedRequestHandler TurnEndedRequest;

        public event TurnEndedHandler TurnEnded;

        #endregion

        private Timer m_forceEndTurnTimer;
        private Timer m_turnTimer;

        public TimeLine(Fight fight)
        {
            Fighters = new List<FightGroupMember>();
            Index = -1;
            State = TimeLineState.Idle;

            Fight = fight;

            CreateTimeLine();
        }

        public Fight Fight
        {
            get;
            private set;
        }

        public TimeLineState State
        {
            get;
            private set;
        }

        public List<FightGroupMember> Fighters
        {
            get;
            private set;
        }

        public int Index
        {
            get;
            private set;
        }

        public FightGroupMember Current
        {
            get
            {
                if (Index < 0 || Index >= Fighters.Count)
                    return null;

                return Fighters[Index];
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (m_turnTimer != null)
                m_turnTimer.Dispose();

            if (m_forceEndTurnTimer != null)
                m_forceEndTurnTimer.Dispose();
        }

        #endregion

        /// <summary>
        ///   Change the current fighter and select the next fighter on the timeline
        /// </summary>
        /// <returns></returns>
        public FightGroupMember UpdateToNextFighter()
        {
            Index = (Index + 1) < Fighters.Count ? Index + 1 : 0;

            return Current;
        }

        public FightGroupMember GetLastFighter()
        {
            int index = (Index - 1) < 0 ? 0 : Index - 1;

            return Fighters[index];
        }

        public FightGroupMember GetNextFighter()
        {
            int index = (Index + 1) < Fighters.Count ? Index + 1 : 0;

            return Fighters[index];
        }

        public void StartTurn()
        {
            m_turnTimer = new Timer(TurnEndReached, true, Fight.TurnTime, Timeout.Infinite);

            State = TimeLineState.TurnInProgress;

            if (TurnStarted != null)
                TurnStarted(this, Current);
        }

        public bool RequestTurnEnd(FightGroupMember fighter)
        {
            if (fighter != Current)
                return false;

            m_turnTimer.Dispose();
            TurnEndReached(false);

            return true;
        }

        private void TurnEndReached(object timerEnded)
        {
            try
            {
                State = TimeLineState.TurnEndRequest;

                if (TurnEndedRequest != null)
                    TurnEndedRequest(this, Current);

                m_forceEndTurnTimer = new Timer(ConfirmTurnEnd, null, Fight.TurnEndTimeOut, Timeout.Infinite);
            }
            catch (Exception e)
            {
                logger.Error("Exception thrown on fight <id:{0}> when turn end is requested : {1}", Fight.Id, e);

                try
                {
                    Fight.EndFight();
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                {
                }
                // ReSharper restore EmptyGeneralCatchClause
            }
        }

        public void ConfirmTurnEnd()
        {
            m_forceEndTurnTimer.Dispose();
            ConfirmTurnEnd(null);
        }

        private void ConfirmTurnEnd(object args)
        {
            try
            {
                m_forceEndTurnTimer.Dispose();

                if (State == TimeLineState.TurnEndRequest)
                {
                    State = TimeLineState.TurnEnding;

                    EndTurn();
                }
            }
            catch (Exception e)
            {
                logger.Error("Exception thrown on fight <id:{0}> when turn end is confirmed : {1}", Fight.Id, e);

                try
                {
                    Fight.EndFight();
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                {
                }
                // ReSharper restore EmptyGeneralCatchClause
            }
        }

        private void EndTurn()
        {
            Current.ResetUsedProperties();

            UpdateToNextFighter();

            if (TurnEnded != null)
                TurnEnded(this, GetLastFighter(), Current);

            if (!Fight.CheckIfEnd())
                StartTurn();
        }

        private void CreateTimeLine()
        {
            int sourceIndex = 0;
            int targetIndex = 0;

            IEnumerable<FightGroupMember> sortedSourceGroup =
                Fight.SourceGroup.Members.OrderByDescending(entry => entry.Entity.Stats["Initiative"].Total).OfType
                    <FightGroupMember>();
            IEnumerable<FightGroupMember> sortedTargetGroup =
                Fight.TargetGroup.Members.OrderByDescending(entry => entry.Entity.Stats["Initiative"].Total).OfType
                    <FightGroupMember>();

            bool sourceStart = SourceIsFirst(sortedSourceGroup, sortedTargetGroup);

            while (sourceIndex < sortedSourceGroup.Count() || targetIndex < sortedTargetGroup.Count())
            {
                FightGroupMember next = sourceStart
                                            ? sortedSourceGroup.ElementAtOrDefault(sourceIndex++)
                                            : sortedTargetGroup.ElementAtOrDefault(targetIndex++);

                if (next != null)
                    Fighters.Add(next);

                sourceStart = !sourceStart;
            }
        }

        private static bool SourceIsFirst(IEnumerable<GroupMember> sortedSourceMembers,
                                          IEnumerable<GroupMember> sortedTargetMember)
        {
            return (sortedSourceMembers.First().Entity.Stats["Initiative"].Total >=
                    sortedTargetMember.First().Entity.Stats["Initiative"].Total);
        }
    }
}