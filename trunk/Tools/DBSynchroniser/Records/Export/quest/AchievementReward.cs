 


// Generated on 11/02/2013 14:55:50
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
    [TableName("AchievementRewards")]
    [D2OClass("AchievementReward", "com.ankamagames.dofus.datacenter.quest")]
    public class AchievementRewardRecord : ID2ORecord, ISaveIntercepter
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

        [D2OIgnore]
        [Ignore]
        public List<uint> TitlesReward
        {
            get { return titlesReward; }
            set
            {
                titlesReward = value;
                m_titlesRewardBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_titlesRewardBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] TitlesRewardBin
        {
            get { return m_titlesRewardBin; }
            set
            {
                m_titlesRewardBin = value;
                titlesReward = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> OrnamentsReward
        {
            get { return ornamentsReward; }
            set
            {
                ornamentsReward = value;
                m_ornamentsRewardBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_ornamentsRewardBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] OrnamentsRewardBin
        {
            get { return m_ornamentsRewardBin; }
            set
            {
                m_ornamentsRewardBin = value;
                ornamentsReward = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (AchievementReward)obj;
            
            Id = castedObj.id;
            AchievementId = castedObj.achievementId;
            LevelMin = castedObj.levelMin;
            LevelMax = castedObj.levelMax;
            ItemsReward = castedObj.itemsReward;
            EmotesReward = castedObj.emotesReward;
            SpellsReward = castedObj.spellsReward;
            TitlesReward = castedObj.titlesReward;
            OrnamentsReward = castedObj.ornamentsReward;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (AchievementReward)parent : new AchievementReward();
            obj.id = Id;
            obj.achievementId = AchievementId;
            obj.levelMin = LevelMin;
            obj.levelMax = LevelMax;
            obj.itemsReward = ItemsReward;
            obj.emotesReward = EmotesReward;
            obj.spellsReward = SpellsReward;
            obj.titlesReward = TitlesReward;
            obj.ornamentsReward = OrnamentsReward;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_itemsRewardBin = itemsReward == null ? null : itemsReward.ToBinary();
            m_emotesRewardBin = emotesReward == null ? null : emotesReward.ToBinary();
            m_spellsRewardBin = spellsReward == null ? null : spellsReward.ToBinary();
            m_titlesRewardBin = titlesReward == null ? null : titlesReward.ToBinary();
            m_ornamentsRewardBin = ornamentsReward == null ? null : ornamentsReward.ToBinary();
        
        }
    }
}