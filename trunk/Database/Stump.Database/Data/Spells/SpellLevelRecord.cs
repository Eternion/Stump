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
using Stump.Database.Data.Effects.instances;
using Stump.Database.Types;

namespace Stump.Database.Data.Spells
{
    [Serializable]
    [ActiveRecord("spell_levels")]
    [AttributeAssociatedFile("SpellLevels")]
    public sealed class SpellLevelRecord : DataBaseRecord<SpellLevelRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("SpellId")]
        public uint SpellId
        {
            get;
            set;
        }

        [Property("SpellBreed")]
        public uint SpellBreed
        {
            get;
            set;
        }

        [Property("ApCost")]
        public uint ApCost
        {
            get;
            set;
        }

        [Property("MinRange")]
        public uint MinRange
        {
            get;
            set;
        }

        [Property("CastInLine")]
        public bool CastInLine
        {
            get;
            set;
        }

        [Property("CastInDiagonal")]
        public bool CastInDiagonal
        {
            get;
            set;
        }

        [Property("CastTestLos")]
        public bool CastTestLos
        {
            get;
            set;
        }

        [Property("CriticalHitProbability")]
        public uint CriticalHitProbability
        {
            get;
            set;
        }

        [Property("CriticalFailureProbability")]
        public uint CriticalFailureProbability
        {
            get;
            set;
        }

        [Property("NeedFreeCell")]
        public bool NeedFreeCell
        {
            get;
            set;
        }

        [Property("NeedFreeTrapCell")]
        public bool NeedFreeTrapCell
        {
            get;
            set;
        }

        [Property("RangeCanBeBoosted")]
        public bool RangeCanBeBoosted
        {
            get;
            set;
        }

        [Property("MaxCastPerTurn")]
        public uint MaxCastPerTurn
        {
            get;
            set;
        }

        [Property("MaxCastPerTarget")]
        public uint MaxCastPerTarget
        {
            get;
            set;
        }

        [Property("MinCastInterval")]
        public uint MinCastInterval
        {
            get;
            set;
        }

        [Property("MinPlayerLevel")]
        public uint MinPlayerLevel
        {
            get;
            set;
        }

        [Property("CriticalFailureEndsTurn")]
        public bool CriticalFailureEndsTurn
        {
            get;
            set;
        }

        [Property("StatesRequired", ColumnType = "Serializable")]
        public List<int> StatesRequired
        {
            get;
            set;
        }

        [Property("StatesForbidden", ColumnType = "Serializable")]
        public List<int> StatesForbidden
        {
            get;
            set;
        }

        [Property("Effects", ColumnType = "Serializable")]
        public List<EffectInstanceDice> Effects
        {
            get;
            set;
        }

        [Property("CriticalEffect", ColumnType = "Serializable")]
        public List<EffectInstanceDice> CriticalEffect
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
    }
}