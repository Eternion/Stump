 


// Generated on 11/02/2013 14:55:49
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
    [TableName("Pets")]
    [D2OClass("Pet", "com.ankamagames.dofus.datacenter.livingObjects")]
    public class PetRecord : ID2ORecord, ISaveIntercepter
    {
        private const String MODULE = "Pets";
        public int id;
        public List<int> foodItems;
        public List<int> foodTypes;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> FoodItems
        {
            get { return foodItems; }
            set
            {
                foodItems = value;
                m_foodItemsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_foodItemsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] FoodItemsBin
        {
            get { return m_foodItemsBin; }
            set
            {
                m_foodItemsBin = value;
                foodItems = value == null ? null : value.ToObject<List<int>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> FoodTypes
        {
            get { return foodTypes; }
            set
            {
                foodTypes = value;
                m_foodTypesBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_foodTypesBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] FoodTypesBin
        {
            get { return m_foodTypesBin; }
            set
            {
                m_foodTypesBin = value;
                foodTypes = value == null ? null : value.ToObject<List<int>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Pet)obj;
            
            Id = castedObj.id;
            FoodItems = castedObj.foodItems;
            FoodTypes = castedObj.foodTypes;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Pet)parent : new Pet();
            obj.id = Id;
            obj.foodItems = FoodItems;
            obj.foodTypes = FoodTypes;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_foodItemsBin = foodItems == null ? null : foodItems.ToBinary();
            m_foodTypesBin = foodTypes == null ? null : foodTypes.ToBinary();
        
        }
    }
}