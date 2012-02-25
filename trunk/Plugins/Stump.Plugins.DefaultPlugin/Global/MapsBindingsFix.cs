using System.IO;
using System.Linq;
using System.Text;
using NLog;
using Stump.Core.IO;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Plugins.DefaultPlugin.Global
{
    public class MapsBindingsFix
    {
        public const bool ActiveFix = false;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Initialization(typeof(World), Silent = true)]
        public static void ApplyFix()
        {
            if (!ActiveFix)
                return;

            logger.Debug("Apply maps bindings fix");

            if (File.Exists("./maps_bindings_fix.sql"))
                File.Delete("./maps_bindings_fix.sql");
            var console = new ConsoleProcent();
            var maps = World.Instance.GetMaps().ToArray();
            int counter = 0;
            int patches = 0;
            foreach (var map in maps)
            {
                var request = FixMap(map);

                if (request != string.Empty)
                {
                    File.AppendAllText("./maps_bindings_fix.sql", request + "\r\n");
                    patches++;
                }

                counter++;

                console.Update(string.Format("{0:0.0}% " + "({1} patches)", ( counter / (double)maps.Length ) * 100, patches));
            }
        }

        public static string FixMap(Map map)
        {
            if (map.Record.Position == null)
                return string.Empty;

            var builder = new StringBuilder();
            builder.Append("UPDATE `maps` SET ");
            var pos = map.Position;

            var top = FindMaps(map, pos.X, pos.Y + 1, map.Outdoor).OrderByDescending(entry => entry.Cells.Count(cell => cell.Walkable)).FirstOrDefault();
            if (top != null)
            {
                map.TopNeighbourId = top.Id;
                builder.AppendFormat("TopNeighbourId='{0}', ", map.TopNeighbourId);
            }

            var bottom = FindMaps(map, pos.X, pos.Y - 1, map.Outdoor).OrderByDescending(entry => entry.Cells.Count(cell => cell.Walkable)).FirstOrDefault();
            if (bottom != null)
            {
                map.BottomNeighbourId = bottom.Id;
                builder.AppendFormat("BottomNeighbourId='{0}', ", map.BottomNeighbourId);
            }

            var right = FindMaps(map, pos.X + 1, pos.Y, map.Outdoor).OrderByDescending(entry => entry.Cells.Count(cell => cell.Walkable)).FirstOrDefault();
            if (right != null)
            {
                map.RightNeighbourId = right.Id;
                builder.AppendFormat("RightNeighbourId='{0}', ", map.RightNeighbourId);
            }

            var left = FindMaps(map, pos.X - 1, pos.Y, map.Outdoor).OrderByDescending(entry => entry.Cells.Count(cell => cell.Walkable)).FirstOrDefault();
            if (left != null)
            {
                map.LeftNeighbourId = left.Id;
                builder.AppendFormat("LeftNeighbourId='{0}', ", map.LeftNeighbourId);
            }

            if (top != null || bottom != null || right != null || left != null)
            {
                builder.Remove(builder.Length - 2, 2); // remove ", "
                builder.AppendFormat(" WHERE Id='{0}'", map.Id);
                builder.Append(";");
                return builder.ToString();
            }
            else
            {
                return string.Empty;
            }
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
            if (maps.Length > 0)
                return maps; 
            
            return new Map[0];
        }
    }
}