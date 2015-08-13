

// Generated on 08/13/2015 17:13:53
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
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public List<uint> StepIds
        {
            get { return this.stepIds; }
            set { this.stepIds = value; }
        }
        [D2OIgnore]
        public uint CategoryId
        {
            get { return this.categoryId; }
            set { this.categoryId = value; }
        }
        [D2OIgnore]
        public Boolean IsRepeatable
        {
            get { return this.isRepeatable; }
            set { this.isRepeatable = value; }
        }
        [D2OIgnore]
        public uint RepeatType
        {
            get { return this.repeatType; }
            set { this.repeatType = value; }
        }
        [D2OIgnore]
        public uint RepeatLimit
        {
            get { return this.repeatLimit; }
            set { this.repeatLimit = value; }
        }
        [D2OIgnore]
        public Boolean IsDungeonQuest
        {
            get { return this.isDungeonQuest; }
            set { this.isDungeonQuest = value; }
        }
        [D2OIgnore]
        public uint LevelMin
        {
            get { return this.levelMin; }
            set { this.levelMin = value; }
        }
        [D2OIgnore]
        public uint LevelMax
        {
            get { return this.levelMax; }
            set { this.levelMax = value; }
        }
        [D2OIgnore]
        public Boolean IsPartyQuest
        {
            get { return this.isPartyQuest; }
            set { this.isPartyQuest = value; }
        }
    }
}