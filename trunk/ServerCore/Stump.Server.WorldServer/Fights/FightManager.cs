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
using System.Linq;
using Stump.Server.BaseServer.Manager;

namespace Stump.Server.WorldServer.Fights
{
    public class FightManager : InstanceManager<Fight>
    {
        /// <summary>
        ///   Remove the fight
        /// </summary>
        /// <param name = "fight"></param>
        /// <returns></returns>
        public static bool RemoveFight(Fight fight)
        {
            fight.Dispose();

            return RemoveInstance(fight);
        }

        /// <summary>
        ///   Create the fight
        /// </summary>
        /// <param name = "fight"></param>
        /// <returns>fightId</returns>
        public static int CreateFight(Fight fight)
        {
            return CreateInstance(fight);
        }

        #region GetFight Methods

        public static Fight GetFightById(int fightId)
        {
            return GetInstanceById(fightId);
        }

        public static Fight GetFightByCharacterId(int chrId)
        {
            return (from entry in m_instances.Values
                    where entry.SourceGroup.GetMemberById(chrId) != null || entry.TargetGroup.GetMemberById(chrId) != null
                    select entry).FirstOrDefault();
        }

        public static Fight GetFightByGroupId(int groupId)
        {
            return ( from entry in m_instances.Values
                     where entry.SourceGroup.Id == groupId || entry.TargetGroup.Id == groupId
                     select entry ).FirstOrDefault();
        }

        #endregion
    }
}