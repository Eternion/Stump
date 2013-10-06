 


// Generated on 10/06/2013 14:21:57
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("AlignmentOrder")]
    [D2OClass("AlignmentOrder")]
    public class AlignmentOrderRecord : ID2ORecord
    {
        private const String MODULE = "AlignmentOrder";
        public int id;
        public uint nameId;
        public uint sideId;

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

        public uint SideId
        {
            get { return sideId; }
            set { sideId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (AlignmentOrder)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            SideId = castedObj.sideId;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new AlignmentOrder();
            obj.id = Id;
            obj.nameId = NameId;
            obj.sideId = SideId;
            return obj;
        
        }
    }
}