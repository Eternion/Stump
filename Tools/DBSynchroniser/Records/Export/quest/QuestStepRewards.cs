 


// Generated on 11/16/2015 14:26:42
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("QuestStepRewards")]
    [D2OClass("QuestStepRewards", "com.ankamagames.dofus.datacenter.quest")]
    public class QuestStepRewardsRecord : ID2ORecord, ISaveIntercepter
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

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
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
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] ItemsRewardBin
        {
            get { return m_itemsRewardBin; }
            set
            {
                m_itemsRewardBin = value;
                itemsReward = value == null ? null : value.ToObject<List<List<uint>>>();
            }
        }

        [D2OIgnore]
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
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] EmotesRewardBin
        {
            get { return m_emotesRewardBin; }
            set
            {
                m_emotesRewardBin = value;
                emotesReward = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
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
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] JobsRewardBin
        {
            get { return m_jobsRewardBin; }
            set
            {
                m_jobsRewardBin = value;
                jobsReward = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
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
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] SpellsRewardBin
        {
            get { return m_spellsRewardBin; }
            set
            {
                m_spellsRewardBin = value;
                spellsReward = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public virtual void AssignFields(object obj)
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
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (QuestStepRewards)parent : new QuestStepRewards();
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
        
        public virtual void BeforeSave(bool insert)
        {
            m_itemsRewardBin = itemsReward == null ? null : itemsReward.ToBinary();
            m_emotesRewardBin = emotesReward == null ? null : emotesReward.ToBinary();
            m_jobsRewardBin = jobsReward == null ? null : jobsReward.ToBinary();
            m_spellsRewardBin = spellsReward == null ? null : spellsReward.ToBinary();
        
        }
    }
}