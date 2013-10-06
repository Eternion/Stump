 


// Generated on 10/06/2013 14:21:58
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Ornaments")]
    [D2OClass("Ornament")]
    public class OrnamentRecord : ID2ORecord
    {
        private const String MODULE = "Ornaments";
        public int id;
        public uint nameId;
        public Boolean visible;
        public int assetId;
        public int iconId;
        public int rarity;
        public int order;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public Boolean Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public int AssetId
        {
            get { return assetId; }
            set { assetId = value; }
        }

        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        public int Rarity
        {
            get { return rarity; }
            set { rarity = value; }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new Ornament();
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