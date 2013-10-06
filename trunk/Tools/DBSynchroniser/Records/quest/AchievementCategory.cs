 


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
    [TableName("AchievementCategories")]
    [D2OClass("AchievementCategory")]
    public class AchievementCategoryRecord : ID2ORecord
    {
        private const String MODULE = "AchievementCategories";
        public uint id;
        public uint nameId;
        public uint parentId;
        public String icon;
        public uint order;
        public String color;
        public List<uint> achievementIds;

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

        public uint ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        [NullString]
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

        [NullString]
        public String Color
        {
            get { return color; }
            set { color = value; }
        }

        [Ignore]
        public List<uint> AchievementIds
        {
            get { return achievementIds; }
            set
            {
                achievementIds = value;
                m_achievementIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_achievementIdsBin;
        public byte[] AchievementIdsBin
        {
            get { return m_achievementIdsBin; }
            set
            {
                m_achievementIdsBin = value;
                achievementIds = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (AchievementCategory)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            ParentId = castedObj.parentId;
            Icon = castedObj.icon;
            Order = castedObj.order;
            Color = castedObj.color;
            AchievementIds = castedObj.achievementIds;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new AchievementCategory();
            obj.id = Id;
            obj.nameId = NameId;
            obj.parentId = ParentId;
            obj.icon = Icon;
            obj.order = Order;
            obj.color = Color;
            obj.achievementIds = AchievementIds;
            return obj;
        
        }
    }
}