

// Generated on 09/02/2014 22:34:36
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Pet", "com.ankamagames.dofus.datacenter.livingObjects")]
    [Serializable]
    public class Pet : IDataObject, IIndexedData
    {
        public const String MODULE = "Pets";
        public int id;
        public List<int> foodItems;
        public List<int> foodTypes;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public List<int> FoodItems
        {
            get { return this.foodItems; }
            set { this.foodItems = value; }
        }
        [D2OIgnore]
        public List<int> FoodTypes
        {
            get { return this.foodTypes; }
            set { this.foodTypes = value; }
        }
    }
}