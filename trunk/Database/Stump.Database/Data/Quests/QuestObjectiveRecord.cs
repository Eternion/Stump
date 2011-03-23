using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Quest
{
    [Serializable]
    [ActiveRecord("quest_objective")]
    [AttributeAssociatedFile("QuestObjectives")]
    [D2OClass("QuestObjective", "com.ankamagames.dofus.datacenter.quest")]
    public sealed class QuestObjectiveRecord : DataBaseRecord<QuestObjectiveRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public uint Id
       {
           get;
           set;
       }

       [D2OField("stepId")]
       [Property("StepId")]
       public uint StepId
       {
           get;
           set;
       }

       [D2OField("typeId")]
       [Property("TypeId")]
       public uint TypeId
       {
           get;
           set;
       }

       [D2OField("parameters")]
       [Property("Parameters", ColumnType="Serializable")]
       public List<uint> Parameters
       {
           get;
           set;
       }

       [D2OField("coords")]
       [Property("Coords", ColumnType="Serializable")]
       public Point Coords
       {
           get;
           set;
       }

    }
}