using System;
using System.IO;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Core.Sql;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace ArkalysPlugin
{
    public static class NpcShops
    {
        [Variable]
        public static readonly bool Active = true;
        
        [Variable]
        public static readonly SerializableDictionary<int, ItemTypeEnum> Sellers = new SerializableDictionary<int, ItemTypeEnum>()
            {
                {812, ItemTypeEnum.HAT},
                {1158, ItemTypeEnum.CLOAK},
                {790, ItemTypeEnum.RING},
                {952, ItemTypeEnum.AMULET},
                {794, ItemTypeEnum.BOOTS},
                {787, ItemTypeEnum.BELT},
                {1053, ItemTypeEnum.DOFUS},                
                {61, ItemTypeEnum.HAMMER},
                {786, ItemTypeEnum.SWORD},
                {59, ItemTypeEnum.AXE},
                {45, ItemTypeEnum.DAGGER},
                {1447, ItemTypeEnum.WAND},
                {1437, ItemTypeEnum.STAFF},
                {605, ItemTypeEnum.BOW},
                {622, ItemTypeEnum.SHIELD},
                {240, ItemTypeEnum.BREAD},
            };        
        
        [Variable]
        public static readonly SerializableDictionary<int, int> Prices = new SerializableDictionary<int, int>()
            {
                {60, 0},
                {80, 15000},
                {100, 35000},
                {120, 90000},
                {140, 170000},
                {160, 240000},
                {180, 330000},
                {200, 470000},
           };

        [Variable]
        public static readonly double BreadHpPerKama = 0.25;

        [Variable]
        public static string PatchName = "npcs_shops.sql";

        private static string m_patchPath;
        [Initialization(typeof(ItemManager))]
        public static void Initialize()
        {
            if (!Active)
                return;

            m_patchPath = Path.Combine(Path.GetDirectoryName(Plugin.CurrentPlugin.Context.AssemblyPath), PatchName);
            if (File.Exists(m_patchPath))
                File.Delete(m_patchPath);

            foreach (var seller in Sellers)
            {
                var npcAction = new KeyValueListBase("npcs_actions");
                npcAction.AddPair("RecognizerType", "Shop");
                npcAction.AddPair("Npc", seller.Key);

                AppendQuery(SqlBuilder.BuildInsert(npcAction));
                AppendQuery("SET @shopid=(SELECT LAST_INSERT_ID())");

                var selector = (from entry in ItemManager.Instance.GetTemplates()
                                where entry.TypeId == (int)seller.Value && entry.Level >= 20 &&
                                entry.Effects.Count > 0 && !entry.Etheral
                                orderby entry.Level ascending
                                select entry);


                foreach (var template in selector)
                {
                    var price = Prices.First(entry => template.Level <= entry.Key).Value;

                    if (template.TypeId == (int)ItemTypeEnum.BREAD)
                    {
                        var restoreEffect = template.Effects.SingleOrDefault(entry => entry.EffectId == EffectsEnum.Effect_AddHealth) as EffectDice;

                        if (restoreEffect == null)
                            continue;

                        price = (int)( Math.Max(restoreEffect.DiceFace, restoreEffect.DiceNum) * BreadHpPerKama);
                    }

                    var itemQuery = new KeyValueListBase("items_selled");
                    itemQuery.AddPair("RecognizerType", "Npc");
                    itemQuery.AddPair("ItemId", template.Id);
                    itemQuery.AddPair("Npc_NpcShopId", new RawData("@shopid"));
                    itemQuery.AddPair("Npc_CustomPrice", price);
                    itemQuery.AddPair("Npc_MaxStats", price == 0 ? "1" : "0");

                    AppendQuery(SqlBuilder.BuildInsert(itemQuery));

                    if (price == 0)
                    {
                        var updatePrice = new UpdateKeyValueList("items_templates");
                        updatePrice.AddPair("Price", 0);
                        updatePrice.AddWherePair("Id", template.Id);
                        AppendQuery(SqlBuilder.BuildUpdate(updatePrice));
                    }
                }
            }
        }

        private static void AppendQuery(string query)
        {
            File.AppendAllText(m_patchPath, query + ";\r\n");
        }
    }
}