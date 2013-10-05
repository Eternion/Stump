 


// Generated on 10/06/2013 01:10:58
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("Items")]
    public class ItemRecord : ID2ORecord
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

        [PrimaryKey("Id", false)]
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

        [Ignore]
        public List<uint> RecipeIds
        {
            get { return recipeIds; }
            set
            {
                recipeIds = value;
                m_recipeIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_recipeIdsBin;
        public byte[] RecipeIdsBin
        {
            get { return m_recipeIdsBin; }
            set
            {
                m_recipeIdsBin = value;
                recipeIds = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public Boolean BonusIsSecret
        {
            get { return bonusIsSecret; }
            set { bonusIsSecret = value; }
        }

        [Ignore]
        public List<EffectInstance> PossibleEffects
        {
            get { return possibleEffects; }
            set
            {
                possibleEffects = value;
                m_possibleEffectsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_possibleEffectsBin;
        public byte[] PossibleEffectsBin
        {
            get { return m_possibleEffectsBin; }
            set
            {
                m_possibleEffectsBin = value;
                possibleEffects = value == null ? null : value.ToObject<List<EffectInstance>>();
            }
        }

        [Ignore]
        public List<uint> FavoriteSubAreas
        {
            get { return favoriteSubAreas; }
            set
            {
                favoriteSubAreas = value;
                m_favoriteSubAreasBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_favoriteSubAreasBin;
        public byte[] FavoriteSubAreasBin
        {
            get { return m_favoriteSubAreasBin; }
            set
            {
                m_favoriteSubAreasBin = value;
                favoriteSubAreas = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public uint FavoriteSubAreasBonus
        {
            get { return favoriteSubAreasBonus; }
            set { favoriteSubAreasBonus = value; }
        }

        [Ignore]
        public ItemType Type
        {
            get { return type; }
            set
            {
                type = value;
                m_typeBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_typeBin;
        public byte[] TypeBin
        {
            get { return m_typeBin; }
            set
            {
                m_typeBin = value;
                type = value == null ? null : value.ToObject<ItemType>();
            }
        }

        public uint Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (Item)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            TypeId = castedObj.typeId;
            DescriptionId = castedObj.descriptionId;
            IconId = castedObj.iconId;
            Level = castedObj.level;
            RealWeight = castedObj.realWeight;
            Cursed = castedObj.cursed;
            UseAnimationId = castedObj.useAnimationId;
            Usable = castedObj.usable;
            Targetable = castedObj.targetable;
            Price = castedObj.price;
            TwoHanded = castedObj.twoHanded;
            Etheral = castedObj.etheral;
            ItemSetId = castedObj.itemSetId;
            Criteria = castedObj.criteria;
            CriteriaTarget = castedObj.criteriaTarget;
            HideEffects = castedObj.hideEffects;
            Enhanceable = castedObj.enhanceable;
            NonUsableOnAnother = castedObj.nonUsableOnAnother;
            AppearanceId = castedObj.appearanceId;
            SecretRecipe = castedObj.secretRecipe;
            RecipeIds = castedObj.recipeIds;
            BonusIsSecret = castedObj.bonusIsSecret;
            PossibleEffects = castedObj.possibleEffects;
            FavoriteSubAreas = castedObj.favoriteSubAreas;
            FavoriteSubAreasBonus = castedObj.favoriteSubAreasBonus;
            Type = castedObj.type;
            Weight = castedObj.weight;
        }
        
        public object CreateObject()
        {
            var obj = new Item();
            
            obj.id = Id;
            obj.nameId = NameId;
            obj.typeId = TypeId;
            obj.descriptionId = DescriptionId;
            obj.iconId = IconId;
            obj.level = Level;
            obj.realWeight = RealWeight;
            obj.cursed = Cursed;
            obj.useAnimationId = UseAnimationId;
            obj.usable = Usable;
            obj.targetable = Targetable;
            obj.price = Price;
            obj.twoHanded = TwoHanded;
            obj.etheral = Etheral;
            obj.itemSetId = ItemSetId;
            obj.criteria = Criteria;
            obj.criteriaTarget = CriteriaTarget;
            obj.hideEffects = HideEffects;
            obj.enhanceable = Enhanceable;
            obj.nonUsableOnAnother = NonUsableOnAnother;
            obj.appearanceId = AppearanceId;
            obj.secretRecipe = SecretRecipe;
            obj.recipeIds = RecipeIds;
            obj.bonusIsSecret = BonusIsSecret;
            obj.possibleEffects = PossibleEffects;
            obj.favoriteSubAreas = FavoriteSubAreas;
            obj.favoriteSubAreasBonus = FavoriteSubAreasBonus;
            obj.type = Type;
            obj.weight = Weight;
            return obj;
        
        }
    }
}