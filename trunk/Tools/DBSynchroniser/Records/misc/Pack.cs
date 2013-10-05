 


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
    [D2OClass("Pack")]
    public class PackRecord : ID2ORecord
    {
        private const String MODULE = "Pack";
        public int id;
        public String name;
        public Boolean hasSubAreas;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public Boolean HasSubAreas
        {
            get { return hasSubAreas; }
            set { hasSubAreas = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (Pack)obj;
            
            Id = castedObj.id;
            Name = castedObj.name;
            HasSubAreas = castedObj.hasSubAreas;
        }
        
        public object CreateObject()
        {
            var obj = new Pack();
            
            obj.id = Id;
            obj.name = Name;
            obj.hasSubAreas = HasSubAreas;
            return obj;
        
        }
    }
}