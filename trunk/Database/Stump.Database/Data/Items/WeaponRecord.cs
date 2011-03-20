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
using Castle.ActiveRecord;

namespace Stump.Database.Data.Items
{
    [Serializable]
    [ActiveRecord("weapons")]
    //[AttributeAssociatedFile("Weapon")]
    public sealed class WeaponRecord :ItemRecord
    {
        [JoinedKey("ItemGuid")]
        private long ItemGuid { get; set; }

        [Property("ApCost")]
        public int ApCost
        {
            get;
            set;
        }

        [Property("MinRange")]
        public int MinRange
        {
            get;
            set;
        }

        [Property("WeaponRange")]
        public int WeaponRange
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
        public int CriticalHitProbability
        {
            get;
            set;
        }

        [Property("CriticalHitBonus")]
        public int CriticalHitBonus
        {
            get;
            set;
        }

        [Property("CriticalFailureProbability")]
        public int CriticalFailureProbability
        {
            get;
            set;
        }

        [Property("Hidden")]
        public bool Hidden
        {
            get;
            set;
        }
    }
}