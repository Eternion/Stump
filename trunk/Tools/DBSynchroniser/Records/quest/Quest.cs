 


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
    [D2OClass("Quests")]
    public class QuestRecord : ID2ORecord
    {
        private const String MODULE = "Quests";
        public uint id;
        public uint nameId;
        public List<uint> stepIds;
        public uint categoryId;
        public Boolean isRepeatable;
        public uint repeatType;
        public uint repeatLimit;
        public Boolean isDungeonQuest;
        public uint levelMin;
        public uint levelMax;

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

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
        public byte[] StepIdsBin
        {
            get { return m_stepIdsBin; }
            set
            {
                m_stepIdsBin = value;
                stepIds = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public uint CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        public Boolean IsRepeatable
        {
            get { return isRepeatable; }
            set { isRepeatable = value; }
        }

        public uint RepeatType
        {
            get { return repeatType; }
            set { repeatType = value; }
        }

        public uint RepeatLimit
        {
            get { return repeatLimit; }
            set { repeatLimit = value; }
        }

        public Boolean IsDungeonQuest
        {
            get { return isDungeonQuest; }
            set { isDungeonQuest = value; }
        }

        public uint LevelMin
        {
            get { return levelMin; }
            set { levelMin = value; }
        }

        public uint LevelMax
        {
            get { return levelMax; }
            set { levelMax = value; }
        }

        public void AssignFields(object obj)
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
        }
        
        public object CreateObject()
        {
            var obj = new Quest();
            
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
            return obj;
        
        }
    }
}