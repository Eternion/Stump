 


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
    [TableName("TaxCollectorFirstnames")]
    [D2OClass("TaxCollectorFirstname")]
    public class TaxCollectorFirstnameRecord : ID2ORecord
    {
        private const String MODULE = "TaxCollectorFirstnames";
        public int id;
        public uint firstnameId;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint FirstnameId
        {
            get { return firstnameId; }
            set { firstnameId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (TaxCollectorFirstname)obj;
            
            Id = castedObj.id;
            FirstnameId = castedObj.firstnameId;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new TaxCollectorFirstname();
            obj.id = Id;
            obj.firstnameId = FirstnameId;
            return obj;
        
        }
    }
}