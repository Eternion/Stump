using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Items.Runes;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Jobs;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft.Runes
{
    public class RuneManager : DataManager<RuneManager>, ISaveable
    {
        [Variable]
        public static int DecraftHistoryLimit = 1000;

        [Variable]
        public static double DecraftPercentile = 0.5;

        private Dictionary<EffectsEnum, RuneInformation[]> m_runes = new Dictionary<EffectsEnum, RuneInformation[]>();
        private Dictionary<ItemTypeEnum, LimitedOccurenceCounter<ItemTemplate>> m_decraftHistory = new Dictionary<ItemTypeEnum, LimitedOccurenceCounter<ItemTemplate>>();
        private List<int> m_craftedItemsIds = new List<int>();

        private Dictionary<int, DecraftItemRecord> m_historyRecords = new Dictionary<int, DecraftItemRecord>();
        
        [Initialization(typeof (ItemManager))]
        public void Initiliaze()
        {
            var runes = new Dictionary<EffectsEnum, List<RuneInformation>>();

            foreach (var item in ItemManager.Instance.GetTemplates().Where(x => x.TypeId == (int) ItemTypeEnum.RUNE_DE_FORGEMAGIE))
            {
                foreach (var effect in item.Effects.OfType<EffectDice>())
                {
                    var rune = new RuneInformation(item, effect.Max);

                    if (runes.ContainsKey(effect.EffectId))
                        runes[effect.EffectId].Add(rune);
                    else
                        runes.Add(effect.EffectId, new List<RuneInformation>(new[] {rune}));
                }
            }

            m_runes = runes.ToDictionary(x => x.Key, x => x.Value.ToArray());

            m_historyRecords = Database.Fetch<DecraftItemRecord>(DecraftItemRelator.FetchQuery).ToDictionary(x => x.ItemId);
            foreach (var record in m_historyRecords.Values)
            {
                var item = ItemManager.Instance.TryGetTemplate(record.ItemId);
                RegisterDecraft(item, record.Amount);
            }

            World.Instance.RegisterSaveableInstance(this);
        }

        [Initialization(typeof(JobManager))]
        public void InitializeCraftableItems()
        {
            foreach (var job in JobManager.Instance.GetJobTemplates())
            {
                m_craftedItemsIds.AddRange(job.Skills.SelectMany(x => x.Recipes).Select(x => x.Id));
            }
        }

        public RuneInformation[] GetEffectRunes(EffectsEnum effect)
        {
            RuneInformation[] runes;
            return m_runes.TryGetValue(effect, out runes) ? runes : new RuneInformation[0];
        }

        public void RegisterDecraft(ItemTemplate item)
        {
            LimitedOccurenceCounter<ItemTemplate> counter;
            if (!m_decraftHistory.TryGetValue((ItemTypeEnum) item.TypeId, out counter))
            {
                counter = new LimitedOccurenceCounter<ItemTemplate>(DecraftHistoryLimit);
                m_decraftHistory.Add((ItemTypeEnum) item.TypeId, counter);
            }

            counter.Add(item);
        }

        private void RegisterDecraft(ItemTemplate item, int amount)
        {
            LimitedOccurenceCounter<ItemTemplate> counter;
            if (!m_decraftHistory.TryGetValue((ItemTypeEnum)item.TypeId, out counter))
            {
                counter = new LimitedOccurenceCounter<ItemTemplate>(DecraftHistoryLimit);
                m_decraftHistory.Add((ItemTypeEnum)item.TypeId, counter);
            }

            counter.Add(item, amount);
        }

        public int GetDecraftOccurence(ItemTemplate item)
        {
            LimitedOccurenceCounter<ItemTemplate> counter;
            if (m_decraftHistory.TryGetValue((ItemTypeEnum)item.TypeId, out counter))
            {
                return counter.GetOccurence(item);
            }

            return 0;
        }

        public int GetDecraftMedian(ItemTypeEnum type)
        {
            LimitedOccurenceCounter<ItemTemplate> counter;
            if (m_decraftHistory.TryGetValue(type, out counter))
            {
                return counter.GetPercentile(DecraftPercentile);
            }

            return 0;
        }

        private bool IsItemCraftable(ItemTemplate item)
        {
            return m_craftedItemsIds.Contains(item.Id);
        }

        public double GetDecraftCoefficient(ItemTemplate item)
        {
            if (!IsItemCraftable(item))
                return 0.5;

            var occurence = GetDecraftOccurence(item);
            var median = GetDecraftMedian((ItemTypeEnum) item.TypeId);

            if (median == 0)
                return 1;
            
            return (double) median/occurence;
        }

        public void Save()
        {
            Database.BeginTransaction();
            var dbIds = m_historyRecords.Keys.ToList();
            var ids = new List<int>();

            foreach (var counter in m_decraftHistory.Values)
            {
                foreach(var item in counter)
                {
                    ids.Add(item.Key.Id);

                    DecraftItemRecord record;
                    if (!m_historyRecords.TryGetValue(item.Key.Id, out record))
                    {
                        record = new DecraftItemRecord()
                        {
                            ItemId = item.Key.Id,
                            Amount = item.Value,
                        };

                        Database.Insert(record);
                        m_historyRecords.Add(record.ItemId, record);
                    }
                    else if (record.Amount != item.Value)
                    {
                        record.Amount = item.Value;
                        Database.Save(record);
                    }

                }
            }

            foreach(var id in dbIds.Except(ids).Distinct())
            {
                m_historyRecords.Remove(id);
                Database.Delete<DecraftItemRecord>(id);
            }

            Database.CompleteTransaction();
        }
    }
}