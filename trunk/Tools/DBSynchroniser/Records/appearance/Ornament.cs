 


// Generated on 10/19/2013 17:17:42
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
    [TableName("Ornaments")]
    [D2OClass("Ornament", "com.ankamagames.dofus.datacenter.appearance")]
    public class OrnamentRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "Ornaments";
        public int id;
        public uint nameId;
        public Boolean visible;
        public int assetId;
        public int iconId;
        public int rarity;
        public int order;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
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
        public Boolean Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        [D2OIgnore]
        public int AssetId
        {
            get { return assetId; }
            set { assetId = value; }
        }

        [D2OIgnore]
        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        [D2OIgnore]
        public int Rarity
        {
            get { return rarity; }
            set { rarity = value; }
        }

        [D2OIgnore]
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Ornament)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            Visible = castedObj.visible;
            AssetId = castedObj.assetId;
            IconId = castedObj.iconId;
            Rarity = castedObj.rarity;
            Order = castedObj.order;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (Ornament)parent : new Ornament();
            obj.id = Id;
            obj.nameId = NameId;
            obj.visible = Visible;
            obj.assetId = AssetId;
            obj.iconId = IconId;
            obj.rarity = Rarity;
            obj.order = Order;
            return obj;
        
        }
    }
}