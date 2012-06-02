using System;
using System.Linq;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Conditions;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database.Items.Templates
{
    [Serializable]
    [ActiveRecord("items_templates", DiscriminatorColumn = "RecognizerType", DiscriminatorType = "String", DiscriminatorValue = "Item")]
    [D2OClass("Item", "com.ankamagames.dofus.datacenter.items")]
    public class ItemTemplate : WorldBaseRecord<ItemTemplate>
    {
        public const uint EquipementCategory = 0;
        public const uint ConsumablesCategory = 1;
        public const uint RessourcesCategory = 2;
        public const uint QuestCategory = 3;
        public const uint OtherCategory = 4;

        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [D2OField("weight")]
        [Property("Weight")]
        public uint Weight
        {
            get;
            set;
        }

        [D2OField("realWeight")]
        [Property("RealWeight")]
        public uint RealWeight
        {
            get;
            set;
        }

        [D2OField("nameId")]
        [Property("NameId")]
        public uint NameId
        {
            get;
            set;
        }

        private string m_name;

        public string Name
        {
            get
            {
                return m_name ?? ( m_name = TextManager.Instance.GetText(NameId) );
            }
        }

        [D2OField("typeId")]
        [Property("TypeId")]
        public uint TypeId
        {
            get;
            set;
        }

        private ItemTypeRecord m_type;

        public ItemTypeRecord Type
        {
            get { return m_type ?? (m_type = ItemManager.Instance.TryGetItemType((int) TypeId)); }
        }

        [D2OField("descriptionId")]
        [Property("DescriptionId")]
        public uint DescriptionId
        {
            get;
            set;
        }

        private string m_description;

        public string Descrption
        {
            get
            {
                return m_description ?? ( m_description = TextManager.Instance.GetText(DescriptionId) );
            }
        }

        [D2OField("iconId")]
        [Property("IconId")]
        public uint IconId
        {
            get;
            set;
        }

        [D2OField("level")]
        [Property("Level")]
        public uint Level
        {
            get;
            set;
        }

        [D2OField("cursed")]
        [Property("Cursed")]
        public Boolean Cursed
        {
            get;
            set;
        }

        [D2OField("useAnimationId")]
        [Property("UseAnimationId")]
        public int UseAnimationId
        {
            get;
            set;
        }

        [D2OField("usable")]
        [Property("Usable")]
        public Boolean Usable
        {
            get;
            set;
        }

        [D2OField("targetable")]
        [Property("Targetable")]
        public Boolean Targetable
        {
            get;
            set;
        }

        [D2OField("price")]
        [Property("Price")]
        public float Price
        {
            get;
            set;
        }

        [D2OField("twoHanded")]
        [Property("TwoHanded")]
        public Boolean TwoHanded
        {
            get;
            set;
        }

        [D2OField("etheral")]
        [Property("Etheral")]
        public Boolean Etheral
        {
            get;
            set;
        }

        [D2OField("itemSetId")]
        [Property("ItemSetId")]
        public int ItemSetId
        {
            get;
            set;
        }

        private ItemSetTemplate m_itemSet;

        public ItemSetTemplate ItemSet
        {
            get
            {
                return ItemSetId < 0 ? null : m_itemSet ?? ( m_itemSet = ItemManager.Instance.TryGetItemSetTemplate((uint)ItemSetId) );
            }
        }

        [D2OField("criteria")]
        [Property("Criteria")]
        public String Criteria
        {
            get;
            set;
        }

        private ConditionExpression m_criteriaExpression;

        public ConditionExpression CriteriaExpression
        {
            get
            {
                if (string.IsNullOrEmpty(Criteria) || Criteria == "null")
                    return null;

                return m_criteriaExpression ?? ( m_criteriaExpression = ConditionExpression.Parse(Criteria) );
            }
            set
            {
                m_criteriaExpression = value;
                Criteria = value.ToString();
            }
        }

        [D2OField("hideEffects")]
        [Property("HideEffects")]
        public Boolean HideEffects
        {
            get;
            set;
        }

        [D2OField("appearanceId")]
        [Property("AppearanceId")]
        public uint AppearanceId
        {
            get;
            set;
        }

        /*[D2OField("recipeIds")]
        [Property("RecipeIds", ColumnType = "Serializable")]
        public List<uint> RecipeIds
        {
            get;
            set;
        }

        [D2OField("favoriteSubAreas")]
        [Property("FavoriteSubAreas", ColumnType = "Serializable")]
        public List<uint> FavoriteSubAreas
        {
            get;
            set;
        }*/

        [D2OField("bonusIsSecret")]
        [Property("BonusIsSecret")]
        public Boolean BonusIsSecret
        {
            get;
            set;
        }

        [D2OField("possibleEffects")]
        [Property("PossibleEffects", ColumnType = "Serializable")]
        public List<EffectInstance> PossibleEffects
        {
            get;
            set;
        }

        private List<EffectBase> m_effects;

        public List<EffectBase> Effects
        {
            get
            {
                return m_effects ??
                       (m_effects =
                        new List<EffectBase>(PossibleEffects.Select(EffectManager.Instance.ConvertExportedEffect)));
            }
            set
            {
                m_effects = value;
            }
        }

        [D2OField("favoriteSubAreasBonus")]
        [Property("FavoriteSubAreasBonus")]
        public uint FavoriteSubAreasBonus
        {
            get;
            set;
        }

        [Property]
        public bool IsLinkedToOwner
        {
            get;
            set;
        }

        protected override bool OnFlushDirty(object id, System.Collections.IDictionary previousState, System.Collections.IDictionary currentState, NHibernate.Type.IType[] types)
        {
            PossibleEffects = (List<EffectInstance>)(currentState["PossibleEffects"] = m_effects == null
                      ? null
                      : m_effects.Select(entry => entry.GetEffectInstance()).ToList());

            return base.OnFlushDirty(id, previousState, currentState, types);
        }

        protected override bool BeforeSave(System.Collections.IDictionary state)
        {
            PossibleEffects = (List<EffectInstance>)( state["PossibleEffects"] = m_effects == null
             ? null
             : m_effects.Select(entry => entry.GetEffectInstance()).ToList() );

            return base.BeforeSave(state);
        }

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