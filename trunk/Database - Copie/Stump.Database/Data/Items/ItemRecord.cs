using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Data.Effects;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Items
{
    [Serializable]
    [ActiveRecord("item")]
    [AttributeAssociatedFile("Items")]
    [D2OClass("Item", "com.ankamagames.dofus.datacenter.items")]
    public sealed class ItemRecord : DataBaseRecord<ItemRecord>
    {

       [D2OField("CONSUMABLES_CATEGORY")]
       [Property("CONSUMABLESCATEGORY")]
       public uint CONSUMABLESCATEGORY
       {
           get;
           set;
       }

       [D2OField("EQUIPEMENT_CATEGORY")]
       [Property("EQUIPEMENTCATEGORY")]
       public uint EQUIPEMENTCATEGORY
       {
           get;
           set;
       }

       [D2OField("RESSOURCES_CATEGORY")]
       [Property("RESSOURCESCATEGORY")]
       public uint RESSOURCESCATEGORY
       {
           get;
           set;
       }

       [D2OField("QUEST_CATEGORY")]
       [Property("QUESTCATEGORY")]
       public uint QUESTCATEGORY
       {
           get;
           set;
       }

       [D2OField("OTHER_CATEGORY")]
       [Property("OTHERCATEGORY")]
       public uint OTHERCATEGORY
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

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
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
       public uint Price
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

       [D2OField("recipeIds")]
       [Property("RecipeIds", ColumnType="Serializable")]
       public List<uint> RecipeIds
       {
           get;
           set;
       }

       [D2OField("favoriteSubAreas")]
       [Property("FavoriteSubAreas", ColumnType="Serializable")]
       public List<uint> FavoriteSubAreas
       {
           get;
           set;
       }

       [D2OField("bonusIsSecret")]
       [Property("BonusIsSecret")]
       public Boolean BonusIsSecret
       {
           get;
           set;
       }

       [D2OField("possibleEffects")]
       [Property("PossibleEffects", ColumnType="Serializable")]
       public List<EffectInstance> PossibleEffects
       {
           get;
           set;
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