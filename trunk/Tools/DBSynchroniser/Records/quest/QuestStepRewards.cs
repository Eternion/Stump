 


// Generated on 10/06/2013 01:10:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("QuestStepRewards")]
    public class QuestStepRewardsRecord : ID2ORecord
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

        [PrimaryKey("Id", false)]
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

        [Ignore]
        public List<List<uint>> ItemsReward
        {
            get { return itemsReward; }
            set
            {
                itemsReward = value;
                m_itemsRewardBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_itemsRewardBin;
        public byte[] ItemsRewardBin
        {
            get { return m_itemsRewardBin; }
            set
            {
                m_itemsRewardBin = value;
                itemsReward = value == null ? null : value.ToObject<List<List<uint>>>();
            }
        }

        [Ignore]
        public List<uint> EmotesReward
        {
            get { return emotesReward; }
            set
            {
                emotesReward = value;
                m_emotesRewardBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_emotesRewardBin;
        public byte[] EmotesRewardBin
        {
            get { return m_emotesRewardBin; }
            set
            {
                m_emotesRewardBin = value;
                emotesReward = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [Ignore]
        public List<uint> JobsReward
        {
            get { return jobsReward; }
            set
            {
                jobsReward = value;
                m_jobsRewardBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_jobsRewardBin;
        public byte[] JobsRewardBin
        {
            get { return m_jobsRewardBin; }
            set
            {
                m_jobsRewardBin = value;
                jobsReward = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [Ignore]
        public List<uint> SpellsReward
        {
            get { return spellsReward; }
            set
            {
                spellsReward = value;
                m_spellsRewardBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_spellsRewardBin;
        public byte[] SpellsRewardBin
        {
            get { return m_spellsRewardBin; }
            set
            {
                m_spellsRewardBin = value;
                spellsReward = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (QuestStepRewards)obj;
            
            Id = castedObj.id;
            StepId = castedObj.stepId;
            LevelMin = castedObj.levelMin;
            LevelMax = castedObj.levelMax;
            ItemsReward = castedObj.itemsReward;
            EmotesReward = castedObj.emotesReward;
            JobsReward = castedObj.jobsReward;
            SpellsReward = castedObj.spellsReward;
        }
        
        public object CreateObject()
        {
            var obj = new QuestStepRewards();
            
            obj.id = Id;
            obj.stepId = StepId;
            obj.levelMin = LevelMin;
            obj.levelMax = LevelMax;
            obj.itemsReward = ItemsReward;
            obj.emotesReward = EmotesReward;
            obj.jobsReward = JobsReward;
            obj.spellsReward = SpellsReward;
            return obj;
        
        }
    }
}