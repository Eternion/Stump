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
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Effects;
using EffectInstanceEx = Stump.DofusProtocol.D2oClasses.EffectInstance;
using ItemSuperType = System.UInt32;
using ItemTemplateEx = Stump.DofusProtocol.D2oClasses.Item;
using ItemTypeEx = Stump.DofusProtocol.D2oClasses.ItemType;

namespace Stump.Server.WorldServer.Items
{
    public class ItemTemplate
    {
        public static Dictionary<ItemSuperType, ItemCategoryEnum> LinkedCategory = new Dictionary<ItemSuperType, ItemCategoryEnum>
            {
                {0, ItemCategoryEnum.Other},
                {1, ItemCategoryEnum.Equipement},
                {2, ItemCategoryEnum.Equipement},
                {3, ItemCategoryEnum.Equipement},
                {4, ItemCategoryEnum.Equipement},
                {5, ItemCategoryEnum.Equipement},
                {6, ItemCategoryEnum.Consumables},
                {7, ItemCategoryEnum.Equipement},
                {8, ItemCategoryEnum.Equipement},
                {9, ItemCategoryEnum.Ressources},
                {10, ItemCategoryEnum.Equipement},
                {11, ItemCategoryEnum.Equipement},
                {12, ItemCategoryEnum.Equipement},
                {13, ItemCategoryEnum.Equipement},
                {14, ItemCategoryEnum.Quest},
                {15, ItemCategoryEnum.Other},
                {16, ItemCategoryEnum.Other},
                {17, ItemCategoryEnum.Other},
                {18, ItemCategoryEnum.Other},
                {19, ItemCategoryEnum.Other},
                {20, ItemCategoryEnum.Other},
                {21, ItemCategoryEnum.Other},
                {22, ItemCategoryEnum.Equipement},
            };

        public static ItemSuperType[] SupertypeNotEquipable = new uint[] {9, 14, 15, 16, 17, 18, 6, 19, 21, 20, 8, 22};

        protected ItemTemplateEx m_basedclass;

        public ItemTemplate(ItemTemplateEx basedItem)
        {
            m_basedclass = basedItem;
        }

        protected ItemTypeEx _type
        {
            get { return ItemManager.GetItemType(m_basedclass.typeId); }
        }

        public int Id
        {
            get { return m_basedclass.id; }
        }

        public string Name
        {
            get;
            set;
        }

        public uint NameId
        {
            get { return m_basedclass.nameId; }
        }

        public ItemTypeEnum Type
        {
            get { return (ItemTypeEnum) m_basedclass.typeId; }
        }

        public ItemCategoryEnum Category
        {
            get
            {
                return LinkedCategory.ContainsKey(_type.superTypeId)
                           ? LinkedCategory[_type.superTypeId]
                           : ItemCategoryEnum.Other;
            }
        }

        public bool Equipable
        {
            get { return !SupertypeNotEquipable.Contains(_type.superTypeId); // To check do this work ?
            }
        }

        public uint Level
        {
            get { return m_basedclass.level; }
        }

        public uint Price
        {
            get { return m_basedclass.price; }
        }

        public uint Weight
        {
            get { return m_basedclass.weight; }
        }

        public bool TwoHanded
        {
            get { return m_basedclass.twoHanded; }
        }

        public bool Usable
        {
            get { return m_basedclass.usable; }
        }

        public bool Cursed
        {
            get { return m_basedclass.cursed; }
        }

        public bool Etheral
        {
            get { return m_basedclass.etheral; }
        }

        public uint AppearanceId
        {
            get { return m_basedclass.appearanceId; }
        }

        public string Criteria
        {
            get { return m_basedclass.criteria; }
        }

        public List<uint> RecipeIds
        {
            get { return m_basedclass.recipeIds; }
        }

        public List<EffectInstanceEx> PossibleEffects
        {
            get { return m_basedclass.possibleEffects; }
        }

        public List<EffectBase> Effects
        {
            get
            {
                if (ItemStored == null)
                    return new List<EffectBase>();

                return ItemStored.Effects.ToList();
            }
        }

        public ItemStored ItemStored
        {
            get;
            set;
        }
    }
}