#region License GNU GPL
// ActorsContext.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Maps
{
    public abstract class WorldObjectsContext
    {
        protected abstract IEnumerable<WorldObject> Objects
        {
            get;
        }

        public abstract Cell[] Cells
        {
            get;
        }

        /// <summary>
        /// Check if distance from C to the segment [AB] is less or very close to sqrt(2)/2 "units" 
        /// and the projection of C on the line (AB) is inside the segment [AB].
        /// This should give a conservative way to compute LOS. In very close cases 
        /// (where the exact implementation of the LOS algorithm could make the difference), then 
        /// we consider that the LOS is blocked. The safe way. 
        /// </summary>
        private bool TooCloseFromSegment(int cx, int cy, int ax, int ay, int bx, int by)
        {
            const double MIN_DISTANCE_SQUARED = 0.4999;

            // Distance computing is inspired by Philip Nicoletti algorithm - http://forums.codeguru.com/printthread.php?t=194400&pp=15&page=2     
            int numerator = ( cx - ax ) * ( bx - ax ) + ( cy - ay ) * ( by - ay );
            int denomenator = ( bx - ax ) * ( bx - ax ) + ( by - ay ) * ( by - ay );

            if (numerator > denomenator || numerator < 0)
                return false; //The projection of the point on the line is outside the segment, so it doesn't block the LOS

            double Base = ( ( ay - cy ) * ( bx - ax ) - ( ax - cx ) * ( by - ay ) );
            double distanceLineSquared = Base * Base / denomenator;
            return ( distanceLineSquared <= MIN_DISTANCE_SQUARED ); // if distance to line is frankly over sqrt(2)/2, it won't block LOS. 
        }

        // complexity : O(n) n - numbers of actors
        public bool CanBeSeenOld(Cell from, Cell to, bool throughEntities = false)
        {
            if (from == null || to == null) return false;
            if (from == to) return true;

            var occupiedCells = new short[0];
            if (!throughEntities)
                occupiedCells = Objects.Select(x => x.Cell.Id).ToArray();

            var fromPoint = MapPoint.GetPoint(from);
            var toPoint = MapPoint.GetPoint(to);

            foreach (var cell in fromPoint.GetAllCellsInRectangle(toPoint, true,
                cell => cell != null && (!Cells[cell.CellId].LineOfSight || ( !throughEntities && Array.IndexOf(occupiedCells, cell.CellId) != -1 ))))
                if (TooCloseFromSegment(cell.X, cell.Y, fromPoint.X, fromPoint.Y, toPoint.X, toPoint.Y))
                    return false;

            return true;
        }

        public bool CanBeSeen(Cell from, Cell to, bool throughEntities = false)
        {
            if (from == null || to == null) return false;
            if (from == to) return true;
            
            var occupiedCells = new short[0];
            if (!throughEntities)
                occupiedCells = Objects.Select(x => x.Cell.Id).ToArray();

            var line = MapPoint.GetPoint(from).GetCellsInLine(MapPoint.GetPoint(to));
            foreach (var point in line.Skip(1)) // skip first cell
            {
                if (to.Id == point.CellId)
                    continue;

                var cell = Cells[point.CellId];

                if (!cell.LineOfSight || !throughEntities && Array.IndexOf(occupiedCells, point.CellId) != -1)
                    return false;
            }

            return true;
        }
    }
}