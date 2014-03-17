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
            var members = World.Instance.Database.Fetch<GuildMemberRecord>(string.Format(GuildMemberRelator.FetchQuery));
            var guilds = World.Instance.Database.Fetch<GuildRecord>(string.Format(GuildRelator.FetchQuery));

            foreach (var member in members.Where(member => GuildManager.Instance.TryGetGuild(member.GuildId) == null))
            {
                Logger.Info("Delete member {0}", member.CharacterId);
                World.Instance.Database.Delete(member);
            }
            /*var allItems = World.Instance.Database.Fetch<PlayerItemRecord>(string.Format(PlayerItemRelator.FetchQuery));

            foreach (var item in allItems.Where(x => x.ItemId == 20368 || x.ItemId == 20386 || x.ItemId == 20387 || x.ItemId == 20156
                || x.ItemId == 20160 || x.ItemId == 20382 || x.ItemId == 20383 || x.ItemId == 20384 || x.ItemId == 20385 || x.ItemId == 20379
                || x.ItemId == 20378 || x.ItemId == 20377 || x.ItemId == 20376 || x.ItemId == 20375 || x.ItemId == 20374 || x.ItemId == 20373
                || x.ItemId == 20372 || x.ItemId == 13485 || x.ItemId == 13486 || x.ItemId == 13483 || x.ItemId == 13484 || x.ItemId == 20154
                || x.ItemId == 20155 || x.ItemId == 20158 || x.ItemId == 20159 || x.ItemId == 20392 || x.ItemId == 10907 || x.ItemId == 20370
                || x.ItemId == 20126 || x.ItemId == 20024 || x.ItemId == 8575 || x.ItemId == 20203 || x.ItemId == 20204 || x.ItemId == 20205
                || x.ItemId == 20206 || x.ItemId == 20128 || x.ItemId == 20130 || x.ItemId == 20121 || x.ItemId == 20122 || x.ItemId == 20118
                || x.ItemId == 20317 || x.ItemId == 20127 || x.ItemId == 20280 || x.ItemId == 20318 || x.ItemId == 20120 || x.ItemId == 20119
                || x.ItemId == 20123 || x.ItemId == 20124 || x.ItemId == 20125 || x.ItemId == 11504 || x.ItemId == 11918 || x.ItemId == 11511))
            {
                item.Effects = ItemManager.Instance.GenerateItemEffects(item.Template);
                World.Instance.Database.Update(item);

                Logger.Info("Update Item {0}", item.ItemId);*/
        }
    }
}
