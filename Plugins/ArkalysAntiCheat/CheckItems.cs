using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace ArkalysAntiCheat
{
    public static class CheckItems
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static readonly short[] checkItems =
        {
            20348,
            20353,
            20362,
            20356
        };

        [Initialization(InitializationPass.Last)]
        public static void Initialize()
        {
            var allItems = World.Instance.Database.Fetch<PlayerItemRecord>(string.Format(PlayerItemRelator.FetchQuery));

            foreach (var item in allItems.Where(x => checkItems.Any(y => y == x.ItemId)))
            {
                if (item.Effects.Any(x => x.EffectId == EffectsEnum.Effect_LivingObjectId))
                {
                    var m_livingObjectIdEffect = (EffectInteger)item.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectId);
                    var m_moodEffect = (EffectInteger)item.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectMood);
                    var m_selectedLevelEffect = (EffectInteger)item.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectSkin);
                    var m_experienceEffect = (EffectInteger)item.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectLevel);
                    var m_categoryEffect = (EffectInteger)item.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectCategory);

                    item.Effects = ItemManager.Instance.GenerateItemEffects(item.Template);

                    item.Effects.Add(m_livingObjectIdEffect);
                    item.Effects.Add(m_moodEffect);
                    item.Effects.Add(m_selectedLevelEffect);
                    item.Effects.Add(m_experienceEffect);
                    item.Effects.Add(m_categoryEffect);
                }
                else
                {
                    item.Effects = ItemManager.Instance.GenerateItemEffects(item.Template);
                }

                World.Instance.Database.Update(item);

                Logger.Info("Update Item {0}", item.ItemId);
            }
        }
    }
}
