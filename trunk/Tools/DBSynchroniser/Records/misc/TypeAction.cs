 


// Generated on 10/06/2013 14:22:00
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
    [D2OClass("TypeAction")]
    public class TypeActionRecord : ID2ORecord
    {
        public const String MODULE = "TypeActions";
        public int id;
        public String elementName;
        public int elementId;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [NullString]
        public String ElementName
        {
            get { return elementName; }
            set { elementName = value; }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new TypeAction();
            obj.id = Id;
            obj.elementName = ElementName;
            obj.elementId = ElementId;
            return obj;
        
        }
    }
}