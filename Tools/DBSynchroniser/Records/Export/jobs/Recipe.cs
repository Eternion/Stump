 


// Generated on 11/16/2015 14:26:41
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Recipes")]
    [D2OClass("Recipe", "com.ankamagames.dofus.datacenter.jobs")]
    public class RecipeRecord : ID2ORecord, ISaveIntercepter
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

        int ID2ORecord.Id
        {
            get { return (int)resultId; }
        }


        [D2OIgnore]
        [PrimaryKey("ResultId", false)]
        public int ResultId
        {
            get { return resultId; }
            set { resultId = value; }
        }

        [D2OIgnore]
        [I18NField]
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
        [Ignore]
        public List<int> IngredientIds
        {
            get { return ingredientIds; }
            set
            {
                ingredientIds = value;
                m_ingredientIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_ingredientIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] IngredientIdsBin
        {
            get { return m_ingredientIdsBin; }
            set
            {
                m_ingredientIdsBin = value;
                ingredientIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> Quantities
        {
            get { return quantities; }
            set
            {
                quantities = value;
                m_quantitiesBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_quantitiesBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] QuantitiesBin
        {
            get { return m_quantitiesBin; }
            set
            {
                m_quantitiesBin = value;
                quantities = value == null ? null : value.ToObject<List<uint>>();
            }
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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Recipe)obj;
            
            ResultId = castedObj.resultId;
            ResultNameId = castedObj.resultNameId;
            ResultTypeId = castedObj.resultTypeId;
            ResultLevel = castedObj.resultLevel;
            IngredientIds = castedObj.ingredientIds;
            Quantities = castedObj.quantities;
            JobId = castedObj.jobId;
            SkillId = castedObj.skillId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Recipe)parent : new Recipe();
            obj.resultId = ResultId;
            obj.resultNameId = ResultNameId;
            obj.resultTypeId = ResultTypeId;
            obj.resultLevel = ResultLevel;
            obj.ingredientIds = IngredientIds;
            obj.quantities = Quantities;
            obj.jobId = JobId;
            obj.skillId = SkillId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_ingredientIdsBin = ingredientIds == null ? null : ingredientIds.ToBinary();
            m_quantitiesBin = quantities == null ? null : quantities.ToBinary();
        
        }
    }
}