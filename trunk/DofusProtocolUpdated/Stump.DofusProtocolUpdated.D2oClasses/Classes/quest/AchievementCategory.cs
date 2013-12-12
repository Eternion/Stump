

// Generated on 12/12/2013 16:57:42
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
        [I18NField]
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
        public uint ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }
        [D2OIgnore]
        public String Icon
        {
            get { return icon; }
            set { icon = value; }
        }
        [D2OIgnore]
        public uint Order
        {
            get { return order; }
            set { order = value; }
        }
        [D2OIgnore]
        public String Color
        {
            get { return color; }
            set { color = value; }
        }
        [D2OIgnore]
        public List<uint> AchievementIds
        {
            get { return achievementIds; }
            set { achievementIds = value; }
        }
    }
}