 


// Generated on 10/13/2013 12:21:16
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
    public class RecipeRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "Recipes";
        public int resultId;
        public uint resultLevel;
        public List<int> ingredientIds;
        public List<uint> quantities;

        [D2OIgnore]
        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Recipe)obj;
            
            ResultId = castedObj.resultId;
            ResultLevel = castedObj.resultLevel;
            IngredientIds = castedObj.ingredientIds;
            Quantities = castedObj.quantities;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (Recipe)parent : new Recipe();
            obj.resultId = ResultId;
            obj.resultLevel = ResultLevel;
            obj.ingredientIds = IngredientIds;
            obj.quantities = Quantities;
            return obj;
        
        }
    }
}