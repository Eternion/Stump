
// Generated on 03/25/2013 19:24:36
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Recipes")]
    [Serializable]
    public class Recipe : IDataObject, IIndexedData
    {
        private const String MODULE = "Recipes";
        public int resultId;
        public uint resultLevel;
        public List<int> ingredientIds;
        public List<uint> quantities;

        int IIndexedData.Id
        {
            get { return (int)resultId; }
        }

        public int ResultId
        {
            get { return resultId; }
            set { resultId = value; }
        }

        public uint ResultLevel
        {
            get { return resultLevel; }
            set { resultLevel = value; }
        }

        public List<int> IngredientIds
        {
            get { return ingredientIds; }
            set { ingredientIds = value; }
        }

        public List<uint> Quantities
        {
            get { return quantities; }
            set { quantities = value; }
        }

    }
}