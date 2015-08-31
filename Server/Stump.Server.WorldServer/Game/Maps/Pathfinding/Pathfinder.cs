using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Maps.Pathfinding
{
    public struct PathNode
    {
        public short Cell;
        public double F;
        public double G;
        public double H;
        public short Parent;
        public NodeState Status;
    }

    public enum NodeState : byte
    {
        None,
        Open,
        Closed
    }

    public class Pathfinder
    {
        [Variable(true)]
        public static int SearchLimit = 500;

        [Variable(true)]
        public static int EstimateHeuristic = 1;

        private static readonly DirectionsEnum[] Directions = new[]
            {
                DirectionsEnum.DIRECTION_SOUTH_WEST,
                DirectionsEnum.DIRECTION_NORTH_WEST,
                DirectionsEnum.DIRECTION_NORTH_EAST,
                DirectionsEnum.DIRECTION_SOUTH_EAST,
                DirectionsEnum.DIRECTION_SOUTH,
                DirectionsEnum.DIRECTION_WEST,
                DirectionsEnum.DIRECTION_NORTH,
                DirectionsEnum.DIRECTION_EAST
            };


        private static double GetHeuristic(MapPoint pointA, MapPoint pointB)
        {
            var dxy = new Point(Math.Abs(pointB.X - pointA.X), Math.Abs(pointB.Y - pointA.Y));
            var orthogonalValue = Math.Abs(dxy.X - dxy.Y);
            var diagonalValue = Math.Abs(((dxy.X + dxy.Y) -  orthogonalValue) / 2);

            return EstimateHeuristic * ( diagonalValue + orthogonalValue + dxy.X + dxy.Y );
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

        public Path FindPath(short startCell, short endCell, bool diagonal, int movementPoints = (short) -1)
        {
            return FindPath(new MapPoint(startCell), new MapPoint(endCell), diagonal, movementPoints);
        }

        public Path FindPath(MapPoint startPoint, MapPoint endPoint, bool diagonal, int movementPoints = (short)-1)
        {
            var success = false;

            var matrix = new PathNode[MapPoint.MapSize + 1];
            var openList = new PriorityQueueB<short>(new ComparePfNodeMatrix(matrix));
            var closedList = new List<PathNode>();

            var location = startPoint.CellId;

            var counter = 0;

            if (movementPoints == 0)
                return Path.GetEmptyPath(CellsInformationProvider.Map, CellsInformationProvider.Map.Cells[startPoint.CellId]);

            matrix[location].Cell = location;
            matrix[location].Parent = -1;
            matrix[location].G = 0;
            matrix[location].F = EstimateHeuristic;
            matrix[location].Status = NodeState.Open;

            openList.Push(location);
            while (openList.Count > 0)
            {
                location = openList.Pop();
                var locationPoint = new MapPoint(location);

                if (matrix[location].Status == NodeState.Closed)
                    continue;

                if (location == endPoint.CellId)
                {
                    matrix[location].Status = NodeState.Closed;
                    success = true;
                    break;
                }

                if (counter > SearchLimit)
                    return Path.GetEmptyPath(CellsInformationProvider.Map, CellsInformationProvider.Map.Cells[startPoint.CellId]);

                for (int i = 0; i < (diagonal ? 8 : 4); i++)
                {
                    var newLocationPoint = locationPoint.GetNearestCellInDirection(Directions[i]);

                    if (newLocationPoint == null)
                        continue;

                    var newLocation = newLocationPoint.CellId;

                    if (newLocation < 0 || newLocation >= MapPoint.MapSize)
                        continue;

                    if (!MapPoint.IsInMap(newLocationPoint.X, newLocationPoint.Y))
                        continue;

                    if (!CellsInformationProvider.IsCellWalkable(newLocation))
                        continue;

                    double newG = matrix[location].G + 1;

                    if ((matrix[newLocation].Status == NodeState.Open ||
                        matrix[newLocation].Status == NodeState.Closed) &&
                        matrix[newLocation].G <= newG)
                        continue;

                    matrix[newLocation].Cell = newLocation;
                    matrix[newLocation].Parent = location;
                    matrix[newLocation].G = newG;
                    matrix[newLocation].H = GetHeuristic(newLocationPoint, endPoint);
                    matrix[newLocation].F = newG + matrix[newLocation].H;

                    openList.Push(newLocation);
                    matrix[newLocation].Status = NodeState.Open;
            }

                counter++;
                matrix[location].Status = NodeState.Closed;
            }

            if (success)
            {
                var node = matrix[endPoint.CellId];

                while (node.Parent != -1)
                {
                    closedList.Add(node);
                    node = matrix[node.Parent];
                }

                closedList.Add(node);
            }

            closedList.Reverse();

            if (movementPoints > 0 && closedList.Count + 1> movementPoints)
                return new Path(CellsInformationProvider.Map, closedList.Take(movementPoints + 1).Select(entry => CellsInformationProvider.Map.Cells[entry.Cell]));

            return new Path(CellsInformationProvider.Map, closedList.Select(entry => CellsInformationProvider.Map.Cells[entry.Cell]));
        }

        public MapPoint[] FindReachableCells(MapPoint from, int distance)
        {
            var result = new List<MapPoint>();
            var matrix = new PathNode[MapPoint.MapSize + 1];
            var openList = new PriorityQueueB<short>(new ComparePfNodeMatrix(matrix));
            var location = from.CellId;
            var counter = 0;

            if (distance == 0)
                return new [] {new MapPoint(from.CellId)};

            matrix[location].Cell = location;
            matrix[location].Parent = -1;
            matrix[location].G = 0;
            matrix[location].F = 0;
            matrix[location].Status = NodeState.Open;

            openList.Push(location);
            while (openList.Count > 0)
            {
                location = openList.Pop();
                var locationPoint = new MapPoint(location);

                if (matrix[location].Status == NodeState.Closed)
                    continue;

                if (counter > SearchLimit)
                    break;

                for (int i = 0; i < 4; i++)
                {
                    var newLocationPoint = locationPoint.GetNearestCellInDirection(Directions[i]);

                    if (newLocationPoint == null)
                        continue;

                    var newLocation = newLocationPoint.CellId;

                    if (newLocation < 0 || newLocation >= MapPoint.MapSize)
                        continue;

                    if (!MapPoint.IsInMap(newLocationPoint.X, newLocationPoint.Y))
                        continue;

                    if (!CellsInformationProvider.IsCellWalkable(newLocation))
                        continue;

                    double newG = matrix[location].G + 1;

                    if ((matrix[newLocation].Status == NodeState.Open ||
                        matrix[newLocation].Status == NodeState.Closed) &&
                        matrix[newLocation].G <= newG)
                        continue;

                    matrix[newLocation].Cell = newLocation;
                    matrix[newLocation].Parent = location;
                    matrix[newLocation].G = newG;
                    matrix[newLocation].H = 0;
                    matrix[newLocation].F = newG + matrix[newLocation].H;

                    if (newG <= distance)
                    {
                        result.Add(newLocationPoint);
                        openList.Push(newLocation);
                        matrix[newLocation].Status = NodeState.Open;
                    }
                }

                counter++;
                matrix[location].Status = NodeState.Closed;
            }

            return result.ToArray();

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