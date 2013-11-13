

// Generated on 10/28/2013 14:03:19
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Recipe", "com.ankamagames.dofus.datacenter.jobs")]
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
        [D2OIgnore]
        public int ResultId
        {
            get { return resultId; }
            set { resultId = value; }
        }
        [D2OIgnore]
        public uint ResultLevel
        {
            get { return resultLevel; }
            set { resultLevel = value; }
        }
        [D2OIgnore]
        public List<int> IngredientIds
        {
            get { return ingredientIds; }
            set { ingredientIds = value; }
        }
        [D2OIgnore]
        public List<uint> Quantities
        {
            get { return quantities; }
            set { quantities = value; }
        }
    }
}