using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.LivingObjects
{
    [Serializable]
    [ActiveRecord("living_object_skin_jnt_mood")]
    [AttributeAssociatedFile("LivingObjectSkinJntMood")]
    [D2OClass("LivingObjectSkinJntMood", "com.ankamagames.dofus.datacenter.livingObjects")]
    public sealed class LivingObjectSkinJntMoodRecord : DataBaseRecord<LivingObjectSkinJntMoodRecord>
    {

       [D2OField("skinId")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("moods")]
       [Property("Moods", ColumnType="Serializable")]
       public List<List<int>> Moods
       {
           get;
           set;
       }

    }
}