 


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
    [D2OClass("IncarnationLevels")]
    public class IncarnationLevelRecord : ID2ORecord
    {
        private const String MODULE = "IncarnationLevels";
        public int id;
        public int incarnationId;
        public int level;
        public uint requiredXp;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int IncarnationId
        {
            get { return incarnationId; }
            set { incarnationId = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public uint RequiredXp
        {
            get { return requiredXp; }
            set { requiredXp = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (IncarnationLevel)obj;
            
            Id = castedObj.id;
            IncarnationId = castedObj.incarnationId;
            Level = castedObj.level;
            RequiredXp = castedObj.requiredXp;
        }
        
        public object CreateObject()
        {
            var obj = new IncarnationLevel();
            
            obj.id = Id;
            obj.incarnationId = IncarnationId;
            obj.level = Level;
            obj.requiredXp = RequiredXp;
            return obj;
        
        }
    }
}