using System.Linq;
using System.Runtime.CompilerServices;
using NLog;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Game.Items;

namespace ArkalysAntiCheat
{
    public static class CheckItems
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Initialization(InitializationPass.Last)]
        public static void Initialize()
        {
            var allItems = World.Instance.Database.Fetch<PlayerItemRecord>(string.Format(PlayerItemRelator.FetchQuery));

            foreach (
                var item in
                    allItems.Where(x => x.ItemId == 11504 || x.ItemId == 11918 || x.ItemId == 11511 || x.ItemId == 20371
                                        || x.ItemId == 20370 || x.ItemId == 20369 || x.ItemId == 20128 ||
                                        x.ItemId == 20392 || x.ItemId == 20077 || x.ItemId == 12964
                                        || x.ItemId == 12542 || x.ItemId == 13604 || x.ItemId == 13476 ||
                                        x.ItemId == 20303 || x.ItemId == 20304 || x.ItemId == 20305
                                        || x.ItemId == 20306 || x.ItemId == 20307 || x.ItemId == 20308 ||
                                        x.ItemId == 20317 || x.ItemId == 20396 || x.ItemId == 20128
                                        || x.ItemId == 20118 || x.ItemId == 20121 || x.ItemId == 679 || x.ItemId == 680 ||
                                        x.ItemId == 678))
            {
                item.Effects = ItemManager.Instance.GenerateItemEffects(item.Template);
                World.Instance.Database.Update(item);

                Logger.Info("Update Item {0}", item.ItemId);
            }
        }
    }
}
