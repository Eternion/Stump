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

            var bugs = World.Instance.GetMaps().Where(entry => entry.Record.Position == null).ToArray();

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

            var top = map.SuperArea.Maps.Where(entry => entry.Record.Position != null && entry.Position.X == pos.X && entry.Position.Y == pos.Y - 1 && entry.Outdoor == map.Outdoor).OrderByDescending(entry => entry.Cells.Count(cell => cell.Walkable)).FirstOrDefault();
            if (top != null)
            {
                map.TopNeighbourId = top.Id;
                builder.AppendFormat("TopNeighbourId='{0}', ", map.TopNeighbourId);
            }

            var bottom = map.SuperArea.Maps.Where(entry => entry.Record.Position != null &&  entry.Position.X == pos.X && entry.Position.Y == pos.Y + 1 && entry.Outdoor == map.Outdoor).OrderByDescending(entry => entry.Cells.Count(cell => cell.Walkable)).FirstOrDefault();
            if (bottom != null)
            {
                map.BottomNeighbourId = bottom.Id;
                builder.AppendFormat("BottomNeighbourId='{0}', ", map.BottomNeighbourId);
            }

            var right = map.SuperArea.Maps.Where(entry => entry.Record.Position != null &&  entry.Position.X == pos.X + 1 && entry.Position.Y == pos.Y && entry.Outdoor == map.Outdoor).OrderByDescending(entry => entry.Cells.Count(cell => cell.Walkable)).FirstOrDefault();
            if (right != null)
            {
                map.RightNeighbourId = right.Id;
                builder.AppendFormat("RightNeighbourId='{0}', ", map.RightNeighbourId);
            }

            var left = map.SuperArea.Maps.Where(entry => entry.Record.Position != null &&  entry.Position.X == pos.X - 1 && entry.Position.Y == pos.Y - 1 && entry.Outdoor == map.Outdoor).OrderByDescending(entry => entry.Cells.Count(cell => cell.Walkable)).FirstOrDefault();
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
    }
}