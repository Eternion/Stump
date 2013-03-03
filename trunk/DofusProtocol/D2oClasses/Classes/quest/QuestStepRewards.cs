
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("QuestStepRewards")]
    [Serializable]
    public class QuestStepRewards : IDataObject, IIndexedData
    {
        private const String MODULE = "QuestStepRewards";
        public uint id;
        public uint stepId;
        public int levelMin;
        public int levelMax;
        public List<List<uint>> itemsReward;
        public List<uint> emotesReward;
        public List<uint> jobsReward;
        public List<uint> spellsReward;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint StepId
        {
            get { return stepId; }
            set { stepId = value; }
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

        public List<uint> JobsReward
        {
            get { return jobsReward; }
            set { jobsReward = value; }
        }

        public List<uint> SpellsReward
        {
            get { return spellsReward; }
            set { spellsReward = value; }
        }

    }
}