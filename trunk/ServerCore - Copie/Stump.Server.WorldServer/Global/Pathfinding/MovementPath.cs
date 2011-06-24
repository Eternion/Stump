
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Global.Pathfinding
{
    public class MovementPath
    {
        private bool m_compressed;
        private List<uint> m_serverMovementKeys;

        public MovementPath(Map map)
        {
            Path = new List<ObjectPosition>();
            Map = map;
        }

        public MovementPath(Map map, List<uint> keysMovements)
        {
            Path = new List<ObjectPosition>();
            Map = map;

            BuildByClientMovementKeys(keysMovements);
        }

        public Map Map
        {
            get;
            set;
        }

        public ObjectPosition Start
        {
            get { return Path.FirstOrDefault(); }
        }

        public ObjectPosition End
        {
            get { return Path.LastOrDefault(); }
        }

        public List<ObjectPosition> Path
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

                foreach (ObjectPosition vectorIsometric in Path)
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
                    if (l > MapPoint.MapHeight * 2 + MapPoint.MapWidth)
                        throw new Exception("Path too long. Maybe an orientation problem ?");

                    if (Path.Count > i + l + 1)
                        Path.Insert(i + l + 1 , new ObjectPosition(nextPoint, Path[i].Direction));
                    else
                        Path.Add(new ObjectPosition(nextPoint, Path[i].Direction));

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

            foreach (ObjectPosition element in Path)
            {
                sb.Append("[" + element.Point.X + ", " + element.Point.Y + ", " + element.Direction + "] ");
            }
            return sb.ToString();
        }

        private void BuildByClientMovementKeys(List<uint> keys)
        {
            ObjectPosition last = null;

            for (int i = 0; i < keys.Count; i++)
            {
                var mapPoint = new MapPoint((ushort) (keys[i] & 4095));
                var pathElement = new ObjectPosition(mapPoint);

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