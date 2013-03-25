
// Generated on 03/25/2013 19:24:37
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AchievementRewards")]
    [Serializable]
    public class AchievementReward : IDataObject, IIndexedData
    {
        private const String MODULE = "AchievementRewards";
        public uint id;
        public uint achievementId;
        public int levelMin;
        public int levelMax;
        public List<List<uint>> itemsReward;
        public List<uint> emotesReward;
        public List<uint> spellsReward;
        public List<uint> titlesReward;
        public List<uint> ornamentsReward;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint AchievementId
        {
            get { return achievementId; }
            set { achievementId = value; }
        }

        public int LevelMin
        {
            get { return levelMin; }
            set { levelMin = value; }
        }

        public int LevelMax
        {
            get { return levelMax; }
            set { levelMax = value; }
        }

        public List<List<uint>> ItemsReward
        {
            get { return itemsReward; }
            set { itemsReward = value; }
        }

        public List<uint> EmotesReward
        {
            get { return emotesReward; }
            set { emotesReward = value; }
        }

        public List<uint> SpellsReward
        {
            get { return spellsReward; }
            set { spellsReward = value; }
        }

        public List<uint> TitlesReward
        {
            get { return titlesReward; }
            set { titlesReward = value; }
        }

        public List<uint> OrnamentsReward
        {
            get { return ornamentsReward; }
            set { ornamentsReward = value; }
        }

    }
}