 


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
    [TableName("AlignmentSides")]
    [D2OClass("AlignmentSide", "com.ankamagames.dofus.datacenter.alignments")]
    public class AlignmentSideRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "AlignmentSides";
        public int id;
        public uint nameId;
        public Boolean canConquest;

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
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (AlignmentSide)parent : new AlignmentSide();
            obj.id = Id;
            obj.nameId = NameId;
            obj.canConquest = CanConquest;
            return obj;
        
        }
    }
}