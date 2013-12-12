

// Generated on 12/12/2013 16:57:40
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
        public const String MODULE = "Recipes";
        public int resultId;
        [I18NField]
        public uint resultNameId;
        public uint resultTypeId;
        public uint resultLevel;
        public List<int> ingredientIds;
        public List<uint> quantities;
        public int jobId;
        public int skillId;
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
        public uint ResultNameId
        {
            get { return resultNameId; }
            set { resultNameId = value; }
        }
        [D2OIgnore]
        public uint ResultTypeId
        {
            get { return resultTypeId; }
            set { resultTypeId = value; }
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
        [D2OIgnore]
        public int JobId
        {
            get { return jobId; }
            set { jobId = value; }
        }
        [D2OIgnore]
        public int SkillId
        {
            get { return skillId; }
            set { skillId = value; }
        }
    }
}