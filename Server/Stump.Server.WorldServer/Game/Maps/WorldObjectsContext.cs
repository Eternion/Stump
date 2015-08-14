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
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes.Set;

namespace Stump.Server.WorldServer.Game.Maps
{
    public abstract class WorldObjectsContext
    {
        protected abstract IReadOnlyCollection<WorldObject> Objects
        {
            get;
        }

        public abstract Cell[] Cells
        {
            get;
        }

        public bool CanBeSeen(Cell from, Cell to, bool throughEntities = false)
        {
            return CanBeSeen(MapPoint.GetPoint(from), MapPoint.GetPoint(to), throughEntities);
        }

        public bool CanBeSeen(MapPoint from, MapPoint to, bool throughEntities = false)
        {
            if (from == null || to == null) return false;
            if (from == to) return true;
            
            var occupiedCells = new short[0];
            if (!throughEntities)
                occupiedCells = Objects.Where(x => x.BlockSight).Select(x => x.Cell.Id).ToArray();

            var line = new LineSet(from, to);
            foreach (var point in line.EnumerateSet().Skip(1)) // skip first cell
            {
                if (to.CellId == point.CellId)
                    continue;

                var cell = Cells[point.CellId];

                if (!cell.LineOfSight || !throughEntities && Array.IndexOf(occupiedCells, point.CellId) != -1)
                    return false;
            }

            return true;        }
    }
}