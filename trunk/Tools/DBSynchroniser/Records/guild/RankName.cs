 


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
    [TableName("RankNames")]
    [D2OClass("RankName")]
    public class RankNameRecord : ID2ORecord
    {
        private const String MODULE = "RankNames";
        public int id;
        public uint nameId;
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

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (RankName)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            Order = castedObj.order;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new RankName();
            obj.id = Id;
            obj.nameId = NameId;
            obj.order = Order;
            return obj;
        
        }
    }
}