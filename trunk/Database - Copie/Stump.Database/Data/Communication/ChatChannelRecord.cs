using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Communication
{
    [Serializable]
    [ActiveRecord("chat_channel")]
    [AttributeAssociatedFile("ChatChannels")]
    [D2OClass("ChatChannel", "com.ankamagames.dofus.datacenter.communication")]
    public sealed class ChatChannelRecord : DataBaseRecord<ChatChannelRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public uint Id
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

       [D2OField("descriptionId")]
       [Property("DescriptionId")]
       public uint DescriptionId
       {
           get;
           set;
       }

       [D2OField("shortcut")]
       [Property("Shortcut")]
       public String Shortcut
       {
           get;
           set;
       }

       [D2OField("shortcutKey")]
       [Property("ShortcutKey")]
       public String ShortcutKey
       {
           get;
           set;
       }

       [D2OField("isPrivate")]
       [Property("IsPrivate")]
       public Boolean IsPrivate
       {
           get;
           set;
       }

       [D2OField("allowObjects")]
       [Property("AllowObjects")]
       public Boolean AllowObjects
       {
           get;
           set;
       }

    }
}