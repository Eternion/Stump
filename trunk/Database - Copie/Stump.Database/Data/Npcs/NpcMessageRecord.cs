using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Npcs
{
    [Serializable]
    [ActiveRecord("npc_message")]
    [AttributeAssociatedFile("NpcMessages")]
    [D2OClass("NpcMessage", "com.ankamagames.dofus.datacenter.npcs")]
    public sealed class NpcMessageRecord : DataBaseRecord<NpcMessageRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("messageId")]
       [Property("MessageId")]
       public uint MessageId
       {
           get;
           set;
       }

       [D2OField("messageParams")]
       [Property("MessageParams")]
       public String MessageParams
       {
           get;
           set;
       }

    }
}