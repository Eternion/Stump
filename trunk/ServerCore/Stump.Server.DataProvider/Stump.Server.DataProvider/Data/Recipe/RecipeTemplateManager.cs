// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Stump.Server.DataProvider.Core;
using Stump.Server.DataProvider.Data.D2oTool;

namespace Stump.Server.DataProvider.Data.Recipe
{
    public class RecipeTemplateManager : DataManager<int, RecipeTemplate>
    {
        protected override RecipeTemplate InternalGetOne(int id)
        {
            var recipe = D2OLoader.LoadData<DofusProtocol.D2oClasses.Recipe>(id);

            if (recipe == null)
                return null;

            return new RecipeTemplate
                       {
                           ResultId = recipe.resultId,
                           ResultLevel = recipe.resultLevel,
                           Ingredients = recipe.ingredientIds.Select((t, i) => new IngredientTemplate { IngredientId = t, Quantity = recipe.quantities[i] }).ToList()
                       };
        }

        protected override Dictionary<int, RecipeTemplate> InternalGetAll()
        {
            return D2OLoader.LoadData<DofusProtocol.D2oClasses.Recipe>().
                Select(r => new RecipeTemplate
                                {
                                    ResultId = r.resultId,
                                    ResultLevel = r.resultLevel,
                                    Ingredients = r.ingredientIds.Select((t, i) => new IngredientTemplate { IngredientId = t, Quantity = r.quantities[i] }).ToList()
                                }).ToDictionary(r => r.ResultId);
        }
    }
}