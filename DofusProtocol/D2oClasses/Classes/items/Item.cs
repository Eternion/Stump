

// Generated on 10/28/2013 14:03:18
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Item", "com.ankamagames.dofus.datacenter.items")]
    [Serializable]
    public class Item : IDataObject, IIndexedData
    {
        private const String MODULE = "Items";
        public const uint EQUIPEMENT_CATEGORY = 0;
        public const uint CONSUMABLES_CATEGORY = 1;
        public const uint RESSOURCES_CATEGORY = 2;
        public const uint QUEST_CATEGORY = 3;
        public const uint OTHER_CATEGORY = 4;
        public int id;
        [I18NField]
        public uint nameId;
        public uint typeId;
        [I18NField]
        public uint descriptionId;
        public int iconId;
        public uint level;
        public uint realWeight;
        public Boolean cursed;
        public int useAnimationId;
        public Boolean usable;
        public Boolean targetable;
        public double price;
        public Boolean twoHanded;
        public Boolean etheral;
        public int itemSetId;
        public String criteria;
        public String criteriaTarget;
        public Boolean hideEffects;
        public Boolean enhanceable;
        public Boolean nonUsableOnAnother;
        public uint appearanceId;
        public Boolean secretRecipe;
        public List<uint> recipeIds;
        public Boolean bonusIsSecret;
        public List<EffectInstance> possibleEffects;
        public List<uint> favoriteSubAreas;
        public uint favoriteSubAreasBonus;
        public uint weight;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }
        [D2OIgnore]
        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }
        [D2OIgnore]
        public uint Level
        {
            get { return level; }
            set { level = value; }
        }
        [D2OIgnore]
        public uint RealWeight
        {
            get { return realWeight; }
            set { realWeight = value; }
        }
        [D2OIgnore]
        public Boolean Cursed
        {
            get { return cursed; }
            set { cursed = value; }
        }
        [D2OIgnore]
        public int UseAnimationId
        {
            get { return useAnimationId; }
            set { useAnimationId = value; }
        }
        [D2OIgnore]
        public Boolean Usable
        {
            get { return usable; }
            set { usable = value; }
        }
        [D2OIgnore]
        public Boolean Targetable
        {
            get { return targetable; }
            set { targetable = value; }
        }
        [D2OIgnore]
        public double Price
        {
            get { return price; }
            set { price = value; }
        }
        [D2OIgnore]
        public Boolean TwoHanded
        {
            get { return twoHanded; }
            set { twoHanded = value; }
        }
        [D2OIgnore]
        public Boolean Etheral
        {
            get { return etheral; }
            set { etheral = value; }
        }
        [D2OIgnore]
        public int ItemSetId
        {
            get { return itemSetId; }
            set { itemSetId = value; }
        }
        [D2OIgnore]
        public String Criteria
        {
            get { return criteria; }
            set { criteria = value; }
        }
        [D2OIgnore]
        public String CriteriaTarget
        {
            get { return criteriaTarget; }
            set { criteriaTarget = value; }
        }
        [D2OIgnore]
        public Boolean HideEffects
        {
            get { return hideEffects; }
            set { hideEffects = value; }
        }
        [D2OIgnore]
        public Boolean Enhanceable
        {
            get { return enhanceable; }
            set { enhanceable = value; }
        }
        [D2OIgnore]
        public Boolean NonUsableOnAnother
        {
            get { return nonUsableOnAnother; }
            set { nonUsableOnAnother = value; }
        }
        [D2OIgnore]
        public uint AppearanceId
        {
            get { return appearanceId; }
            set { appearanceId = value; }
        }
        [D2OIgnore]
        public Boolean SecretRecipe
        {
            get { return secretRecipe; }
            set { secretRecipe = value; }
        }
        [D2OIgnore]
        public List<uint> RecipeIds
        {
            get { return recipeIds; }
            set { recipeIds = value; }
        }
        [D2OIgnore]
        public Boolean BonusIsSecret
        {
            get { return bonusIsSecret; }
            set { bonusIsSecret = value; }
        }
        [D2OIgnore]
        public List<EffectInstance> PossibleEffects
        {
            get { return possibleEffects; }
            set { possibleEffects = value; }
        }
        [D2OIgnore]
        public List<uint> FavoriteSubAreas
        {
            get { return favoriteSubAreas; }
            set { favoriteSubAreas = value; }
        }
        [D2OIgnore]
        public uint FavoriteSubAreasBonus
        {
            get { return favoriteSubAreasBonus; }
            set { favoriteSubAreasBonus = value; }
        }
        [D2OIgnore]
        public uint Weight
        {
            get { return weight; }
            set { weight = value; }
        }
    }
}