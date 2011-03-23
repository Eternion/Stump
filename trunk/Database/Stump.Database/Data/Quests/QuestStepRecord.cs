using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Quest
{
    [Serializable]
    [ActiveRecord("quest_step")]
    [AttributeAssociatedFile("QuestSteps")]
    [D2OClass("QuestStep", "com.ankamagames.dofus.datacenter.quest")]
    public sealed class QuestStepRecord : DataBaseRecord<QuestStepRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public uint Id
       {
           get;
           set;
       }

       [D2OField("questId")]
       [Property("QuestId")]
       public uint QuestId
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

       [D2OField("dialogId")]
       [Property("DialogId")]
       public int DialogId
       {
           get;
           set;
       }

       [D2OField("optimalLevel")]
       [Property("OptimalLevel")]
       public uint OptimalLevel
       {
           get;
           set;
       }

       [D2OField("experienceReward")]
       [Property("ExperienceReward")]
       public uint ExperienceReward
       {
           get;
           set;
       }

       [D2OField("kamasReward")]
       [Property("KamasReward")]
       public uint KamasReward
       {
           get;
           set;
       }

       [D2OField("itemsReward")]
       [Property("ItemsReward", ColumnType="Serializable")]
       public List<List<uint>> ItemsReward
       {
           get;
           set;
       }

       [D2OField("emotesReward")]
       [Property("EmotesReward", ColumnType="Serializable")]
       public List<uint> EmotesReward
       {
           get;
           set;
       }

       [D2OField("jobsReward")]
       [Property("JobsReward", ColumnType="Serializable")]
       public List<uint> JobsReward
       {
           get;
           set;
       }

       [D2OField("spellsReward")]
       [Property("SpellsReward", ColumnType="Serializable")]
       public List<uint> SpellsReward
       {
           get;
           set;
       }

       [D2OField("objectiveIds")]
       [Property("ObjectiveIds", ColumnType="Serializable")]
       public List<uint> ObjectiveIds
       {
           get;
           set;
       }

    }
}