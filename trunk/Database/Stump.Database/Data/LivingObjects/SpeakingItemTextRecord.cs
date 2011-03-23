using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.LivingObjects
{
    [Serializable]
    [ActiveRecord("speaking_item_text")]
    [AttributeAssociatedFile("SpeakingItemsText")]
    [D2OClass("SpeakingItemText", "com.ankamagames.dofus.datacenter.livingObjects")]
    public sealed class SpeakingItemTextRecord : DataBaseRecord<SpeakingItemTextRecord>
    {

       [D2OField("textId")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("textProba")]
       [Property("TextProba")]
       public double TextProba
       {
           get;
           set;
       }

       [D2OField("textStringId")]
       [Property("TextStringId")]
       public uint TextStringId
       {
           get;
           set;
       }

       [D2OField("textLevel")]
       [Property("TextLevel")]
       public int TextLevel
       {
           get;
           set;
       }

       [D2OField("textSound")]
       [Property("TextSound")]
       public int TextSound
       {
           get;
           set;
       }

       [D2OField("textRestriction")]
       [Property("TextRestriction")]
       public String TextRestriction
       {
           get;
           set;
       }

    }
}