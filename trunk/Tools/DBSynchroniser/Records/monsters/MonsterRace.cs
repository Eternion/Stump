 


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
    [D2OClass("MonsterRaces")]
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

        public void AssignFields(object obj)
        {
            var castedObj = (MonsterRace)obj;
            
            Id = castedObj.id;
            SuperRaceId = castedObj.superRaceId;
            NameId = castedObj.nameId;
        }
        
        public object CreateObject()
        {
            var obj = new MonsterRace();
            
            obj.id = Id;
            obj.superRaceId = SuperRaceId;
            obj.nameId = NameId;
            return obj;
        
        }
    }
}