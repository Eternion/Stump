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
using Stump.Server.BaseServer.Manager;

namespace Stump.Server.WorldServer.Groups
{
    public class GroupManager : InstanceManager<Group>
    {
        /// <summary>
        ///   Add the group
        /// </summary>
        /// <param name = "grp"></param>
        /// <returns>groupId</returns>
        public static int CreateGroup(Group grp)
        {
            return CreateInstance(grp);
        }

        public static bool RemoveGroup(Group grp)
        {
            return RemoveInstance(grp);
        }

        public static Group GetGroupOfId(int id)
        {
            return GetInstanceById(id);
        }
    }
}