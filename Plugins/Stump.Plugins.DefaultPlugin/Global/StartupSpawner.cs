using System.Linq;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Maps.Spawns;

namespace Stump.Plugins.DefaultPlugin.Global
{
    public static class StartupSpawner
    {
        [Initialization(typeof(World), Silent=true)]
        public static void SpawnAllMaps()
        {
            foreach (var map in World.Instance.GetMaps())
            {
                foreach(var pool in map.SpawningPools.OfType<ClassicalSpawningPool>())
                {
                    while (pool.SpawnNextGroup())
                        ;
                }
            }
        }
    }
}