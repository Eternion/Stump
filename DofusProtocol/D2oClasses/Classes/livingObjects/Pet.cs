

// Generated on 02/02/2016 14:15:04
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
        public int minDurationBeforeMeal;
        public int maxDurationBeforeMeal;
        public List<EffectInstance> possibleEffects;
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
        [D2OIgnore]
        public int MinDurationBeforeMeal
        {
            get { return this.minDurationBeforeMeal; }
            set { this.minDurationBeforeMeal = value; }
        }
        [D2OIgnore]
        public int MaxDurationBeforeMeal
        {
            get { return this.maxDurationBeforeMeal; }
            set { this.maxDurationBeforeMeal = value; }
        }
        [D2OIgnore]
        public List<EffectInstance> PossibleEffects
        {
            get { return this.possibleEffects; }
            set { this.possibleEffects = value; }
        }
    }
}