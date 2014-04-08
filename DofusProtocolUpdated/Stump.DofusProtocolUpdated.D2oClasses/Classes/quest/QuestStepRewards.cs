

// Generated on 12/12/2013 16:57:42
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("QuestStepRewards", "com.ankamagames.dofus.datacenter.quest")]
    [Serializable]
    public class QuestStepRewards : IDataObject, IIndexedData
    {
        public const String MODULE = "QuestStepRewards";
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
        [D2OIgnore]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint StepId
        {
            get { return stepId; }
            set { stepId = value; }
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
        public List<List<uint>> ItemsReward
        {
            get { return itemsReward; }
            set { itemsReward = value; }
        }
        [D2OIgnore]
        public List<uint> EmotesReward
        {
            get { return emotesReward; }
            set { emotesReward = value; }
        }
        [D2OIgnore]
        public List<uint> JobsReward
        {
            get { return jobsReward; }
            set { jobsReward = value; }
        }
        [D2OIgnore]
        public List<uint> SpellsReward
        {
            get { return spellsReward; }
            set { spellsReward = value; }
        }
    }
}