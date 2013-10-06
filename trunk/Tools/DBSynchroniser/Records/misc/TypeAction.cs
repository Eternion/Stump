 


// Generated on 10/06/2013 18:02:18
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("TypeActions")]
    [D2OClass("TypeAction", "com.ankamagames.dofus.datacenter.misc")]
    public class TypeActionRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        public const String MODULE = "TypeActions";
        public int id;
        public String elementName;
        public int elementId;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [NullString]
        public String ElementName
        {
            get { return elementName; }
            set { elementName = value; }
        }

        [D2OIgnore]
        public int ElementId
        {
            get { return elementId; }
            set { elementId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (TypeAction)obj;
            
            Id = castedObj.id;
            ElementName = castedObj.elementName;
            ElementId = castedObj.elementId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (TypeAction)parent : new TypeAction();
            obj.id = Id;
            obj.elementName = ElementName;
            obj.elementId = ElementId;
            return obj;
        
        }
    }
}