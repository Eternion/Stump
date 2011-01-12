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
using System.Collections.Concurrent;

namespace Stump.Server.WorldServer.Entities
{
    public interface IEntityContainer
    {
        /// <summary>
        /// Thread Safe set containing characters.
        /// </summary>
        ConcurrentDictionary<long, Entity> Entities { get; }
        /// <summary>
        /// Find all entities contained in this set. 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Entity> FindAll();
        /// <summary>
        /// Get an entity with the given id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Entity Get(long id);

    }
}
