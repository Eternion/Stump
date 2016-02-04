using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DBSynchroniser.Records.Maps;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Core.IO;
using Stump.Core.Xml;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace DBSynchroniser.Maps.Placements
{
    public static class PlacementsFix
    {
        [Variable]
        public static string PatternsDir = "./patterns";

        [Variable]
        public static byte SearchDeep = 5;

        [Variable]
        public static bool SortByComplexity = true;

        public static void ApplyFix()
        {
            Console.WriteLine("Apply placements fix ...");

            Console.WriteLine("Override old placements ?(y/n)");
            bool @override = Console.ReadLine() == "y";

            //Program.Database.Database.Execute("UPDATE `world_maps` SET BlueCellsBin = null, RedCellsBin = null");

            var dir = PatternsDir;
            var patterns = new List<PlacementPattern>();
            var patternsNames = new Dictionary<PlacementPattern, string>();
            foreach (var file in Directory.EnumerateFiles(dir, "*.xml", SearchOption.AllDirectories))
            {
                try
                {
                    var pattern = XmlUtils.Deserialize<PlacementPattern>(file);

                    if (SortByComplexity)
                    {
                        var calc = new PlacementComplexityCalculator(pattern.Blues.Concat(pattern.Reds).ToArray());
                        pattern.Complexity = calc.Compute();
                    }

                    patterns.Add(pattern);
                    patternsNames.Add(pattern, Path.GetFileNameWithoutExtension(file));
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR:Cannot parse pattern {0}", file);
                }

            }

            if (patterns.Count == 0)
            {
                Console.WriteLine("ERROR:no pattern in {0} found", dir);
                return;
            }

            var successCounter = new Dictionary<PlacementPattern, int>();
            var console = new ConsoleProgress();
            var maps = Program.Database.Database.Fetch<MapRecord>(MapRelator.FetchQuery);
            int patches = 0;
            for (int i = 0; i < maps.Count; i++)
            {
                var map = maps[i];
                if (@override || (map.BlueFightCells.Length == 0 ||
                    map.RedFightCells.Length == 0))
                {

                    var fixPatternsComplx = patterns.Where(entry => !entry.Relativ).Select(entry => entry.Complexity).ToArray();
                    var fixPatterns = patterns.Where(entry => !entry.Relativ).ShuffleWithProbabilities(fixPatternsComplx).ToArray();
                    PlacementPattern success = fixPatterns.FirstOrDefault(entry => entry.TestPattern(map));

                    if (success != null)
                    {
                        map.BlueFightCells =
                            success.Blues.Select(entry => (short) MapPoint.CoordToCellId(entry.X, entry.Y)).ToArray();
                        map.RedFightCells =
                            success.Reds.Select(entry => (short) MapPoint.CoordToCellId(entry.X, entry.Y)).ToArray();
                    }
                    else
                    {
                        var relativePatternsComplx = patterns.Where(entry => entry.Relativ).Select(entry => entry.Complexity).ToArray();
                        var relativPatterns = patterns.Where(entry => entry.Relativ).ShuffleWithProbabilities(relativePatternsComplx).ToArray();

                        // 300 is approx. the middle of the map
                        foreach (var center in GetCellsCircle(map.Cells[300], map, SearchDeep, 0))
                        {
                            var centerPt = MapPoint.CellIdToCoord((uint) center.Id);
                            success = relativPatterns.FirstOrDefault(entry => entry.TestPattern(centerPt, map));

                            if (success != null)
                            {
                                map.BlueFightCells =
                                    success.Blues.Select(
                                        entry =>
                                        (short) MapPoint.CoordToCellId(entry.X + centerPt.X, entry.Y + centerPt.Y)).
                                        ToArray();
                                map.RedFightCells =
                                    success.Reds.Select(
                                        entry =>
                                        (short) MapPoint.CoordToCellId(entry.X + centerPt.X, entry.Y + centerPt.Y)).
                                        ToArray();
                                break;
                            }
                        }
                    }

                    // save it
                    if (success != null)
                    {
                        var database = Program.Database.Database;
                        database.Update(map);
                        patches++;
                    }
                }


                console.Update(string.Format("{0:0.0}% ({1} patchs)", i / (double)maps.Count * 100, patches));
            }

            Console.WriteLine("{0}/{1} ({2:0.0}%) patterns used :", successCounter.Count, patterns.Count, successCounter.Count / (double)patterns.Count * 100);
            foreach (var counter in successCounter)
            {
                Console.WriteLine("{0} :\t{1,12:0.0}%", patternsNames[counter.Key], counter.Value / (double)patches * 100);
            }

            Console.WriteLine("{0} on {1} maps fixed ({2:0.0}%)", patches, maps.Count, patches / (double)maps.Count * 100);

            Console.WriteLine("Maps placements fix applied");
        }

        private static Cell[] GetCellsCircle(Cell centerCell, MapRecord map, int radius, int minradius)
        {
            var centerPoint = new MapPoint(centerCell);
            var result = new List<Cell>();

            if (radius == 0)
            {
                if (minradius == 0)
                    result.Add(centerCell);

                return result.ToArray();
            }

            int x = (int) (centerPoint.X - radius);
            int y = 0;
            int i = 0;
            int j = 1;
            while (x <= centerPoint.X + radius)
            {
                y = -i;

                while (y <= i)
                {
                    if (minradius == 0 || Math.Abs(centerPoint.X - x) + Math.Abs(y) >= minradius)
                        AddCellIfValid(x, y + centerPoint.Y, map, result);

                    y++;
                }

                if (i == radius)
                {
                    j = -j;
                }

                i = i + j;
                x++;
            }

            return result.ToArray();
        }

        private static void AddCellIfValid(int x, int y, MapRecord map, IList<Cell> container)
        {
            if (!MapPoint.IsInMap(x, y))
                return;

            container.Add(map.Cells[MapPoint.CoordToCellId(x, y)]);
        }
    }
}