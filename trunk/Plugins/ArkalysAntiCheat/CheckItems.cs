using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game;

namespace ArkalysAntiCheat
{
    public class CheckItems
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Initialization(InitializationPass.Database)]
        public void Initialize()
        {
            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                {
                    var items = World.Instance.Database.Fetch<PlayerItemRecord>(string.Format(PlayerItemRelator.FetchQuery));

                    foreach (var item in items.Where(item => item.ItemId == 13210 || item.ItemId == 13211 || item.ItemId == 13212 || item.ItemId == 13213).Where(item => !item.Effects.Exists(x => x.EffectId == EffectsEnum.Effect_NonExchangeable_982)))
                    {
                        Logger.Info("Objiviant not account linked - Delete item {0} from player's inventory {1}", item.ToString(), item.OwnerId);
                        World.Instance.Database.Delete(item);
                    }

                    foreach (var item in items)
                    {
                        foreach (var effectLiving in item.Effects.Where(x => x.EffectId == EffectsEnum.Effect_LivingObjectId))
                        {
                            Logger.Info("Objiviant Effect && Not Account Linked - Remove Objiviant effect from player's inventory {0}", item.OwnerId);
                            item.Effects.Remove(effectLiving);
                        }
                    }
                });
        }
    }
}
