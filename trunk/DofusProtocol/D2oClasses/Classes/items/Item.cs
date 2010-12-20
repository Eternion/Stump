using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("Items")]
	public class Item : Object
	{
		public const uint CONSUMABLES_CATEGORY = 1;
		internal const String MODULE = "Items";
		internal static Array SUPERTYPE_NOT_EQUIPABLE = new [] { 9, 14, 15, 16, 17, 18, 6, 19, 21, 20, 8, 22 };
		internal static Array FILTER_EQUIPEMENT = new [] { false, true, true, true, true, true, false, true, true, false, true, true, true, true, false, false, false, false, false, false, false, false, true };
		internal static Array FILTER_CONSUMABLES = new [] { false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
		internal static Array FILTER_RESSOURCES = new [] { false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false };
		internal static Array FILTER_QUEST = new [] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false };
		public const uint EQUIPEMENT_CATEGORY = 0;
		public const uint RESSOURCES_CATEGORY = 2;
		public const uint QUEST_CATEGORY = 3;
		public const uint OTHER_CATEGORY = 4;
		public uint weight;
		internal Array _recipes;
		public int id;
		public uint nameId;
		public uint typeId;
		public uint descriptionId;
		public uint iconId;
		public uint level;
		public Boolean cursed;
		public int useAnimationId;
		public Boolean usable;
		public Boolean targetable;
		public uint price;
		public Boolean twoHanded;
		public Boolean etheral;
		public int itemSetId;
		public String criteria;
		public Boolean hideEffects;
		public uint appearanceId;
		public List<uint> recipeIds;
		public List<uint> favoriteSubAreas;
		public Boolean bonusIsSecret;
		public List<EffectInstance> possibleEffects;
		public uint favoriteSubAreasBonus;
		
	}
}
