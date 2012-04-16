using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Stump.Plugins.DefaultPlugin.Global.Placements
{
    public class PlacementComplexityCalculator
    {
        private class PointsGroup
        {
            public PointsGroup(Point[] points, Point center)
            {
                Points = points;
                Center = center;
            }

            public Point[] Points;
            public Point Center;
        }

        private Point[] m_points;

        public PlacementComplexityCalculator(Point[] points)
        {
            m_points = points;
        }

        public int Compute()
        {
            var groups = GetPointsGroups();
            if (groups.Length == 0)
                return 0;

            var distanceSum = 0d;

            var exclusions = new List<PointsGroup>();
            foreach (var @group in groups)
            {
                distanceSum += groups.Where(entry => !exclusions.Contains(entry)).Sum(entry => DistanceTo(entry.Center, @group.Center));

                exclusions.Add(@group);
            }

            var distanceAverage = distanceSum / groups.Length;
            var counts = m_points.Length;

            return (int) (counts * distanceAverage + groups.Length * groups.Average(entry => entry.Points.Length));
        }

        private PointsGroup[] GetPointsGroups()
        {
            var result = new List<PointsGroup>();
            var exclusions = new List<Point>();

            foreach (var point in m_points)
            {
                if (exclusions.Contains(point))
                    continue;

                var adjacents = FindAllAdjacentsPoints(point, new List<Point>(new [] { point }));
                adjacents.Add(point);

                var group = adjacents.ToArray();

                if (group.Length > 0)
                {
                    exclusions.Add(point);
                    exclusions.AddRange(adjacents);
                    result.Add(new PointsGroup(group, GetCenter(group)));
                }
            } 

            return result.ToArray();
        }

        private List<Point> FindAllAdjacentsPoints(Point point, List<Point> exclusions)
        {
            var result = new List<Point>();

            foreach (var adjacentPoint in GetAdjacentPoints(point).Where(entry => m_points.Contains(entry)))
            {
                if (exclusions.Contains(adjacentPoint))
                    continue;

                exclusions.Add(adjacentPoint);
                result.Add(adjacentPoint);

                result.AddRange(FindAllAdjacentsPoints(adjacentPoint, exclusions));
            }

            return result;
        }

        private Point GetCenter(Point[] points)
        {
            return new Point(points.Sum(entry => entry.X) / points.Length, points.Sum(entry => entry.Y) / points.Length);
        }

        private double DistanceTo(Point ptA, Point ptB)
        {
            return Math.Sqrt(( ptB.X - ptA.X ) * ( ptB.X - ptA.X ) + ( ptB.Y - ptA.Y ) * ( ptB.Y - ptA.Y ));
        }

        private Point[] GetAdjacentPoints(Point point)
        {
            return new[] {
                              point + new Size(1, 0),
                              point + new Size(0, 1),
                              point + new Size(-1, 0),
                              point + new Size(0, -1),
                              point + new Size(1, 1),
                              point + new Size(-1, 1),
                              point + new Size(1, -1),
                              point + new Size(-1, -1),
                          };
        }
    }
}