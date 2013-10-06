 


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
    [TableName("ServerPopulations")]
    [D2OClass("ServerPopulation")]
    public class ServerPopulationRecord : ID2ORecord
    {
        private const String MODULE = "ServerPopulations";
        public int id;
        public uint nameId;
        public int weight;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new ServerPopulation();
            obj.id = Id;
            obj.nameId = NameId;
            obj.weight = Weight;
            return obj;
        
        }
    }
}