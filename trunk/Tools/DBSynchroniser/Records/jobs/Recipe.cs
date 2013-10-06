 


// Generated on 10/06/2013 14:22:00
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Recipes")]
    [D2OClass("Recipe")]
    public class RecipeRecord : ID2ORecord
    {
        private const String MODULE = "Recipes";
        public int resultId;
        public uint resultLevel;
        public List<int> ingredientIds;
        public List<uint> quantities;

        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
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
        public byte[] IngredientIdsBin
        {
            get { return m_ingredientIdsBin; }
            set
            {
                m_ingredientIdsBin = value;
                ingredientIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new Recipe();
            obj.resultId = ResultId;
            obj.resultLevel = ResultLevel;
            obj.ingredientIds = IngredientIds;
            obj.quantities = Quantities;
            return obj;
        
        }
    }
}