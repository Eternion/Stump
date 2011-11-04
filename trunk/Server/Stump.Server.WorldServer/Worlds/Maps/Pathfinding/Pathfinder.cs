using System;
using System.Linq;
using System.Collections.Generic;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

namespace Stump.Server.WorldServer.Worlds.Maps.Pathfinding
{
    internal struct PathNode
    {
        public short Cell;
        public double F;
        public double G;
        public double H;
        public short Parent;
        public NodeState Status;
    }

    internal enum NodeState : byte
    {
        None,
        Open,
        Closed
    }

    public class Pathfinder
    {
        [Variable(true)]
        public static int SearchLimit = 500;

        private static readonly int[] Directions = new[]
            {
                (int) MapPoint.MapWidth,
                (int) (MapPoint.MapWidth - 1),
                (int) (-MapPoint.MapWidth),
                (int) (-MapPoint.MapWidth + 1),
                1,
                (int) ((MapPoint.MapWidth*2) - 1),
                -1,
                (int) (-((MapPoint.MapWidth*2) + 1))
            };

        private static double GetHeuristic(MapPoint pointA, MapPoint pointB)
        {
            return Math.Abs(pointA.X - pointB.X) + Math.Abs(pointA.Y - pointB.Y);
        }

        private static double GetNeighborDistance(MapPoint pointA, MapPoint pointB)
        {
            return GetHeuristic(pointA, pointB);
        }

        public Pathfinder(ICellsInformationProvider cellsInformationProvider)
        {
            CellsInformationProvider = cellsInformationProvider;
        }

        public ICellsInformationProvider CellsInformationProvider
        {
            get;
            private set;
        }

        public Path FindPath(short startCell, short endCell, bool diagonal, int movementPoints = (short)-1)
        {
            var success = false;

            var matrix = new PathNode[MapPoint.MapSize + 1];
            var openList = new PriorityQueueB<short>(new ComparePfNodeMatrix(matrix));
            var closedList = new List<PathNode>();

            var startPoint = new MapPoint(startCell);
            var endPoint = new MapPoint(endCell);

            var location = startCell;
            var locationPoint = new MapPoint(location);

            var counter = 0;
            var usedMP = 0;

            matrix[location].Cell = location;
            matrix[location].G = 0;
            matrix[location].H = GetHeuristic(startPoint, endPoint); 
            matrix[location].F = matrix[location].H;
            matrix[location].Parent = -1;
            matrix[location].Status = NodeState.Open;

            openList.Push(location);
            while (openList.Count > 0)
            {
                location = openList.Pop();

                if (matrix[location].Status == NodeState.Closed)
                    continue;

                if (location == endCell)
                {
                    matrix[location].Status = NodeState.Closed;
                    success = true;
                    break;
                }

                if (counter > SearchLimit)
                    return Path.GetEmptyPath(CellsInformationProvider.Map, CellsInformationProvider.Map.Cells[startCell]);

                for (int i = 0; i < (diagonal ? 8 : 4); i++)
                {
                    var newLocation = (short) (location + Directions[i]);
                    var newLocationPoint = new MapPoint(newLocation);

                    if (!MapPoint.IsInMap(newLocationPoint.X, newLocationPoint.Y))
                        continue;

                    if (newLocation >= MapPoint.MapSize)
                        continue;

                    if (!CellsInformationProvider.IsCellWalkable(newLocation))
                        continue;

                    double newG = matrix[location].G + GetNeighborDistance(locationPoint, newLocationPoint);

                    if (( matrix[newLocation].Status == NodeState.Open ||
                        matrix[newLocation].Status == NodeState.Closed ) &&
                        matrix[newLocation].G <= newG)
                        continue;

                    matrix[newLocation].Cell = newLocation;
                    matrix[newLocation].Parent = location;
                    matrix[newLocation].G = newG;
                    matrix[newLocation].H = GetHeuristic(newLocationPoint, endPoint);
                    matrix[newLocation].F = matrix[newLocation].G + matrix[newLocation].H;
                    openList.Push(newLocation);

                    matrix[newLocation].Status = NodeState.Open;
            }

                counter++;
                usedMP++;
                matrix[location].Status = NodeState.Closed;

                if (usedMP >= movementPoints)
                {
                    success = true;
                    endCell = location;
                    break;
                }
            }

            if (success)
            {
                PathNode node = matrix[endCell];

                while (node.Parent != -1)
                {
                    closedList.Add(node);

                    node = matrix[node.Parent];
                }

                closedList.Add(node);
            }

            closedList.Reverse();

            return new Path(CellsInformationProvider.Map, closedList.Select(entry => CellsInformationProvider.Map.Cells[entry.Cell]));
        }

        #region Nested type: ComparePfNodeMatrix

        internal class ComparePfNodeMatrix : IComparer<short>
        {
            private readonly PathNode[] m_matrix;

            public ComparePfNodeMatrix(PathNode[] matrix)
            {
                m_matrix = matrix;
            }

            #region IComparer<ushort> Members

            public int Compare(short a, short b)
            {
                if (m_matrix[a].F > m_matrix[b].F)
                {
                    return 1;
                }

                if (m_matrix[a].F < m_matrix[b].F)
                {
                    return -1;
                }
                return 0;
            }

            #endregion
        }

        #endregion
    }

}