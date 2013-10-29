 


// Generated on 10/28/2013 14:03:25
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
    [TableName("MonsterRaces")]
    [D2OClass("MonsterRace", "com.ankamagames.dofus.datacenter.monsters")]
    public class MonsterRaceRecord : ID2ORecord
    {
        private const String MODULE = "MonsterRaces";
        public int id;
        public int superRaceId;
        [I18NField]
        public uint nameId;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public int SuperRaceId
        {
            get { return superRaceId; }
            set { superRaceId = value; }
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
            var castedObj = (MonsterRace)obj;
            
            Id = castedObj.id;
            SuperRaceId = castedObj.superRaceId;
            NameId = castedObj.nameId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (MonsterRace)parent : new MonsterRace();
            obj.id = Id;
            obj.superRaceId = SuperRaceId;
            obj.nameId = NameId;
            return obj;
        
        }
    }
}