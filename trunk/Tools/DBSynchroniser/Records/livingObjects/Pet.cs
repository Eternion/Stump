 


// Generated on 10/06/2013 01:10:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("Pets")]
    public class PetRecord : ID2ORecord
    {
        private const String MODULE = "Pets";
        public int id;
        public List<int> foodItems;
        public List<int> foodTypes;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

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
        public byte[] FoodItemsBin
        {
            get { return m_foodItemsBin; }
            set
            {
                m_foodItemsBin = value;
                foodItems = value == null ? null : value.ToObject<List<int>>();
            }
        }

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
        public byte[] FoodTypesBin
        {
            get { return m_foodTypesBin; }
            set
            {
                m_foodTypesBin = value;
                foodTypes = value == null ? null : value.ToObject<List<int>>();
            }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (Pet)obj;
            
            Id = castedObj.id;
            FoodItems = castedObj.foodItems;
            FoodTypes = castedObj.foodTypes;
        }
        
        public object CreateObject()
        {
            var obj = new Pet();
            
            obj.id = Id;
            obj.foodItems = FoodItems;
            obj.foodTypes = FoodTypes;
            return obj;
        
        }
    }
}