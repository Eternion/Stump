 


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
    [TableName("EmblemSymbols")]
    [D2OClass("EmblemSymbol")]
    public class EmblemSymbolRecord : ID2ORecord
    {
        private const String MODULE = "EmblemSymbols";
        public int id;
        public int iconId;
        public int skinId;
        public int order;
        public int categoryId;
        public Boolean colorizable;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        public int SkinId
        {
            get { return skinId; }
            set { skinId = value; }
        }

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new EmblemSymbol();
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