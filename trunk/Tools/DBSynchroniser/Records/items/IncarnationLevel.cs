 


// Generated on 10/13/2013 12:21:15
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
    [TableName("IncarnationLevels")]
    [D2OClass("IncarnationLevel", "com.ankamagames.dofus.datacenter.items")]
    public class IncarnationLevelRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "IncarnationLevels";
        public int id;
        public int incarnationId;
        public int level;
        public uint requiredXp;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public int IncarnationId
        {
            get { return incarnationId; }
            set { incarnationId = value; }
        }

        [D2OIgnore]
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        [D2OIgnore]
        public uint RequiredXp
        {
            get { return requiredXp; }
            set { requiredXp = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (IncarnationLevel)obj;
            
            Id = castedObj.id;
            IncarnationId = castedObj.incarnationId;
            Level = castedObj.level;
            RequiredXp = castedObj.requiredXp;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (IncarnationLevel)parent : new IncarnationLevel();
            obj.id = Id;
            obj.incarnationId = IncarnationId;
            obj.level = Level;
            obj.requiredXp = RequiredXp;
            return obj;
        
        }
    }
}