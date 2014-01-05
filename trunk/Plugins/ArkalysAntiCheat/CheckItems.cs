using System.Linq;
using NLog;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Items;

namespace ArkalysAntiCheat
{
    public static class CheckItems
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Initialization(InitializationPass.Fourth)]
        public static void Initialize()
        {
            var allItems = World.Instance.Database.Fetch<PlayerItemRecord>(string.Format(PlayerItemRelator.FetchQuery));

            foreach (var item in allItems.Where(x => x.ItemId == 20347 || x.ItemId == 20348 || x.ItemId == 20349 || x.ItemId == 20350
                || x.ItemId == 20351 || x.ItemId == 20352 || x.ItemId == 20353 || x.ItemId == 20354 || x.ItemId == 20355 || x.ItemId == 20356
                || x.ItemId == 20357 || x.ItemId == 20358 || x.ItemId == 20359 || x.ItemId == 20360 || x.ItemId == 20361 || x.ItemId == 20362
                || x.ItemId == 20364 || x.ItemId == 20366))
            {
                item.Effects = ItemManager.Instance.GenerateItemEffects(item.Template);
                World.Instance.Database.Update(item);

                Logger.Info("Update Item {0}", item.ItemId);
            }
        }
    }
}
