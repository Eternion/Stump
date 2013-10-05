 


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
    [D2OClass("QuestObjectiveTypes")]
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

        public void AssignFields(object obj)
        {
            var castedObj = (QuestObjectiveType)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
        }
        
        public object CreateObject()
        {
            var obj = new QuestObjectiveType();
            
            obj.id = Id;
            obj.nameId = NameId;
            return obj;
        
        }
    }
}