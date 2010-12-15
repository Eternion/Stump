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
        // todo : use a class like 'PathBuilder'
        void Move(List<uint> movementKeys);

        /// <summary>
        ///   Move the entity.
        /// </summary>
        void Move(VectorIso to);

        /// <summary>
        ///   Move the entity.
        /// </summary>
        void Move(ushort cellId);

        /// <summary>
        ///   Move the entity.
        /// </summary>
        void Move(ushort cellId, DirectionsEnum direction);

        /// <summary>
        ///   Teleport instantly the entity.
        /// </summary>
        /// <param name="to"></param>
        void MoveInstant(VectorIso to);

        /// <summary>
        ///   Teleport instantly the entity.
        /// </summary>
        void MoveInstant(ushort cellId);


        /// <summary>
        ///   Teleport instantly the entity.
        /// </summary>
        void MoveInstant(ushort cellId, DirectionsEnum direction);

        void MovementEnded();

        void StopMove(VectorIso currentVectorIso);
    }
}