using System.Linq;
using NLog;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Items;

namespace ArkalysAntiCheat
{
    public static class CheckItems
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static readonly short[] checkItems =
        {
            20355,
            20347,
            20348,
            20349,
            20350,
            20361,
            20360,
            20362,
            20359,
            20615,
            20614,
            20379,
            20375,
            20619,
            20620,
            20374,
            20373,
            20372,
            6713,
            2156,
            20367
        };

        [Initialization(InitializationPass.Last)]
        public static void Initialize()
        {
            var allItems = World.Instance.Database.Fetch<PlayerItemRecord>(string.Format(PlayerItemRelator.FetchQuery));

            foreach (var item in allItems.Where(x => checkItems.Any(y => y == x.ItemId)))
            {
                item.Effects = ItemManager.Instance.GenerateItemEffects(item.Template);
                World.Instance.Database.Update(item);

                Logger.Info("Update Item {0}", item.ItemId);
            }
        }
    }
}
