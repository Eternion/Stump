using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Maps.Pathfinding
{
    public class Path
    {
        private Cell[] m_cellsPath;
        private MapPoint[] m_path;
        private ObjectPosition[] m_compressedPath;
        private ObjectPosition m_endPathPosition;

        public Path(Map map, IEnumerable<Cell> path)
        {
            Map = map;
            m_cellsPath = path.ToArray();
            m_path = m_cellsPath.Select(entry => new MapPoint(entry)).ToArray();
        }

        private Path(Map map, IEnumerable<ObjectPosition> compressedPath)
        {
            Map = map;
            m_compressedPath = compressedPath.ToArray();
            m_cellsPath = BuildCompletePath();
            m_path = m_cellsPath.Select(entry => new MapPoint(entry)).ToArray();
        }

        public bool Walk
        {
            get;
            private set;
        }

        public Map Map
        {
            get;
            private set;
        }

        public Cell StartCell
        {
            get { return m_cellsPath[0]; }
        }

        public Cell EndCell
        {
            get { return m_cellsPath[m_cellsPath.Length - 1]; }
        }

        public ObjectPosition EndPathPosition
        {
            get { return m_endPathPosition ?? (m_endPathPosition = new ObjectPosition(Map, EndCell, GetEndCellDirection())); }
        }

        public int MPCost
        {
            get { return m_cellsPath.Length - 1; }
        }

        public bool IsEmpty()
        {
            return m_cellsPath.Length == 0;
        }

        public Cell[] Cells
        {
            get { return m_cellsPath; }
            protected set { m_cellsPath = value;
                m_endPathPosition = null;
                m_path = m_cellsPath.Select(entry => new MapPoint(entry)).ToArray();
            }
        }

        public DirectionsEnum GetEndCellDirection()
        {
            if (m_cellsPath.Length <= 1)
                return DirectionsEnum.DIRECTION_EAST;

            if (m_compressedPath != null)
                return m_compressedPath.Last().Direction;

            return m_path[m_path.Length - 2].OrientationToAdjacent(m_path[m_path.Length - 1]);
        }

        public ObjectPosition[] GetCompressedPath()
        {
            return m_compressedPath ?? (m_compressedPath = BuildCompressedPath());
        }

        public Cell[] GetPath()
        {
            return m_cellsPath;
        }

        public bool Contains(short cellId)
        {
            return m_cellsPath.Any(entry => entry.Id == cellId);
        }

        public IEnumerable<short> GetServerPathKeys()
        {
            return m_cellsPath.Select(entry => entry.Id);
        }

        public void CutPath(int index, bool skip = false)
        {
            if (index >= m_cellsPath.Length || index < 0)
                return;

            m_cellsPath = skip ? m_cellsPath.Skip(index).ToArray() : m_cellsPath.Take(index).ToArray();

            m_path = m_cellsPath.Select(entry => new MapPoint(entry)).ToArray();

            m_endPathPosition = new ObjectPosition(Map, EndCell, GetEndCellDirection());
        }

        private ObjectPosition[] BuildCompressedPath()
        {
            if (m_cellsPath.Length <= 0)
                return new ObjectPosition[0];

            if (m_cellsPath.Length <= 1)
                return new [] { new ObjectPosition(Map, m_cellsPath[0]) };

            // build the path
            var path = new List<ObjectPosition>();
            for (var i = 1; i < m_cellsPath.Length; i++)
            {
                path.Add(new ObjectPosition(Map, m_cellsPath[i - 1], m_path[i - 1].OrientationToAdjacent(m_path[i])));
            }

            path.Add(new ObjectPosition(Map, m_cellsPath[m_cellsPath.Length - 1], path[path.Count - 1].Direction));

            // compress it
            if (path.Count <= 0)
                return path.ToArray();

            var i2 = path.Count - 2; // we don't touch to the last vector
            while (i2 > 0)
            {
                if (path[i2].Direction == path[i2 - 1].Direction)
                    path.RemoveAt(i2);
                i2--;
            }

            return path.ToArray();
        }

        private Cell[] BuildCompletePath()
        {
            var completePath = new List<Cell>();

            for (var i = 0; i < m_compressedPath.Length - 1; i++)
            {
                completePath.Add(m_compressedPath[i].Cell);

                var l = 0;
                var nextPoint = m_compressedPath[i].Point;
                while (( nextPoint = nextPoint.GetNearestCellInDirection(m_compressedPath[i].Direction) ) != null &&
                      nextPoint.CellId != m_compressedPath[i + 1].Cell.Id)
                {
                    if (l > MapPoint.MapHeight * 2 + MapPoint.MapWidth)
                        throw new Exception("Path too long. Maybe an orientation problem ?");

                    var cell = Map.Cells[nextPoint.CellId];

                    if (!Map.IsCellWalkable(cell)) //Verify Hack
                        return completePath.ToArray();

                    completePath.Add(cell);

                    l++;
                }
            }

            completePath.Add(m_compressedPath[m_compressedPath.Length - 1].Cell);

            return completePath.ToArray();
        }

        public static Path BuildFromCompressedPath(Map map, IEnumerable<short> keys)
        {
            var path = (from key in keys
                        let cellId = key & 4095
                        let direction = (DirectionsEnum) ((key >> 12) & 7)
                        select new ObjectPosition(map, map.Cells[cellId], direction));

            return new Path(map, path);
        }

        public static Path GetEmptyPath(Map map, Cell startCell)
        {
            return new Path(map, new [] { startCell });
        }

        public void SetWalk()
        {
            Walk = true;
        }
    }
}