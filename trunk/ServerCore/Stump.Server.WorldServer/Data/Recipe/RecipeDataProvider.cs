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
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Initializing;

namespace Stump.Server.WorldServer.Data.RecipeData
{
    public static class RecipeDataProvider
    {
        private static Dictionary<int, Recipe> m_recipes;


        [StageStep(Stages.One, "Loaded Recipes")]
        public static void LoadRecipes()
        {
            m_recipes = DataLoader.LoadData<Recipe>().ToDictionary(r => r.resultId);
        }

        public static Recipe GetRecipe(int resultId)
        {
            if (m_recipes.ContainsKey(resultId))
                return m_recipes[resultId];
            return null;
        }
    }
}