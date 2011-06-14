
using System.Collections.Generic;
using System.Linq;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.Recipe
{
    //public class RecipeTemplateManager : DataManager<int, RecipeTemplate>
    //{
    //    protected override RecipeTemplate InternalGetOne(int id)
    //    {
    //        var recipe = D2OLoader.LoadData<DofusProtocol.D2oClasses.Recipe>(id);

    //        if (recipe == null)
    //            return null;

    //        return new RecipeTemplate
    //                   {
    //                       ResultId = recipe.resultId,
    //                       ResultLevel = recipe.resultLevel,
    //                       Ingredients = recipe.ingredientIds.Select((t, i) => new IngredientTemplate { IngredientId = t, Quantity = recipe.quantities[i] }).ToList()
    //                   };
    //    }

    //    protected override Dictionary<int, RecipeTemplate> InternalGetAll()
    //    {
    //        return D2OLoader.LoadData<DofusProtocol.D2oClasses.Recipe>().
    //            Select(r => new RecipeTemplate
    //                            {
    //                                ResultId = r.resultId,
    //                                ResultLevel = r.resultLevel,
    //                                Ingredients = r.ingredientIds.Select((t, i) => new IngredientTemplate { IngredientId = t, Quantity = r.quantities[i] }).ToList()
    //                            }).ToDictionary(r => r.ResultId);
    //    }
  //  }
}