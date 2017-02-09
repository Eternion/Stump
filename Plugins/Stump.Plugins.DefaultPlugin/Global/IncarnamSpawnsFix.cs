using System.Linq;
using NLog;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Maps.Spawns;

namespace Stump.Plugins.DefaultPlugin.Global
{
    public static class IncarnamSpawnsFix
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private const int INCARNAM_SUPERAREA_ID = 3;

        [Initialization(typeof(Server.WorldServer.Game.World), Silent = true)]
        public static void ApplyFix()
        {
            logger.Debug("Apply incarnam spawns fix");

            var area = Server.WorldServer.Game.World.Instance.GetSuperArea(INCARNAM_SUPERAREA_ID);

            if (area == null)
            {
                logger.Debug("Fix not applied");
                return;
            }

            foreach (var map in area.Maps)
            {
                map.DisableClassicalMonsterSpawns();
                foreach (var pool in map.SpawningPools.OfType<ClassicalSpawningPool>().ToArray())
                    map.RemoveSpawningPool(pool);

                map.AddSpawningPool(new IncarnamSpawningPool(map, map.SubArea.GetMonsterSpawnInterval()));
            }
        }
    }
}