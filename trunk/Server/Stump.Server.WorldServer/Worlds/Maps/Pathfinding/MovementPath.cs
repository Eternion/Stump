using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

namespace Stump.Server.WorldServer.Worlds.Maps.Pathfinding
{
    public class MovementPath
    {
        private List<ObjectPosition> m_uncompressPath;
        private bool m_compressed;
        private List<short> m_serverMovementKeys;

        public MovementPath(Map map)
        {
            Path = new List<ObjectPosition>();
            Map = map;
        }

        public MovementPath(Map map, IEnumerable<short> keysMovements)
        {
            Path = new List<ObjectPosition>();
            Map = map;

            BuildByClientMovementKeys(keysMovements);
        }

        public Map Map
        {
            get;
            private set;
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
            private set;
        }

        public int MpCost
        {
            get;
            private set;
        }

        public List<short> GetServerMovementKeys()
        {
            if (m_serverMovementKeys == null || m_serverMovementKeys.Count == 0)
            {
                m_serverMovementKeys = new List<short>();

                foreach (ObjectPosition position in Path)
                {
                    m_serverMovementKeys.Add(position.Cell.Id);
                }
            }

            return m_serverMovementKeys;
        }

        public void Compress()
        {
            if (!m_compressed)
            {
                m_uncompressPath = new List<ObjectPosition>(Path);

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
                var nextPoint = Path[i].Point;
                while (( nextPoint = nextPoint.GetNearestCellInDirection(Path[i].Direction) ) != null &&
                       nextPoint.CellId != Path[i + l + 1].Cell.Id)
                {
                    if (l > MapPoint.MapHeight * 2 + MapPoint.MapWidth)
                        throw new Exception("Path too long. Maybe an orientation problem ?");

                    if (Path.Count > i + l + 1)
                        Path.Insert(i + l + 1 , new ObjectPosition(Map, Map.Cells[nextPoint.CellId], Path[i].Direction));
                    else
                        Path.Add(new ObjectPosition(Map, Map.Cells[nextPoint.CellId], Path[i].Direction));

                    l++;
                }

                i++;
                i += l;
            }

            m_compressed = false;

            m_uncompressPath = new List<ObjectPosition>(Path);
            MpCost = Path.Count - 1;
        }

        public IEnumerable<short> GetCells()
        {
            return Path.Select(t => t.Point.CellId);
        }

        public bool Contains(short cellId)
        {
            return m_uncompressPath.Count(entry => entry.Cell.Id == cellId) > 0;
        }

        private void BuildByClientMovementKeys(IEnumerable<short> keys)
        {
            foreach (var pathElement in
                from key in keys
                let mapPoint = new MapPoint((short) (key & 4095))
                let direction = (DirectionsEnum) ((key >> 12) & 7)
                select new ObjectPosition(Map, Map.Cells[mapPoint.CellId], direction))
            {
                Path.Add(pathElement);
            }

            Fill();
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
    }
}