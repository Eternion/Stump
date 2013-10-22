 


// Generated on 10/19/2013 17:17:45
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
    [TableName("MonsterSuperRaces")]
    [D2OClass("MonsterSuperRace", "com.ankamagames.dofus.datacenter.monsters")]
    public class MonsterSuperRaceRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "MonsterSuperRaces";
        public int id;
        public uint nameId;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (MonsterSuperRace)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (MonsterSuperRace)parent : new MonsterSuperRace();
            obj.id = Id;
            obj.nameId = NameId;
            return obj;
        
        }
    }
}