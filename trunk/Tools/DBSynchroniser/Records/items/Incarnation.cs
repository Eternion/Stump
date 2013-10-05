 


// Generated on 10/06/2013 01:10:58
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("Incarnation")]
    public class IncarnationRecord : ID2ORecord
    {
        private const String MODULE = "Incarnation";
        public uint id;
        public String lookMale;
        public String lookFemale;

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public String LookMale
        {
            get { return lookMale; }
            set { lookMale = value; }
        }

        public String LookFemale
        {
            get { return lookFemale; }
            set { lookFemale = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (Incarnation)obj;
            
            Id = castedObj.id;
            LookMale = castedObj.lookMale;
            LookFemale = castedObj.lookFemale;
        }
        
        public object CreateObject()
        {
            var obj = new Incarnation();
            
            obj.id = Id;
            obj.lookMale = LookMale;
            obj.lookFemale = LookFemale;
            return obj;
        
        }
    }
}