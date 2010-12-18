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
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Pathfinding;

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
        }

        bool CanMove();

        /// <summary>
        ///   Move the entity.
        /// </summary>
        void Move(MovementPath movementPath);

        /// <summary>
        ///   Teleport instantly the entity.
        /// </summary>
        /// <param name="to"></param>
        void MoveInstant(VectorIsometric to);

        void MovementEnded();

        void StopMove(VectorIsometric currentVectorIsometric);
    }
}