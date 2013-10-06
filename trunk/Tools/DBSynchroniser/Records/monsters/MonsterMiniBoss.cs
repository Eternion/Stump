 


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
    [TableName("MonsterMiniBoss")]
    [D2OClass("MonsterMiniBoss", "com.ankamagames.dofus.datacenter.monsters")]
    public class MonsterMiniBossRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "MonsterMiniBoss";
        public int id;
        public int monsterReplacingId;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public int MonsterReplacingId
        {
            get { return monsterReplacingId; }
            set { monsterReplacingId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (MonsterMiniBoss)obj;
            
            Id = castedObj.id;
            MonsterReplacingId = castedObj.monsterReplacingId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (MonsterMiniBoss)parent : new MonsterMiniBoss();
            obj.id = Id;
            obj.monsterReplacingId = MonsterReplacingId;
            return obj;
        
        }
    }
}