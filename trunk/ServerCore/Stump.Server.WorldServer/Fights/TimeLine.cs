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
using Stump.Server.WorldServer.Groups;

namespace Stump.Server.WorldServer.Fights
{
    public class TimeLine
    {
        private readonly Fight m_fight;
        private readonly List<FightGroupMember> m_timeline;
        private int m_index;

        public TimeLine(Fight fight)
        {
            m_fight = fight;
            m_timeline = new List<FightGroupMember>();
            CreateTimeLine();
            m_index = -1;
        }

        public FightGroupMember Current
        {
            get
            {
                if (m_index < 0 || m_index >= m_timeline.Count)
                    return null;

                return m_timeline[m_index];
            }
        }

        private void CreateTimeLine()
        {
            int sourceIndex = 0;
            int targetIndex = 0;

            List<GroupMember> sortedSourceGroup =
                m_fight.SourceGroup.Members.OrderByDescending(entry => entry.Entity.Stats["Initiative"].Total).ToList();
            List<GroupMember> sortedTargetGroup =
                m_fight.TargetGroup.Members.OrderByDescending(entry => entry.Entity.Stats["Initiative"].Total).ToList();

            bool sourceStart = SourceIsFirst(sortedSourceGroup, sortedTargetGroup);

            while (sourceIndex < sortedSourceGroup.Count || targetIndex < sortedTargetGroup.Count)
            {
                FightGroupMember next = sourceStart
                                            ? GetNextMember(sortedSourceGroup, sourceIndex++)
                                            : GetNextMember(sortedTargetGroup, targetIndex++);

                if (next != null)
                    m_timeline.Add(next);

                sourceStart = !sourceStart;
            }
        }

        private static FightGroupMember GetNextMember(List<GroupMember> groupMembers, int index)
        {
            if (index >= groupMembers.Count)
                return null;

            return groupMembers[index] as FightGroupMember;
        }

        private static bool SourceIsFirst(IEnumerable<GroupMember> sortedSourceMembers,
                                          IEnumerable<GroupMember> sortedTargetMember)
        {
            return (sortedSourceMembers.First().Entity.Stats["Initiative"].Total >=
                    sortedTargetMember.First().Entity.Stats["Initiative"].Total);
        }

        public FightGroupMember GetNext()
        {
            m_index = (m_index + 1) < m_timeline.Count ? m_index + 1 : 0;

            return Current;
        }
    }
}