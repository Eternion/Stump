 


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
    [TableName("AlignmentSides")]
    [D2OClass("AlignmentSide")]
    public class AlignmentSideRecord : ID2ORecord
    {
        private const String MODULE = "AlignmentSides";
        public int id;
        public uint nameId;
        public Boolean canConquest;

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

        public Boolean CanConquest
        {
            get { return canConquest; }
            set { canConquest = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (AlignmentSide)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            CanConquest = castedObj.canConquest;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new AlignmentSide();
            obj.id = Id;
            obj.nameId = NameId;
            obj.canConquest = CanConquest;
            return obj;
        
        }
    }
}