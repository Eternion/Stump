using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Servers
{
    [Serializable]
    [ActiveRecord("server_community")]
    [AttributeAssociatedFile("ServerCommunities")]
    [D2OClass("ServerCommunity", "com.ankamagames.dofus.datacenter.servers")]
    public sealed class ServerCommunityRecord : DataBaseRecord<ServerCommunityRecord>
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

       [D2OField("defaultCountries")]
       [Property("DefaultCountries", ColumnType="Serializable")]
       public List<String> DefaultCountries
       {
           get;
           set;
       }

    }
}