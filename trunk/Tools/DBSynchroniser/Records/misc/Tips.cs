 


// Generated on 10/06/2013 01:10:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("Tips")]
    public class TipsRecord : ID2ORecord
    {
        private const String MODULE = "Tips";
        public int id;
        public uint descId;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint DescId
        {
            get { return descId; }
            set { descId = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (Tips)obj;
            
            Id = castedObj.id;
            DescId = castedObj.descId;
        }
        
        public object CreateObject()
        {
            var obj = new Tips();
            
            obj.id = Id;
            obj.descId = DescId;
            return obj;
        
        }
    }
}