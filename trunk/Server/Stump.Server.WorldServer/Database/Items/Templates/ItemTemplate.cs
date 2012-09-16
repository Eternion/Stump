using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Conditions;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database
{
    public class ItemTemplateConfiguration : EntityTypeConfiguration<ItemTemplate>
    {
        public ItemTemplateConfiguration()
        {
            ToTable("items_templates");
            Map(x => x.Requires("Discriminator").HasValue("Item"));

            Ignore(x => x.FavoriteSubAreas);
            Ignore(x => x.RecipeIds);
        }
    }

    [D2OClass("Item", "com.ankamagames.dofus.datacenter.items")]
    public partial class ItemTemplate : IAssignedByD2O, ISaveIntercepter
    {
        public const uint EquipementCategory = 0;
        public const uint ConsumablesCategory = 1;
        public const uint RessourcesCategory = 2;
        public const uint QuestCategory = 3;
        public const uint OtherCategory = 4;
        private ConditionExpression m_criteriaExpression;
        private string m_description;
        private List<EffectBase> m_effects;
        private List<uint> m_favoriteSubAreas;
        private byte[] m_favoriteSubAreasBin;
        private ItemSetTemplate m_itemSet;
        private string m_name;
        private List<EffectInstance> m_possibleEffects;
        private byte[] m_possibleEffectsBin;
        private List<uint> m_recipeIds;
        private byte[] m_recipeIdsBin;
        private ItemTypeRecord m_type;

        public int Id
        {
            get;
            set;
        }

        public uint Weight
        {
            get;
            set;
        }

        public uint RealWeight
        {
            get;
            set;
        }

        public uint NameId
        {
            get;
            set;
        }

        public string Name
        {
            get { return m_name ?? (m_name = TextManager.Instance.GetText(NameId)); }
        }

        public uint TypeId
        {
            get;
            set;
        }

        public ItemTypeRecord Type
        {
            get { return m_type ?? (m_type = ItemManager.Instance.TryGetItemType((int) TypeId)); }
        }

        public uint DescriptionId
        {
            get;
            set;
        }

        public string Descrption
        {
            get { return m_description ?? (m_description = TextManager.Instance.GetText(DescriptionId)); }
        }

        public int IconId
        {
            get;
            set;
        }

        public uint Level
        {
            get;
            set;
        }

        public Boolean Cursed
        {
            get;
            set;
        }

        public int UseAnimationId
        {
            get;
            set;
        }

        public Boolean Usable
        {
            get;
            set;
        }

        public Boolean Targetable
        {
            get;
            set;
        }

        public float Price
        {
            get;
            set;
        }

        public Boolean TwoHanded
        {
            get;
            set;
        }

        public Boolean Etheral
        {
            get;
            set;
        }

        public int ItemSetId
        {
            get;
            set;
        }

        public ItemSetTemplate ItemSet
        {
            get
            {
                return ItemSetId < 0
                           ? null
                           : m_itemSet ?? (m_itemSet = ItemManager.Instance.TryGetItemSetTemplate((uint) ItemSetId));
            }
        }

        public String Criteria
        {
            get;
            set;
        }

        public ConditionExpression CriteriaExpression
        {
            get
            {
                if (string.IsNullOrEmpty(Criteria) || Criteria == "null")
                    return null;

                return m_criteriaExpression ?? (m_criteriaExpression = ConditionExpression.Parse(Criteria));
            }
            set
            {
                m_criteriaExpression = value;
                Criteria = value.ToString();
            }
        }

        public Boolean HideEffects
        {
            get;
            set;
        }

        public uint AppearanceId
        {
            get;
            set;
        }

        public byte[] RecipeIdsBin
        {
            get { return m_recipeIdsBin; }
            set
            {
                m_recipeIdsBin = value;
                m_recipeIds = value.ToObject<List<uint>>();
            }
        }

        public List<uint> RecipeIds
        {
            get { return m_recipeIds; }
            set
            {
                m_recipeIds = value;
                m_recipeIdsBin = value.ToBinary();
            }
        }

        public byte[] FavoriteSubAreasBin
        {
            get { return m_favoriteSubAreasBin; }
            set
            {
                m_favoriteSubAreasBin = value;
                m_favoriteSubAreas = value.ToObject<List<uint>>();
            }
        }

        public List<uint> FavoriteSubAreas
        {
            get { return m_favoriteSubAreas; }
            set
            {
                m_favoriteSubAreas = value;
                m_favoriteSubAreasBin = value.ToBinary();
            }
        }

        public Boolean BonusIsSecret
        {
            get;
            set;
        }

        public byte[] PossibleEffectsBin
        {
            get { return m_possibleEffectsBin; }
            set
            {
                m_possibleEffectsBin = value;
                m_possibleEffects = value.ToObject<List<EffectInstance>>();
            }
        }

        public List<EffectInstance> PossibleEffects
        {
            get { return m_possibleEffects; }
            set
            {
                m_possibleEffects = value;
                m_possibleEffectsBin = value.ToBinary();
            }
        }

        public List<EffectBase> Effects
        {
            get
            {
                if (m_effects != null)
                    return m_effects;

                if (PossibleEffects == null)
                    return m_effects = new List<EffectBase>();

                return
                    m_effects =
                    new List<EffectBase>(PossibleEffects.Select(EffectManager.Instance.ConvertExportedEffect));
            }
            set { m_effects = value; }
        }

        public uint FavoriteSubAreasBonus
        {
            get;
            set;
        }

        public bool IsLinkedToOwner
        {
            get;
            set;
        }

        #region IAssignedByD2O Members

        public void AssignFields(object d2oObject)
        {
            var template = (Item) d2oObject;
            Id = template.id;
            Weight = template.weight;
            RealWeight = template.realWeight;
            NameId = template.nameId;
            TypeId = template.typeId;
            DescriptionId = template.descriptionId;
            IconId = template.iconId;
            Level = template.level;
            Cursed = template.cursed;
            UseAnimationId = template.useAnimationId;
            Usable = template.usable;
            Targetable = template.targetable;
            Price = template.price;
            TwoHanded = template.twoHanded;
            Etheral = template.etheral;
            ItemSetId = template.itemSetId;
            Criteria = template.criteria;
            HideEffects = template.hideEffects;
            AppearanceId = template.appearanceId;
            RecipeIds = template.recipeIds;
            FavoriteSubAreas = template.favoriteSubAreas;
            BonusIsSecret = template.bonusIsSecret;
            PossibleEffects = template.possibleEffects;
            FavoriteSubAreas = template.favoriteSubAreas;
        }

        #endregion

        #region ISaveIntercepter Members

        public void BeforeSave(ObjectStateEntry currentEntry)
        {
            PossibleEffects = m_effects == null ? null : m_effects.Select(entry => entry.GetEffectInstance()).ToList();
            m_possibleEffectsBin = m_possibleEffects.ToBinary();
            m_favoriteSubAreasBin = m_favoriteSubAreas.ToBinary();
            m_recipeIdsBin = m_recipeIds.ToBinary();
        }

        #endregion

        public bool IsWeapon()
        {
            return this is WeaponTemplate;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Id);
        }
    }
}