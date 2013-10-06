 


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
    [TableName("QuestObjectiveTypes")]
    [D2OClass("QuestObjectiveType")]
    public class QuestObjectiveTypeRecord : ID2ORecord
    {
        private const String MODULE = "QuestObjectiveTypes";
        public uint id;
        public uint nameId;

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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (QuestObjectiveType)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new QuestObjectiveType();
            obj.id = Id;
            obj.nameId = NameId;
            return obj;
        
        }
    }
}