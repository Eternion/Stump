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

namespace GameplayPlugin.Npcs
{
    public static class NpcShops
    {
        [Variable] public static readonly bool Active = true;

        [Variable] public static readonly short OrbItemTemplate = 20000;

        [Variable] public static readonly SerializableDictionary<int, ItemTypeEnum[]> Sellers =
            new SerializableDictionary<int, ItemTypeEnum[]>
                {
                    {812, new[] {ItemTypeEnum.CHAPEAU}},
                    {1158, new[] {ItemTypeEnum.CAPE}},
                    {790, new[] {ItemTypeEnum.ANNEAU}},
                    {952, new[] {ItemTypeEnum.AMULETTE}},
                    {794, new[] {ItemTypeEnum.BOTTES}},
                    {787, new[] {ItemTypeEnum.CEINTURE}},
                    {1053, new[] {ItemTypeEnum.DOFUS}},
                    {
                        61,
                        new[]
                            {
                                ItemTypeEnum.MARTEAU, ItemTypeEnum.ÉPÉE, ItemTypeEnum.BAGUETTE, ItemTypeEnum.BÂTON,
                                ItemTypeEnum.ARC, ItemTypeEnum.DAGUE, ItemTypeEnum.HACHE, ItemTypeEnum.PIOCHE
                            }
                    },
                    {240, new[] {ItemTypeEnum.PAIN}},
                };

        [Variable] public static readonly int OrbSeller = 551;

        [Variable] public static readonly SerializableDictionary<int, int> OrbsPrices =
            new SerializableDictionary<int, int>
                {
                    {60, 0},
                    {80, 1200},
                    {100, 2000},
                    {120, 3000},
                    {140, 4500},
                    {160, 6050},
                    {180, 7900},
                    {200, 10000},
                };

        [Variable] public static readonly SerializableDictionary<int, int> Prices =
            new SerializableDictionary<int, int>
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

        [Variable] public static readonly double BreadHpPerKama = 0.25;

        [Variable] public static readonly int MinItemLevel = 20;

        [Variable] public static string PatchName = "npcs_shops.sql";

        [Variable] public static int[] IgnoredItems =
        {
            12655,
            12656,
            12657,
            12658,
            11088,
            11089,
            11091,
            11092,
            9418,
            9451,
            9452,
            9453,
            11418,
            11419,
            11420,
            11421,
            9660,
            9662,
            9663,
            9667,
            9652,
            9654,
            9655,
            9658,
            13483,
            13484,
            13485,
            13486,
            12502,
            12503,
            12504,
            12505,
            11055,
            11056,
            11057,
            11058,
            10554,
            10556,
            10558,
            10560,
            10562,
            11376,
            11377,
            11378,
            11379,
            10180,
            10181,
            10182,
            10183,
            9410,
            9411,
            9412,
            9413,
            10987,
            10988,
            10989,
            10990,
            9725,
            9728,
            9727,
            9724,
            9726,
            12203,
            9729,
            9736,
            9731,
            9735,
            9730,
            9751,
            9754,
            9753,
            9750,
            9752,
            9709,
            11041,
            9707,
            9708,
            9706,
            9713,
            9717,
            9711,
            9712,
            9716,
            9755,
            9759,
            9757,
            9758,
            9756,
            9747,
            9749,
            9745,
            9746,
            9748,
            9741,
            9744,
            9743,
            9742,
            9739
        };

        private static string m_patchPath;

