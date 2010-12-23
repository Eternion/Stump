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
using System.Text;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Global.Pathfinding
{
    public class MovementPath
    {
        public Map Map
        {
            get;
            set;
        }

        public VectorIsometric Start
        {
            get
            {
                return Path.FirstOrDefault();
            }
        }

        public VectorIsometric End
        {
            get
            {
                return Path.LastOrDefault();
            }
        }

        public MovementPath(Map map)
        {
            Path = new List<VectorIsometric>(100);
            Map = map;
        }

        public List<VectorIsometric> Path
        {
            get;
            set;
        }

        public void Compress()
        {
            if (Path.Count > 0)
            {
                var i = Path.Count - 2; // we don't touch the last vector
                while (i > 0)
                {
                    if (Path[i].Direction == Path[i - 1].Direction)
                        Path.RemoveAt(i);
                    i--;
                }
            }
        }
       
        public void Fill()
        {
            //int i = 0;
            //PathElement _loc_2;
            //PathElement _loc_3;
            //if (m_path.Count > 0)
            //{
            //    _loc_2 = new PathElement();
            //    _loc_2.Orientation = 0;
            //    _loc_2.Step = m_end;
            //    m_path.Add(_loc_2);
            //    while (i < m_path.Count - 1)
            //    {
            //        if (i > MapPoint.MAP_HEIGHT * 2 + MapPoint.MAP_WIDTH)
            //            throw new Exception("Path too long. Maybe an orientation problem?");
            //    }
            //}
            // m_path.pop();
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

            foreach (var element in Path)
            {
                sb.Append("[" + element.Point.X + ", " + element.Point.Y + ", " + element.Direction + "] ");
            }
            return sb.ToString();
        }

    }
}
