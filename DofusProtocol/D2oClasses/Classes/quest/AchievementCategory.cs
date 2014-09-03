

// Generated on 09/02/2014 22:34:38
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AchievementCategory", "com.ankamagames.dofus.datacenter.quest")]
    [Serializable]
    public class AchievementCategory : IDataObject, IIndexedData
    {
        public const String MODULE = "AchievementCategories";
        public uint id;
        public uint nameId;
        public uint parentId;
        public String icon;
        public uint order;
        public String color;
        public List<uint> achievementIds;
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
        public uint ParentId
        {
            get { return this.parentId; }
            set { this.parentId = value; }
        }
        [D2OIgnore]
        public String Icon
        {
            get { return this.icon; }
            set { this.icon = value; }
        }
        [D2OIgnore]
        public uint Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
        [D2OIgnore]
        public String Color
        {
            get { return this.color; }
            set { this.color = value; }
        }
        [D2OIgnore]
        public List<uint> AchievementIds
        {
            get { return this.achievementIds; }
            set { this.achievementIds = value; }
        }
    }
}