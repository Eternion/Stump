
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Pets")]
    [Serializable]
    public class Pet : IDataObject, IIndexedData
    {
        private const String MODULE = "Pets";
        public int id;
        public List<int> foodItems;
        public List<int> foodTypes;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public List<int> FoodItems
        {
            get { return foodItems; }
            set { foodItems = value; }
        }

        public List<int> FoodTypes
        {
            get { return foodTypes; }
            set { foodTypes = value; }
        }

    }
}