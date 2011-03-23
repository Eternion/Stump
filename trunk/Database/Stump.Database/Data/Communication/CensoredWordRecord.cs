using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Communication
{
    [Serializable]
    [ActiveRecord("censored_word")]
    [AttributeAssociatedFile("CensoredWords")]
    [D2OClass("CensoredWord", "com.ankamagames.dofus.datacenter.communication")]
    public sealed class CensoredWordRecord : DataBaseRecord<CensoredWordRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public uint Id
       {
           get;
           set;
       }

       [D2OField("listId")]
       [Property("ListId")]
       public uint ListId
       {
           get;
           set;
       }

       [D2OField("language")]
       [Property("Language")]
       public String Language
       {
           get;
           set;
       }

       [D2OField("word")]
       [Property("Word")]
       public String Word
       {
           get;
           set;
       }

       [D2OField("deepLooking")]
       [Property("DeepLooking")]
       public Boolean DeepLooking
       {
           get;
           set;
       }

    }
}