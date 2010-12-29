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
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Global.Pathfinding
{
    internal class MapMovementAdapter
    {
        public static List<uint> GetServerMovement(MovementPath movement)
        {
            var keys = new List<uint>();

            movement.Compress();

            foreach (var vectorIsometric in movement.Path)
            {
                keys.Add((uint)( ( (int)vectorIsometric.Direction & 7 ) << 12 | vectorIsometric.Point.CellId & 4095 ));
            }

            return keys;
        }

        public static MovementPath GetClientMovement(Map map, List<uint> keys)
        {
            VectorIsometric last = null;
            var movementPath = new MovementPath(map);

            for (int i = 0; i < keys.Count; i++)
            {
                var mapPoint = new MapPoint(map, (ushort) (keys[i] & 4095));
                var pathElement = new VectorIsometric(mapPoint);

                if (i > 0 && last != null)
                    last.Direction = last.Point.OrientationTo(pathElement.Point);

                movementPath.Path.Add(pathElement);
                last = pathElement;
            }

            // movementPath.fill();
            return movementPath;
        }
    }
}