 


// Generated on 10/13/2013 12:21:16
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
    [TableName("MountBehaviors")]
    [D2OClass("MountBehavior", "com.ankamagames.dofus.datacenter.mounts")]
    public class MountBehaviorRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        public const String MODULE = "MountBehaviors";
        public uint id;
        public uint nameId;
        public uint descriptionId;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
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
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (MountBehavior)parent : new MountBehavior();
            obj.id = Id;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            return obj;
        
        }
    }
}