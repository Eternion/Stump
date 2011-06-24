using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Communication
{
    [Serializable]
    [ActiveRecord("info_message")]
    [AttributeAssociatedFile("InfoMessages")]
    [D2OClass("InfoMessage", "com.ankamagames.dofus.datacenter.communication")]
    public sealed class InfoMessageRecord : DataBaseRecord<InfoMessageRecord>
    {

       [D2OField("typeId")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public uint Id
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

       [D2OField("textId")]
       [Property("TextId")]
       public uint TextId
       {
           get;
           set;
       }

    }
}