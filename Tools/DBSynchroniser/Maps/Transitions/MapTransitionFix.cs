using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Maps;

namespace DBSynchroniser.Maps.Transitions
{
    public static class MapTransitionFix
    {
        public static void ApplyFix()
        {
            var worldDb = Program.ConnectToWorld();

            Console.WriteLine("Load maps ...");
            World.Instance.ChangeDataSource(worldDb.Database);
            World.Instance.LoadSpaces();
            var console = new ConsoleProgress();
            
            Console.WriteLine("Apply fix ...");
            var maps = World.Instance.GetMaps().ToArray();
            var counter = 0;
            var patches = 0;
            foreach (var map in maps)
            {
                FixMap(map);
                worldDb.Database.Update(map.Record);
              
                counter++;

                console.Update(string.Format("{0:0.0}% " + "({1} patches)", ( counter / (double)maps.Length ) * 100, patches));
            }
            console.End();
            Console.WriteLine("Maps transitions fix applied");
        }
        public static void FixMap(Map map)
        {
            if (map.Record.Position == null)
                return;
            
            var pos = map.Position;

            var top = FindMaps(map, pos.X, pos.Y - 1).OrderByDescending(entry => entry.Cells.Count(cell => cell.Walkable)).FirstOrDefault();
            if (top != null)
            {
                map.TopNeighbourId = top.Id;
            }
            else
            {
                map.TopNeighbourId = -1;
            }

            var bottom = FindMaps(map, pos.X, pos.Y + 1).OrderByDescending(entry => entry.Cells.Count(cell => cell.Walkable)).FirstOrDefault();
            if (bottom != null)
            {
                map.BottomNeighbourId = bottom.Id;
            }
            else
            {
                map.BottomNeighbourId = -1;
            }

            var right = FindMaps(map, pos.X + 1, pos.Y).OrderByDescending(entry => entry.Cells.Count(cell => cell.Walkable)).FirstOrDefault();
            if (right != null)
            {
                map.RightNeighbourId = right.Id;
            }
            else
            {
                map.RightNeighbourId = -1;
            }

            var left = FindMaps(map, pos.X - 1, pos.Y).OrderByDescending(entry => entry.Cells.Count(cell => cell.Walkable)).FirstOrDefault();
            if (left != null)
            {
                map.LeftNeighbourId = left.Id;
            }
            else
            {
                map.LeftNeighbourId = -1;
            }
        }

        private static IEnumerable<Map> FindMaps(Map adjacent, int x, int y)
        {
            var maps = FindMaps(adjacent, x, y, adjacent.Outdoor);

            return maps.Length == 0 ? FindMaps(adjacent, x, y, !adjacent.Outdoor) : maps;
        }

        private static Map[] FindMaps(Map adjacent, int x, int y, bool outdoor)
        {
            var maps = adjacent.SubArea.GetMaps(x, y, outdoor);
            if (maps.Length > 0)
                return maps;

            maps = adjacent.Area.GetMaps(x, y, outdoor);
            if (maps.Length > 0)
                return maps;

            maps = adjacent.SuperArea.GetMaps(x, y, outdoor);
            return maps.Length > 0 ? maps : new Map[0];
        }
    }
}