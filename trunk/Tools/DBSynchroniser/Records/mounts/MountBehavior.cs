 


// Generated on 10/06/2013 14:22:01
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("MountBehaviors")]
    [D2OClass("MountBehavior")]
    public class MountBehaviorRecord : ID2ORecord
    {
        public const String MODULE = "MountBehaviors";
        public uint id;
        public uint nameId;
        public uint descriptionId;

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (MountBehavior)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            DescriptionId = castedObj.descriptionId;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new MountBehavior();
            obj.id = Id;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            return obj;
        
        }
    }
}