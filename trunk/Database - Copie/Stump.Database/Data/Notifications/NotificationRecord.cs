using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Notifications
{
    [Serializable]
    [ActiveRecord("notification")]
    [AttributeAssociatedFile("Notifications")]
    [D2OClass("Notification", "com.ankamagames.dofus.datacenter.notifications")]
    public sealed class NotificationRecord : DataBaseRecord<NotificationRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("titleId")]
       [Property("TitleId")]
       public uint TitleId
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

       [D2OField("iconId")]
       [Property("IconId")]
       public int IconId
       {
           get;
           set;
       }

       [D2OField("typeId")]
       [Property("TypeId")]
       public int TypeId
       {
           get;
           set;
       }

       [D2OField("trigger")]
       [Property("Trigger")]
       public String Trigger
       {
           get;
           set;
       }

    }
}