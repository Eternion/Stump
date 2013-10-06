 


// Generated on 10/06/2013 18:02:19
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("ServerPopulations")]
    [D2OClass("ServerPopulation", "com.ankamagames.dofus.datacenter.servers")]
    public class ServerPopulationRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "ServerPopulations";
        public int id;
        public uint nameId;
        public int weight;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
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
        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (ServerPopulation)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            Weight = castedObj.weight;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (ServerPopulation)parent : new ServerPopulation();
            obj.id = Id;
            obj.nameId = NameId;
            obj.weight = Weight;
            return obj;
        
        }
    }
}