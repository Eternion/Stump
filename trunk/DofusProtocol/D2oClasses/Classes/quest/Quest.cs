
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Quests")]
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

        public List<uint> StepIds
        {
            get { return stepIds; }
            set { stepIds = value; }
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

    }
}