

// Generated on 04/24/2015 03:38:27
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
            get { return this.resultId; }
            set { this.resultId = value; }
        }
        [D2OIgnore]
        public uint ResultNameId
        {
            get { return this.resultNameId; }
            set { this.resultNameId = value; }
        }
        [D2OIgnore]
        public uint ResultTypeId
        {
            get { return this.resultTypeId; }
            set { this.resultTypeId = value; }
        }
        [D2OIgnore]
        public uint ResultLevel
        {
            get { return this.resultLevel; }
            set { this.resultLevel = value; }
        }
        [D2OIgnore]
        public List<int> IngredientIds
        {
            get { return this.ingredientIds; }
            set { this.ingredientIds = value; }
        }
        [D2OIgnore]
        public List<uint> Quantities
        {
            get { return this.quantities; }
            set { this.quantities = value; }
        }
        [D2OIgnore]
        public int JobId
        {
            get { return this.jobId; }
            set { this.jobId = value; }
        }
        [D2OIgnore]
        public int SkillId
        {
            get { return this.skillId; }
            set { this.skillId = value; }
        }
    }
}