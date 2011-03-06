// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
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
		internal Array _recipes;
		public int id;
		public uint nameId;
		public int typeId;
		public uint descriptionId;
		public int iconId;
		public int level;
        public int weight;
		public Boolean cursed;
		public int useAnimationId;
		public Boolean usable;
		public Boolean targetable;
		public int price;
		public Boolean twoHanded;
		public Boolean etheral;
		public int itemSetId;
		public String criteria;
		public Boolean hideEffects;
		public int appearanceId;
		public List<uint> recipeIds;
        public Boolean bonusIsSecret;
		public List<EffectInstance> possibleEffects;
		public List<uint> favoriteSubAreas;
		public int favoriteSubAreasBonus;		
	}
}
