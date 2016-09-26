 


// Generated on 09/26/2016 01:50:47
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
    [TableName("AchievementCategories")]
    [D2OClass("AchievementCategory", "com.ankamagames.dofus.datacenter.quest")]
    public class AchievementCategoryRecord : ID2ORecord, ISaveIntercepter
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
        public uint ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        [D2OIgnore]
        [NullString]
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
        [NullString]
        public String Color
        {
            get { return color; }
            set { color = value; }
        }

        [D2OIgnore]
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
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
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
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (AchievementCategory)parent : new AchievementCategory();
            obj.id = Id;
            obj.nameId = NameId;
            obj.parentId = ParentId;
            obj.icon = Icon;
            obj.order = Order;
            obj.color = Color;
            obj.achievementIds = AchievementIds;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_achievementIdsBin = achievementIds == null ? null : achievementIds.ToBinary();
        
        }
    }
}