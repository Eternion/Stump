 


// Generated on 10/06/2013 14:22:01
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("AchievementRewards")]
    [D2OClass("AchievementReward")]
    public class AchievementRewardRecord : ID2ORecord
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

        [PrimaryKey("Id", false)]
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
        public byte[] TitlesRewardBin
        {
            get { return m_titlesRewardBin; }
            set
            {
                m_titlesRewardBin = value;
                titlesReward = value == null ? null : value.ToObject<List<uint>>();
            }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new AchievementReward();
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
    }
}