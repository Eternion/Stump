using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace ScriptsPlugin
{
    public static class CheckItems
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static readonly short[] checkItems =
        {
            20392
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

                if (item.ItemId == 20392)
                {
                    World.Instance.Database.Delete(item);

                    var template = ItemManager.Instance.TryGetTemplate(20000);

                    var orbes = World.Instance.Database.Fetch<PlayerItemRecord>(string.Format("SELECT * FROM characters_items WHERE OwnerId={0} AND ItemId={1}", item.OwnerId, 20000)).FirstOrDefault();

                    if (orbes == null)
                    {
                        orbes = new PlayerItemRecord
                        {
                            Id = PlayerItemRecord.PopNextId(),
                            Effects = ItemManager.Instance.GenerateItemEffects(template),
                            IsNew = true,
                            ItemId = 20000,
                            OwnerId = item.OwnerId,
                            Position = CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED,
                            Stack = 820000
                        };

                        World.Instance.Database.Insert(orbes);
                    }
                    else
                    {
                        orbes.Stack += 820000;

                        World.Instance.Database.Update(orbes);
                    }   
                    
                }
                else
                    World.Instance.Database.Update(item);

                Logger.Info("Update Item {0}", item.ItemId);
            }
        }
    }
}
