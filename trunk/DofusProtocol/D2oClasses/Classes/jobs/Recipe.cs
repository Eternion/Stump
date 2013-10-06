

// Generated on 10/06/2013 17:58:55
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Recipe", "com.ankamagames.dofus.datacenter.jobs")]
    [Serializable]
    public class Recipe : IDataObject
    {
        private const String MODULE = "Recipes";
        public int resultId;
        public uint resultLevel;
        public List<int> ingredientIds;
        public List<uint> quantities;
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