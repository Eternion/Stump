
using System.Collections.Generic;

namespace Stump.Server.DataProvider.Data.Recipe
{
    public class RecipeTemplate
    {
        public int ResultId { get; set; }

        public uint ResultLevel { get; set; }

        public List<IngredientTemplate> Ingredients { get; set; }
    }
}