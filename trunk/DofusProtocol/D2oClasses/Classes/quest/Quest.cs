

// Generated on 10/06/2013 17:58:55
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Quest", "com.ankamagames.dofus.datacenter.quest")]
    [Serializable]
    public class Quest : IDataObject, IIndexedData
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
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public List<uint> StepIds
        {
            get { return stepIds; }
            set { stepIds = value; }
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
    }
}