using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Servers
{
    [Serializable]
    [ActiveRecord("server_population")]
    [AttributeAssociatedFile("ServerPopulations")]
    [D2OClass("ServerPopulation", "com.ankamagames.dofus.datacenter.servers")]
    public sealed class ServerPopulationRecord : DataBaseRecord<ServerPopulationRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("nameId")]
       [Property("NameId")]
       public uint NameId
       {
           get;
           set;
       }

       [D2OField("weight")]
       [Property("Weight")]
       public int Weight
       {
           get;
           set;
       }

    }
}