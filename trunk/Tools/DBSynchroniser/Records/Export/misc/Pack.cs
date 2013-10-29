 


// Generated on 10/28/2013 14:03:24
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
    [TableName("Pack")]
    [D2OClass("Pack", "com.ankamagames.dofus.datacenter.misc")]
    public class PackRecord : ID2ORecord
    {
        private const String MODULE = "Pack";
        public int id;
        public String name;
        public Boolean hasSubAreas;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [NullString]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        [D2OIgnore]
        public Boolean HasSubAreas
        {
            get { return hasSubAreas; }
            set { hasSubAreas = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Pack)obj;
            
            Id = castedObj.id;
            Name = castedObj.name;
            HasSubAreas = castedObj.hasSubAreas;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (Pack)parent : new Pack();
            obj.id = Id;
            obj.name = Name;
            obj.hasSubAreas = HasSubAreas;
            return obj;
        
        }
    }
}