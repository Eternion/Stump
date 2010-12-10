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
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.Entities
{
    public interface IMovable : IEntity
    {
        /// <summary>
        ///   Indicate or set if entity is moving.
        /// </summary>
        bool IsMoving
        {
            get;
            set;
        }

        /// <summary>
        ///   Move the entity.
        /// </summary>
        void Move();

        /// <summary>
        ///   Make entity jump to a given location
        /// </summary>
        /// <param name = "to"></param>
        void Jump(Location to);

        /// <summary>
        ///   Make character stop his movement or not.
        /// </summary>
        /// <param name = "b"></param>
        void Stop(bool b);
    }
}