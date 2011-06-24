using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Npcs
{
    [Serializable]
    [ActiveRecord("npc_action")]
    [AttributeAssociatedFile("NpcActions")]
    [D2OClass("NpcAction", "com.ankamagames.dofus.datacenter.npcs")]
    public sealed class NpcActionRecord : DataBaseRecord<NpcActionRecord>
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