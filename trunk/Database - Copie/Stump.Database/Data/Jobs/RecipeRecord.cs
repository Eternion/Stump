using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Jobs
{
    [Serializable]
    [ActiveRecord("recipe")]
    [AttributeAssociatedFile("Recipes")]
    [D2OClass("Recipe", "com.ankamagames.dofus.datacenter.jobs")]
    public sealed class RecipeRecord : DataBaseRecord<RecipeRecord>
    {

       [D2OField("resultId")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("resultLevel")]
       [Property("ResultLevel")]
       public uint ResultLevel
       {
           get;
           set;
       }

       [D2OField("ingredientIds")]
       [Property("IngredientIds", ColumnType="Serializable")]
       public List<int> IngredientIds
       {
           get;
           set;
       }

       [D2OField("quantities")]
       [Property("Quantities", ColumnType="Serializable")]
       public List<uint> Quantities
       {
           get;
           set;
       }

    }
}