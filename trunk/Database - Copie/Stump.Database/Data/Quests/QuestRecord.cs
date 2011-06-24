using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Quest
{
    [Serializable]
    [ActiveRecord("quest")]
    [AttributeAssociatedFile("Quests")]
    [D2OClass("Quest", "com.ankamagames.dofus.datacenter.quest")]
    public sealed class QuestRecord : DataBaseRecord<QuestRecord>
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

       [D2OField("stepIds")]
       [Property("StepIds", ColumnType="Serializable")]
       public List<uint> StepIds
       {
           get;
           set;
       }

    }
}