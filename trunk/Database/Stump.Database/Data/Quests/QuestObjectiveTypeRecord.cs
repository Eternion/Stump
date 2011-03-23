using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Quest
{
    [Serializable]
    [ActiveRecord("quest_objective_type")]
    [AttributeAssociatedFile("QuestObjectiveTypes")]
    [D2OClass("QuestObjectiveType", "com.ankamagames.dofus.datacenter.quest")]
    public sealed class QuestObjectiveTypeRecord : DataBaseRecord<QuestObjectiveTypeRecord>
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

    }
}