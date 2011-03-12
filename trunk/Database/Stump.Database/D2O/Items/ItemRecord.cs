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
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses;

namespace Stump.Database.D2O
{
    [Serializable]
    [ActiveRecord("items"), JoinedBase]
    public class ItemRecord : D2OBaseRecord<ItemRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [Property("NameId")]
        public uint NameId
        {
            get;
            set;
        }

        [Property("TypeId")]
        public int TypeId
        {
            get;
            set;
        }

        [Property("DescriptionId")]
        public uint DescriptionId
        {
            get;
            set;
        }

        [Property("IconId")]
        public int IconId
        {
            get;
            set;
        }

        [Property("Level")]
        public int Level
        {
            get;
            set;
        }

        [Property("Weight")]
        public int Weight
        {
            get;
            set;
        }

        [Property("Cursed")]
        public bool Cursed
        {
            get;
            set;
        }

        [Property("UseAnimationId")]
        public int UseAnimationId
        {
            get;
            set;
        }

        [Property("Usable")]
        public bool Usable
        {
            get;
            set;
        }

        [Property("Targetable")]
        public bool Targetable
        {
            get;
            set;
        }

        [Property("Price")]
        public int Price
        {
            get;
            set;
        }

        [Property("TwoHanded")]
        public bool TwoHanded
        {
            get;
            set;
        }

        [Property("Etheral")]
        public bool Etheral
        {
            get;
            set;
        }

        [Property("ItemSetId")]
        public int ItemSetId
        {
            get;
            set;
        }

        [Property("Criteria")]
        public string Criteria
        {
            get;
            set;
        }

        [Property("HideEffects")]
        public bool HideEffects
        {
            get;
            set;
        }

        [Property("AppereanceId")]
        public int AppearanceId
        {
            get;
            set;
        }

        [Property("RecipeIds", ColumnType="Serializable")]
        public List<uint> RecipeIds
        {
            get;
            set;
        }

        [Property("IsSecret")]
        public bool IsSecret
        {
            get;
            set;
        }

        [Property("PossibleEffects", ColumnType = "Serializable")]
        public List<EffectInstance> PossibleEffects
        {
            get;
            set;
        }

        [Property("FavoriteSubAreas", ColumnType = "Serializable")]
        public List<uint> FavoriteSubAreas
        {
            get;
            set;
        }

        [Property("FavoriteSubAreasBonus")]
        public int FavoriteSubAreasBonus
        {
            get;
            set;
        }
    }
}