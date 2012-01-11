using System;
using System.Linq;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Worlds.Effects;
using Stump.Server.WorldServer.Worlds.Effects.Instances;

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

        [D2OField("criteria")]
        [Property("Criteria")]
        public String Criteria
        {
            get;
            set;
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
                return m_effects ?? ( m_effects = new List<EffectBase>(PossibleEffects.Select(EffectManager.Instance.ConvertExportedEffect)) );
            }
            set { m_effects = value; }
        }

        [D2OField("favoriteSubAreasBonus")]
        [Property("FavoriteSubAreasBonus")]
        public uint FavoriteSubAreasBonus
        {
            get;
            set;
        }

    }
}