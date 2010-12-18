using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Global
{
    class MovementPath
    {
        private MapPoint m_start = new MapPoint();
        public MapPoint Start
        {
            get { return m_start; }
            set { m_start = value; }
        }

        private MapPoint m_end = new MapPoint();
        public MapPoint End
        {
            get { return m_end; }
            set { m_end = value; }
        }

        private List<PathElement> m_path = new List<PathElement>(100);
        public List<PathElement> Path
        {
            get { return m_path; }
            set { m_path = value; }
        }

        public void Compress()
        {
            int i = 0;
            if (m_path.Count > 0)
            {
                i = m_path.Count - 1;
                while (i > 0)
                {
                    if (m_path[i].Orientation == m_path[i - 1].Orientation)
                        m_path.RemoveAt(i);
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

        public List<int> getCells()
        {
            List<int> cells = new List<int>(m_path.Count);
            for (int i = 0; i < m_path.Count; i++)
                cells.Add(m_path[i].Step.CellId);

            cells.Add(m_end.CellId);
            return cells;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Depart : [" + m_start.X + ", " + m_start.Y + "]");
            sb.AppendLine("Arrivée : [" + m_end.X + ", " + m_end.Y + "]");
            sb.AppendLine("Chemin :");
            for (int i = 0; i < m_path.Count; i++)
            {
                sb.AppendLine("[" + m_path[i].Step.X + ", " + m_path[i].Step.Y + ", " + m_path[i].Orientation + "]");
            }
            return sb.ToString();
        }

    }
}
