 


// Generated on 08/13/2015 17:50:47
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
    [TableName("Quests")]
    [D2OClass("Quest", "com.ankamagames.dofus.datacenter.quest")]
    public class QuestRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Quests";
        public uint id;
        [I18NField]
        public uint nameId;
        public List<uint> stepIds;
        public uint categoryId;
        public Boolean isRepeatable;
        public uint repeatType;
        public uint repeatLimit;
        public Boolean isDungeonQuest;
        public uint levelMin;
        public uint levelMax;
        public Boolean isPartyQuest;

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
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> StepIds
        {
            get { return stepIds; }
            set
            {
                stepIds = value;
                m_stepIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_stepIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] StepIdsBin
        {
            get { return m_stepIdsBin; }
            set
            {
                m_stepIdsBin = value;
                stepIds = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        public uint CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        [D2OIgnore]
        public Boolean IsRepeatable
        {
            get { return isRepeatable; }
            set { isRepeatable = value; }
        }

        [D2OIgnore]
        public uint RepeatType
        {
            get { return repeatType; }
            set { repeatType = value; }
        }

        [D2OIgnore]
        public uint RepeatLimit
        {
            get { return repeatLimit; }
            set { repeatLimit = value; }
        }

        [D2OIgnore]
        public Boolean IsDungeonQuest
        {
            get { return isDungeonQuest; }
            set { isDungeonQuest = value; }
        }

        [D2OIgnore]
        public uint LevelMin
        {
            get { return levelMin; }
            set { levelMin = value; }
        }

        [D2OIgnore]
        public uint LevelMax
        {
            get { return levelMax; }
            set { levelMax = value; }
        }

        [D2OIgnore]
        public Boolean IsPartyQuest
        {
            get { return isPartyQuest; }
            set { isPartyQuest = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Quest)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            StepIds = castedObj.stepIds;
            CategoryId = castedObj.categoryId;
            IsRepeatable = castedObj.isRepeatable;
            RepeatType = castedObj.repeatType;
            RepeatLimit = castedObj.repeatLimit;
            IsDungeonQuest = castedObj.isDungeonQuest;
            LevelMin = castedObj.levelMin;
            LevelMax = castedObj.levelMax;
            IsPartyQuest = castedObj.isPartyQuest;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Quest)parent : new Quest();
            obj.id = Id;
            obj.nameId = NameId;
            obj.stepIds = StepIds;
            obj.categoryId = CategoryId;
            obj.isRepeatable = IsRepeatable;
            obj.repeatType = RepeatType;
            obj.repeatLimit = RepeatLimit;
            obj.isDungeonQuest = IsDungeonQuest;
            obj.levelMin = LevelMin;
            obj.levelMax = LevelMax;
            obj.isPartyQuest = IsPartyQuest;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_stepIdsBin = stepIds == null ? null : stepIds.ToBinary();
        
        }
    }
}