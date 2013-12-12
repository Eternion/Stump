

// Generated on 12/12/2013 16:57:42
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AchievementReward", "com.ankamagames.dofus.datacenter.quest")]
    [Serializable]
    public class AchievementReward : IDataObject, IIndexedData
    {
        public const String MODULE = "AchievementRewards";
        public uint id;
        public uint achievementId;
        public int levelMin;
        public int levelMax;
        public List<uint> itemsReward;
        public List<uint> itemsQuantityReward;
        public List<uint> emotesReward;
        public List<uint> spellsReward;
        public List<uint> titlesReward;
        public List<uint> ornamentsReward;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint AchievementId
        {
            get { return achievementId; }
            set { achievementId = value; }
        }
        [D2OIgnore]
        public int LevelMin
        {
            get { return levelMin; }
            set { levelMin = value; }
        }
        [D2OIgnore]
        public int LevelMax
        {
            get { return levelMax; }
            set { levelMax = value; }
        }
        [D2OIgnore]
        public List<uint> ItemsReward
        {
            get { return itemsReward; }
            set { itemsReward = value; }
        }
        [D2OIgnore]
        public List<uint> ItemsQuantityReward
        {
            get { return itemsQuantityReward; }
            set { itemsQuantityReward = value; }
        }
        [D2OIgnore]
        public List<uint> EmotesReward
        {
            get { return emotesReward; }
            set { emotesReward = value; }
        }
        [D2OIgnore]
        public List<uint> SpellsReward
        {
            get { return spellsReward; }
            set { spellsReward = value; }
        }
        [D2OIgnore]
        public List<uint> TitlesReward
        {
            get { return titlesReward; }
            set { titlesReward = value; }
        }
        [D2OIgnore]
        public List<uint> OrnamentsReward
        {
            get { return ornamentsReward; }
            set { ornamentsReward = value; }
        }
    }
}