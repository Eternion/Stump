 


// Generated on 10/13/2013 12:21:15
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
    [TableName("EmblemSymbols")]
    [D2OClass("EmblemSymbol", "com.ankamagames.dofus.datacenter.guild")]
    public class EmblemSymbolRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "EmblemSymbols";
        public int id;
        public int iconId;
        public int skinId;
        public int order;
        public int categoryId;
        public Boolean colorizable;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        [D2OIgnore]
        public int SkinId
        {
            get { return skinId; }
            set { skinId = value; }
        }

        [D2OIgnore]
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        [D2OIgnore]
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        [D2OIgnore]
        public Boolean Colorizable
        {
            get { return colorizable; }
            set { colorizable = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (EmblemSymbol)obj;
            
            Id = castedObj.id;
            IconId = castedObj.iconId;
            SkinId = castedObj.skinId;
            Order = castedObj.order;
            CategoryId = castedObj.categoryId;
            Colorizable = castedObj.colorizable;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (EmblemSymbol)parent : new EmblemSymbol();
            obj.id = Id;
            obj.iconId = IconId;
            obj.skinId = SkinId;
            obj.order = Order;
            obj.categoryId = CategoryId;
            obj.colorizable = Colorizable;
            return obj;
        
        }
    }
}