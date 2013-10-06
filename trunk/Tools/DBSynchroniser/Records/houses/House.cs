 


// Generated on 10/06/2013 14:21:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Houses")]
    [D2OClass("House")]
    public class HouseRecord : ID2ORecord
    {
        private const String MODULE = "Houses";
        public int typeId;
        public uint defaultPrice;
        public int nameId;
        public int descriptionId;
        public int gfxId;

        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }
        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        public uint DefaultPrice
        {
            get { return defaultPrice; }
            set { defaultPrice = value; }
        }

        public int NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public int DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public int GfxId
        {
            get { return gfxId; }
            set { gfxId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (House)obj;
            
            TypeId = castedObj.typeId;
            DefaultPrice = castedObj.defaultPrice;
            NameId = castedObj.nameId;
            DescriptionId = castedObj.descriptionId;
            GfxId = castedObj.gfxId;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new House();
            obj.typeId = TypeId;
            obj.defaultPrice = DefaultPrice;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            obj.gfxId = GfxId;
            return obj;
        
        }
    }
}