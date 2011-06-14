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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.World.Position;
using Stump.Server.WorldServer.World.Zones;

namespace Stump.Server.WorldServer.Global.Pathfinding
{
    public class MovementPath
    {
        private bool m_compressed;
        private List<uint> m_serverMovementKeys;

        public MovementPath(Map map)
        {
            Path = new List<VectorIsometric>();
            Map = map;
        }

        public MovementPath(Map map, List<uint> keysMovements)
        {
            Path = new List<VectorIsometric>();
            Map = map;

            BuildByClientMovementKeys(keysMovements);
        }

        public Map Map
        {
            get;
            set;
        }

        public VectorIsometric Start
        {
            get { return Path.FirstOrDefault(); }
        }

        public VectorIsometric End
        {
            get { return Path.LastOrDefault(); }
        }

        public List<VectorIsometric> Path
        {
            get;
            set;
        }

        public int MpCost
        {
            get
            {
                return Path.Count - 1; // we substract the first cell because this is the cell where the entity is
            }
        }

        public List<uint> GetServerMovementKeys()
        {
            if (m_serverMovementKeys == null || m_serverMovementKeys.Count == 0)
            {
                m_serverMovementKeys = new List<uint>();

                foreach (VectorIsometric vectorIsometric in Path)
                {
                    m_serverMovementKeys.Add(
                        (uint) (((int) vectorIsometric.Direction & 7) << 12 | vectorIsometric.Point.CellId & 4095));
                }
            }

            return m_serverMovementKeys;
        }

        public void Compress()
        {
            if (!m_compressed)
            {
                if (Path.Count > 0)
                {
                    int i = Path.Count - 2; // we don't touch to the last vector
                    while (i > 0)
                    {
                        if (Path[i].Direction == Path[i - 1].Direction)
                            Path.RemoveAt(i);
                        i--;
                    }
                }

                m_compressed = true;
            }
        }

        public void Fill()
        {
            int i = 0;
            while (i < Path.Count - 1)
            {
                int l = 0;
                MapPoint nextPoint = Path[i].Point;
                while (( nextPoint = nextPoint.GetNearestCellInDirection(Path[i].Direction) ) != null &&
                       nextPoint.CellId != Path[i + l + 1].Point.CellId)
                {
                    if (l > MapPoint.MAP_HEIGHT * 2 + MapPoint.MAP_WIDTH)
                        throw new Exception("Path too long. Maybe an orientation problem ?");

                    if (Path.Count > i + l + 1)
                        Path.Insert(i + l + 1 , new VectorIsometric(nextPoint, Path[i].Direction));
                    else
                        Path.Add(new VectorIsometric(nextPoint, Path[i].Direction));

                    l++;
                }

                i++;
                i += l;
            }

            m_compressed = false;
        }

        public IEnumerable<ushort> GetCells()
        {
            return Path.Select(t => t.Point.CellId);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Start : [" + Start.Point.X + ", " + Start.Point.Y + "], ");
            sb.AppendLine("End : [" + End.Point.X + ", " + End.Point.Y + "], ");
            sb.AppendLine("Path :");

            foreach (VectorIsometric element in Path)
            {
                sb.Append("[" + element.Point.X + ", " + element.Point.Y + ", " + element.Direction + "] ");
            }
            return sb.ToString();
        }

        private void BuildByClientMovementKeys(List<uint> keys)
        {
            VectorIsometric last = null;

            for (int i = 0; i < keys.Count; i++)
            {
                var mapPoint = new MapPoint(Map, (ushort) (keys[i] & 4095));
                var pathElement = new VectorIsometric(mapPoint);

                if (i > 0 && last != null)
                    last.Direction = last.Point.OrientationTo(pathElement.Point);

                Path.Add(pathElement);
                last = pathElement;
            }

            if (keys.Count > 2)
                End.Direction = Path[Path.Count - 2].Direction;

            Fill();
        }
    }
}