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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Stump.Server.WorldServer.Groups;

namespace Stump.Server.WorldServer.Fights
{
    public enum TimeLineState
    {
        Sleeping,
        TurnInProgress,
        TurnEndRequest
    }

    public class TimeLine
    {
        #region Events

        public delegate void TurnStartedHandler(TimeLine sender, FightGroupMember currentFighter);
        public event TurnStartedHandler TurnStarted;


        public delegate void TurnEndedHandler(TimeLine sender, FightGroupMember oldFighter, FightGroupMember newFighter);
        public event TurnEndedHandler TurnEnded;

        #endregion

        private Timer m_turnEndTimer;

        public TimeLine(Fight fight)
        {
            Fighters = new List<FightGroupMember>();
            Index = -1;
            State = TimeLineState.Sleeping;

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

        /// <summary>
        /// Change the current fighter and select the next fighter on the timeline
        /// </summary>
        /// <returns></returns>
        /// <remarks>You must not call it to change the turn. Use it wisely !</remarks>
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

        public void StartTurnTimer()
        {
            m_turnEndTimer = new Timer(TurnEndReached, true, Fight.TurnTime, Timeout.Infinite);

            State = TimeLineState.TurnInProgress;

            if (TurnStarted != null)
                TurnStarted(this, Current);
        }

        public bool EndTurn(FightGroupMember fighter)
        {
            if(fighter != Current)
                return false;

            TurnEndReached(false);

            return true;
        }

        private void TurnEndReached(object timerEnded)
        {
            m_turnEndTimer.Dispose();

            UpdateToNextFighter();

            State = TimeLineState.TurnEndRequest;

            if (TurnEnded != null)
                TurnEnded(this, GetLastFighter(), Current);
        }

        private void CreateTimeLine()
        {
            int sourceIndex = 0;
            int targetIndex = 0;

            var sortedSourceGroup =
                Fight.SourceGroup.Members.OrderByDescending(entry => entry.Entity.Stats["Initiative"].Total).OfType<FightGroupMember>();
            var sortedTargetGroup =
                Fight.TargetGroup.Members.OrderByDescending(entry => entry.Entity.Stats["Initiative"].Total).OfType<FightGroupMember>();

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