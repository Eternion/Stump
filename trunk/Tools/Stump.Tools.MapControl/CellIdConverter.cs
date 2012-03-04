using System.Collections.Generic;
using System.Drawing;

namespace Stump.Tools.MapControl
{
    public static class CellIdConverter
    {
        private static readonly Dictionary<int, Point[]> OrthogonalGridReference = new Dictionary<int, Point[]>();
        
        public static Point GetPlanLocation(this MapCell cell, MapControl mapControl)
        {
            return GetPlanLocation(cell, mapControl.MapHeight, mapControl.MapWidth);
        }

        public static Point GetPlanLocation(this MapCell cell, int mapHeight, int mapWidth)
        {
            var grid = GetReferenceGrid(mapHeight, mapWidth);

            return grid[cell.Id];
        }

        /// <summary>
        /// Initialize a static 2D plan that is used as reference to convert a cell to a (X,Y) point
        /// </summary>
        private static Point[] GetReferenceGrid(int mapHeight, int mapWidth)
        {
            int size = mapHeight * mapHeight * 2;

            if (OrthogonalGridReference.ContainsKey(size))
                return OrthogonalGridReference[size];

            var grid = new Point[size];
            OrthogonalGridReference.Add(size, grid);

            int posX = 0;
            int posY = 0;
            int cellCount = 0;

            for (int x = 0; x < mapHeight; x++)
            {
                for (int y = 0; y < mapWidth; y++)
                    grid[cellCount++] = new Point(posX + y, posY + y);

                posX++;

                for (int y = 0; y < mapWidth; y++)
                    grid[cellCount++] = new Point(posX + y, posY + y);

                posY--;
            }

            return grid;
        }
    }
}