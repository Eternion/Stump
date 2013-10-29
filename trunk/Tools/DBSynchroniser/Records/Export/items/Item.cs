 


// Generated on 10/28/2013 14:03:23
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
    [TableName("Items")]
    [D2OClass("Item", "com.ankamagames.dofus.datacenter.items")]
    public class ItemRecord : ID2ORecord
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
        [I18NField]
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
        [I18NField]
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
        [NullString]
        public String Criteria
        {
            get { return criteria; }
            set { criteria = value; }
        }

        [D2OIgnore]
        [NullString]
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
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] RecipeIdsBin
        {
            get { return m_recipeIdsBin; }
            set
            {
                m_recipeIdsBin = value;
                recipeIds = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        public Boolean BonusIsSecret
        {
            get { return bonusIsSecret; }
            set { bonusIsSecret = value; }
        }

        [D2OIgnore]
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
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] PossibleEffectsBin
        {
            get { return m_possibleEffectsBin; }
            set
            {
                m_possibleEffectsBin = value;
                possibleEffects = value == null ? null : value.ToObject<List<EffectInstance>>();
            }
        }

        [D2OIgnore]
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
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] FavoriteSubAreasBin
        {
            get { return m_favoriteSubAreasBin; }
            set
            {
                m_favoriteSubAreasBin = value;
                favoriteSubAreas = value == null ? null : value.ToObject<List<uint>>();
            }
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

        public virtual void AssignFields(object obj)
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
            Weight = castedObj.weight;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (Item)parent : new Item();
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
            obj.weight = Weight;
            return obj;
        
        }
    }
}