using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.LivingObjects
{
    [Serializable]
    [ActiveRecord("speaking_items_trigger")]
    [AttributeAssociatedFile("SpeakingItemsTriggers")]
    [D2OClass("SpeakingItemsTrigger", "com.ankamagames.dofus.datacenter.livingObjects")]
    public sealed class SpeakingItemsTriggerRecord : DataBaseRecord<SpeakingItemsTriggerRecord>
    {

       [D2OField("triggersId")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("textIds")]
       [Property("TextIds", ColumnType="Serializable")]
       public List<int> TextIds
       {
           get;
           set;
       }

       [D2OField("states")]
       [Property("States", ColumnType="Serializable")]
       public List<int> States
       {
           get;
           set;
       }

    }
}