
using System.Collections.Generic;
using System.IO;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.IO;
using Stump.BaseCore.Framework.Xml;
using Stump.DofusProtocol.Messages.Framework.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.BaseServer.Data;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Data
{
    public static class MapLoader
    {
        /// <summary>
        ///   Name of maps folder
        /// </summary>
        [Variable]
        public static string MapsDir = "maps/";

        [Variable]
        public static string CellTriggersDir = "CellTriggers/";

        /// <summary>
        ///   Load ripped maps from maps directory
        /// </summary>
        public static IEnumerable<Map> LoadMaps()
        {
            foreach (
                string file in Directory.GetFiles(Settings.ContentPath + MapsDir, "*.map", SearchOption.AllDirectories))
            {
                var reader = new BigEndianReader(File.OpenRead(file));

                yield return reader.ReadMap();
            }
        }

        public static int GetMapFilesCount()
        {
            return Directory.GetFiles(Settings.ContentPath + MapsDir, "*.map", SearchOption.AllDirectories).Length;
        }

        public static IEnumerable<CellTrigger> LoadTriggers()
        {
            var directory = new DirectoryInfo(Settings.StaticPath + CellTriggersDir);

            foreach (FileInfo file in directory.GetFiles("*.xml", SearchOption.AllDirectories))
            {
                yield return XmlUtils.Deserialize<CellTrigger>(file.FullName);
            }
        }

        public static IDictionary<int, MapPosition> LoadMapPositions()
        {
            return DataLoader.LoadDataByIdAsDictionary<int, MapPosition>(entry => entry.id);
        }
    }
}