using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using Stump.Core.Extensions;
using Stump.Core.IO;
using Stump.Core.Sql;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.DofusProtocol.D2oClasses.Tool.D2p;
using Stump.DofusProtocol.D2oClasses.Tool.Dlm;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Database.World.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Tools.CacheManager.SQL;

namespace Stump.Tools.CacheManager.Maps
{
    public static class MapLoader
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public const string DecryptionKey = "649ae451ca33ec53bbcbcc33becf15f4";

        public static void LoadMaps(string mapFolder)
        {
            logger.Info("Build table 'maps' ...");

            Program.DBAccessor.ExecuteNonQuery(SqlBuilder.BuildDelete("maps"));

            using (var d2pFile = new D2pFile(Path.Combine(mapFolder, "maps0.d2p")))
            {
                var files = d2pFile.ReadAllFiles();
                int counter = 0;
                int cursorLeft = Console.CursorLeft;
                int cursorTop = Console.CursorTop;
                foreach (var file in files)
                {
                    var data = file.Value;

                    var reader = new DlmReader(new MemoryStream(data), DecryptionKey);
                    DlmMap map;
                    try
                    {
                        map = reader.ReadMap();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Cannot evaluate map {0}", file.Key.FullFileName);
                        continue;
                    }
                    var values = BuildFromMap(map);

                    var listKey = new KeyValueListBase("maps", values);
                    Program.DBAccessor.ExecuteNonQuery(SqlBuilder.BuildInsert(listKey));

                    counter++;

                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write("{0}/{1} ({2}%)", counter, files.Count, (int)( ( counter / (double)files.Count ) * 100d ));
                }
                Console.SetCursorPosition(cursorLeft, cursorTop);
            }
        }

        private static Dictionary<string, object> BuildFromMap(DlmMap map)
        {
            var values = new Dictionary<string, object>();

            values.Add("Version", map.Version);
            values.Add("Id",  map.Id);
            values.Add("RelativeId",  map.RelativeId);
            values.Add("MapType",  map.MapType);
            values.Add("SubAreaId",  map.SubAreaId);
            values.Add("ClientTopNeighbourId",  map.TopNeighbourId);
            values.Add("ClientBottomNeighbourId", map.BottomNeighbourId);
            values.Add("ClientLeftNeighbourId", map.LeftNeighbourId);
            values.Add("ClientRightNeighbourId", map.RightNeighbourId);
            values.Add("ShadowBonusOnEntities",  map.ShadowBonusOnEntities);


            values.Add("UseLowpassFilter",  map.UseLowPassFilter ? 1 : 0);
            values.Add("UseReverb", map.UseReverb ? 1 : -1);

            values.Add("PresetId", map.PresetId);

            var elements = new List<MapElement>();
            foreach (var layer in map.Layers)
            {
                foreach (var cell in layer.Cells)
                {
                    foreach (var element in cell.Elements.OfType<DlmGraphicalElement>())
                    {
                        elements.Add(new MapElement(element.Identifier, cell.Id));  
                    }
                }
            }

            var cells = new Cell[MapPoint.MapSize];
            foreach (var cellData in map.Cells)
            {
                var cell = new Cell
                {
                    Id = cellData.Id,
                    Floor = cellData.Floor,
                    LosMov = cellData.LosMov,
                    Speed = cellData.Speed,
                    MapChangeData = cellData.MapChangeData,
                    MoveZone = cellData.MoveZone
                };

                cells[cellData.Id] = cell;
                
            }

            var compressedCells = new byte[cells.Length * Cell.StructSize];

            for (int i = 0; i < cells.Length; i++)
            {
                Array.Copy(cells[i].Serialize(), 0, compressedCells, i * Cell.StructSize, Cell.StructSize);
            }

            compressedCells = ZipHelper.Compress(compressedCells);

            values.Add("CompressedCells",  new RawData("0x" + compressedCells.ByteArrayToString()));

            var rawElements = new byte[elements.Count * MapElement.Size];

            for (int i = 0; i < elements.Count; i++)
            {
                Array.Copy(elements[i].Serialize(), 0, rawElements, i * MapElement.Size, MapElement.Size);
            }

            values.Add("CompressedElements", new RawData("0x" + ZipHelper.Compress(rawElements).ByteArrayToString()));

            return values;
        }
    }
}