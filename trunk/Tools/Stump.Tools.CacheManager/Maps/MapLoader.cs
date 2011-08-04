using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using Stump.Core.Extensions;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.World.Maps.Cells;
using Stump.Tools.CacheManager.SQL;

namespace Stump.Tools.CacheManager.Maps
{
    public static class MapLoader
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static void LoadMaps(string mapFolder)
        {
            logger.Info("Build table 'maps' ...");

            Program.DBAccessor.ExecuteNonQuery(SqlBuilder.BuildDelete("maps"));

            using (var d2pFile = new PakFile(Path.Combine(mapFolder, "maps0.d2p")))
            {
                string[] files = d2pFile.GetFilesName();
                int counter = 0;
                int cursorLeft = Console.CursorLeft;
                int cursorTop = Console.CursorTop;
                foreach (var file in files)
                {
                    var data = d2pFile.ReadFile(file);

                    var uncompressedMap = new MemoryStream();
                    ZipHelper.Uncompress(new MemoryStream(data), uncompressedMap);
                    uncompressedMap.Seek(0, SeekOrigin.Begin);

                    var values = ReadMap(new BigEndianReader(uncompressedMap));
                    Program.DBAccessor.ExecuteNonQuery(SqlBuilder.BuildInsertInto("maps", values));

                    counter++;

                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write("{0}/{1} ({2}%)", counter, files.Length, (int)((counter / (double)files.Length) * 100d));
                }
                Console.SetCursorPosition(cursorLeft, cursorTop);
            }
        }

        private static Dictionary<string, object> ReadMap(this BigEndianReader reader)
        {
            var values = new Dictionary<string, object>();

            int header = reader.ReadByte();

            if (header != 77)
                throw new FileLoadException("Wrong header file");

            byte version = reader.ReadByte();
            values.Add("Version", version);
            values.Add("Id",  reader.ReadInt());
            values.Add("RelativeId",  reader.ReadUInt());
            values.Add("MapType",  reader.ReadByte());
            values.Add("SubAreaId",  reader.ReadInt());
            values.Add("TopNeighbourId",  reader.ReadInt());
            values.Add("BottomNeighbourId",  reader.ReadInt());
            values.Add("LeftNeighbourId",  reader.ReadInt());
            values.Add("RightNeighbourId",  reader.ReadInt());
            values.Add("ShadowBonusOnEntities",  reader.ReadInt());

            if (version >= 3)
            {
                // background color
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
            }
            if (version >= 4)
            {
                /*this.ZOOM_SCALE = */
                reader.ReadUShort(); // 100 
                /*this.ZOOM_OFFSET_X = */
                reader.ReadShort();
                /*this.ZOOM_OFFSET_Y = */
                reader.ReadShort();
            }

            values.Add("UseLowpassFilter",  reader.ReadByte());
            byte usereverb = reader.ReadByte();
            values.Add("UseReverb", usereverb);

            values.Add("PresetId", usereverb == 1 ? reader.ReadInt() : -1);

            // fixtures background
            int count = reader.ReadByte();
            for (int i = 0; i < count; i++)
            {
                // new fixture
                reader.ReadInt();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
            }

            // fixtures foreground
            count = reader.ReadByte();
            for (int i = 0; i < count; i++)
            {
                // new fixture
                reader.ReadInt();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
            }

            reader.ReadInt();

            reader.ReadInt(); // ground

            // layers
            count = reader.ReadByte();
            for (int i = 0; i < count; i++)
            {
                // new layer
                reader.ReadInt(); // id
                short cellscount = reader.ReadShort(); // count
                for (int l = 0; l < cellscount; l++)
                {
                    reader.ReadShort(); // cellid
                    short elemcount = reader.ReadShort(); // count
                    for (int k = 0; k < elemcount; k++)
                    {
                        switch (reader.ReadByte())
                        {
                            case 2: // GRAPICAL
                                reader.ReadUInt();
                                reader.ReadByte();
                                reader.ReadByte();
                                reader.ReadByte();
                                reader.ReadByte();
                                reader.ReadByte();
                                reader.ReadByte();
                                reader.ReadByte();
                                reader.ReadByte();
                                reader.ReadByte();
                                reader.ReadUInt();
                                break;
                            case 33: // SOUND
                                reader.ReadInt();
                                reader.ReadShort();
                                reader.ReadInt();
                                reader.ReadInt();
                                reader.ReadShort();
                                reader.ReadShort();
                                break;
                        }
                    }
                }
            }

            var cells = new Cell[MapPoint.MapSize];
            for (short i = 0; i < MapPoint.MapSize; i++)
            {
                var cell = new Cell
                    {
                        Id = i,
                        Floor = (short) (reader.ReadByte()*10)
                    };

                if (cell.Floor == -1280)
                    continue;

                cell.LosMov = reader.ReadByte();
                cell.Speed = reader.ReadByte();
                cell.MapChangeData = reader.ReadByte();

                cells[i] = cell;
            }

            values.Add("Cells",  cells.ToBinary());

            return values;
        }
    }
}