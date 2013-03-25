
// Generated on 03/25/2013 19:24:34
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Items")]
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
        public uint nameId;
        public uint typeId;
        public uint descriptionId;
        public int iconId;
        public uint level;
        public uint realWeight;
        public Boolean cursed;
        public int useAnimationId;
        public Boolean usable;
        public Boolean targetable;
        public float price;
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
        public ItemType type;
        public uint weight;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        public uint Level
        {
            get { return level; }
            set { level = value; }
        }

        public uint RealWeight
        {
            get { return realWeight; }
            set { realWeight = value; }
        }

        public Boolean Cursed
        {
            get { return cursed; }
            set { cursed = value; }
        }

        public int UseAnimationId
        {
            get { return useAnimationId; }
            set { useAnimationId = value; }
        }

        public Boolean Usable
        {
            get { return usable; }
            set { usable = value; }
        }

        public Boolean Targetable
        {
            get { return targetable; }
            set { targetable = value; }
        }

        public float Price
        {
            get { return price; }
            set { price = value; }
        }

        public Boolean TwoHanded
        {
            get { return twoHanded; }
            set { twoHanded = value; }
        }

        public Boolean Etheral
        {
            get { return etheral; }
            set { etheral = value; }
        }

        public int ItemSetId
        {
            get { return itemSetId; }
            set { itemSetId = value; }
        }

        public String Criteria
        {
            get { return criteria; }
            set { criteria = value; }
        }

        public String CriteriaTarget
        {
            get { return criteriaTarget; }
            set { criteriaTarget = value; }
        }

        public Boolean HideEffects
        {
            get { return hideEffects; }
            set { hideEffects = value; }
        }

        public Boolean Enhanceable
        {
            get { return enhanceable; }
            set { enhanceable = value; }
        }

        public Boolean NonUsableOnAnother
        {
            get { return nonUsableOnAnother; }
            set { nonUsableOnAnother = value; }
        }

        public uint AppearanceId
        {
            get { return appearanceId; }
            set { appearanceId = value; }
        }

        public Boolean SecretRecipe
        {
            get { return secretRecipe; }
            set { secretRecipe = value; }
        }

        public List<uint> RecipeIds
        {
            get { return recipeIds; }
            set { recipeIds = value; }
        }

        public Boolean BonusIsSecret
        {
            get { return bonusIsSecret; }
            set { bonusIsSecret = value; }
        }

        public List<EffectInstance> PossibleEffects
        {
            get { return possibleEffects; }
            set { possibleEffects = value; }
        }

        public List<uint> FavoriteSubAreas
        {
            get { return favoriteSubAreas; }
            set { favoriteSubAreas = value; }
        }

        public uint FavoriteSubAreasBonus
        {
            get { return favoriteSubAreasBonus; }
            set { favoriteSubAreasBonus = value; }
        }

        public ItemType Type
        {
            get { return type; }
            set { type = value; }
        }

        public uint Weight
        {
            get { return weight; }
            set { weight = value; }
        }

    }
}