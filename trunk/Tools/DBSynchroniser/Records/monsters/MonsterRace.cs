 


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
    [TableName("MonsterRaces")]
    [D2OClass("MonsterRace")]
    public class MonsterRaceRecord : ID2ORecord
    {
        private const String MODULE = "MonsterRaces";
        public int id;
        public int superRaceId;
        public uint nameId;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int SuperRaceId
        {
            get { return superRaceId; }
            set { superRaceId = value; }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new MonsterRace();
            obj.id = Id;
            obj.superRaceId = SuperRaceId;
            obj.nameId = NameId;
            return obj;
        
        }
    }
}