        [Initialization(typeof (ItemManager))]
        public static void Initialize()
        {
            if (!Active)
                return;

            m_patchPath = Path.Combine(Path.GetDirectoryName(Plugin.CurrentPlugin.Context.AssemblyPath), PatchName);
            if (File.Exists(m_patchPath))
                File.Delete(m_patchPath);

            var orbSellerAction = new KeyValueListBase("npcs_actions");
            orbSellerAction.AddPair("Type", "Shop");
            orbSellerAction.AddPair("NpcId", OrbSeller);
            orbSellerAction.AddPair("Parameter0", OrbItemTemplate);

            AppendQuery("SET @orbshop=(SELECT Id FROM `npcs_actions` WHERE " +
                        string.Format("`NpcId`={0} AND `Type`='Shop')", OrbSeller));
            AppendQuery(SqlBuilder.BuildDelete("npcs_actions", "Id=@orbshop"));
            AppendQuery(SqlBuilder.BuildDelete("npcs_items", "NpcShopId=@orbshop"));

            AppendQuery(SqlBuilder.BuildInsert(orbSellerAction));
            AppendQuery("SET @orbshop=(SELECT LAST_INSERT_ID())");

            foreach (var seller in Sellers)
            {
                var npcAction = new KeyValueListBase("npcs_actions");
                npcAction.AddPair("Type", "Shop");
                npcAction.AddPair("NpcId", seller.Key);

                AppendQuery("SET @shopid=(SELECT Id FROM `npcs_actions` WHERE " +
                            string.Format("`NpcId`={0} AND `Type`='Shop')", seller.Key));
                AppendQuery(SqlBuilder.BuildDelete("npcs_actions", "Id=@shopid"));
                AppendQuery(SqlBuilder.BuildDelete("npcs_items", "NpcShopId=@shopid"));

                AppendQuery(SqlBuilder.BuildInsert(npcAction));
                AppendQuery("SET @shopid=(SELECT LAST_INSERT_ID())");

                var selector = (from entry in ItemManager.Instance.GetTemplates()
                                                             where
                                                                 Array.IndexOf(seller.Value, (ItemTypeEnum) entry.TypeId) !=
                                                                 -1 && entry.Level >= MinItemLevel &&
                                                                 entry.Effects.Count > 0 && !entry.Etheral
                                                             orderby entry.Level ascending
                                                             select entry);

                foreach (var template in selector)
                {
                    if (IgnoredItems.Contains(template.Id))
                        continue;

                    var price = Prices.First(entry => template.Level <= entry.Key).Value;

                    if (template.TypeId == (int) ItemTypeEnum.PAIN)
                    {
                        var restoreEffect =
                            template.Effects.SingleOrDefault(entry => entry.EffectId == EffectsEnum.Effect_AddHealth) as
                            EffectDice;

                        if (restoreEffect == null)
                            continue;

                        price = (int) (Math.Max(restoreEffect.DiceFace, restoreEffect.DiceNum)*BreadHpPerKama);
                    }

                    var itemQuery = new KeyValueListBase("npcs_items");
                    itemQuery.AddPair("ItemId", template.Id);
                    itemQuery.AddPair("NpcShopId", new RawData("@shopid"));
                    itemQuery.AddPair("CustomPrice", price);
                    itemQuery.AddPair("BuyCriterion", string.Empty);
                    AppendQuery(SqlBuilder.BuildInsert(itemQuery));

                    if (price > 0)
                    {
                        var orbPrice = OrbsPrices.First(entry => template.Level <= entry.Key).Value;

                        var perfectItemQuery = new KeyValueListBase("npcs_items");
                        perfectItemQuery.AddPair("ItemId", template.Id);
                        perfectItemQuery.AddPair("NpcShopId", new RawData("@orbshop"));
                        perfectItemQuery.AddPair("CustomPrice", orbPrice);
                        perfectItemQuery.AddPair("MaxStats", "1");
                        perfectItemQuery.AddPair("BuyCriterion", string.Empty);
                        AppendQuery(SqlBuilder.BuildInsert(perfectItemQuery));
                    }

                    if (price != 0)
                        continue;

                    var updatePrice = new UpdateKeyValueList("items_templates");
                    updatePrice.AddPair("Price", 0);
                    updatePrice.AddWherePair("Id", template.Id);
                    AppendQuery(SqlBuilder.BuildUpdate(updatePrice));
                }
            }
        }

        private static void AppendQuery(string query)
        {
            File.AppendAllText(m_patchPath, query + ";\r\n");
        }
    }
}