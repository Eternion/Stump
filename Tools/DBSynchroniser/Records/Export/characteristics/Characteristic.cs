 


// Generated on 09/01/2015 10:48:46
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
    [TableName("Characteristics")]
    [D2OClass("Characteristic", "com.ankamagames.dofus.datacenter.characteristics")]
    public class CharacteristicRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Characteristics";
        public int id;
        public String keyword;
        [I18NField]
        public uint nameId;
        public String asset;
        public int categoryId;
        public Boolean visible;
        public int order;
        public Boolean upgradable;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [NullString]
        public String Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        [NullString]
        public String Asset
        {
            get { return asset; }
            set { asset = value; }
        }

        [D2OIgnore]
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        [D2OIgnore]
        public Boolean Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        [D2OIgnore]
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        [D2OIgnore]
        public Boolean Upgradable
        {
            get { return upgradable; }
            set { upgradable = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Characteristic)obj;
            
            Id = castedObj.id;
            Keyword = castedObj.keyword;
            NameId = castedObj.nameId;
            Asset = castedObj.asset;
            CategoryId = castedObj.categoryId;
            Visible = castedObj.visible;
            Order = castedObj.order;
            Upgradable = castedObj.upgradable;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Characteristic)parent : new Characteristic();
            obj.id = Id;
            obj.keyword = Keyword;
            obj.nameId = NameId;
            obj.asset = Asset;
            obj.categoryId = CategoryId;
            obj.visible = Visible;
            obj.order = Order;
            obj.upgradable = Upgradable;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}