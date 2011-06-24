using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Servers
{
    [Serializable]
    [ActiveRecord("server_game_type")]
    [AttributeAssociatedFile("ServerGameTypes")]
    [D2OClass("ServerGameType", "com.ankamagames.dofus.datacenter.servers")]
    public sealed class ServerGameTypeRecord : DataBaseRecord<ServerGameTypeRecord>
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

    }
}