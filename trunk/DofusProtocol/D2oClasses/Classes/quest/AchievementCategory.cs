
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AchievementCategories")]
    [Serializable]
    public class AchievementCategory : IDataObject, IIndexedData
    {
        private const String MODULE = "AchievementCategories";
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

        public uint ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        public String Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

        public String Color
        {
            get { return color; }
            set { color = value; }
        }

        public List<uint> AchievementIds
        {
            get { return achievementIds; }
            set { achievementIds = value; }
        }

    }
}