 


// Generated on 10/06/2013 18:02:17
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Incarnation")]
    [D2OClass("Incarnation", "com.ankamagames.dofus.datacenter.items")]
    public class IncarnationRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "Incarnation";
        public uint id;
        public String lookMale;
        public String lookFemale;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [NullString]
        public String LookMale
        {
            get { return lookMale; }
            set { lookMale = value; }
        }

        [D2OIgnore]
        [NullString]
        public String LookFemale
        {
            get { return lookFemale; }
            set { lookFemale = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Incarnation)obj;
            
            Id = castedObj.id;
            LookMale = castedObj.lookMale;
            LookFemale = castedObj.lookFemale;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (Incarnation)parent : new Incarnation();
            obj.id = Id;
            obj.lookMale = LookMale;
            obj.lookFemale = LookFemale;
            return obj;
        
        }
    }
}