#region License GNU GPL

// WeaponTemplate.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

#endregion

using System;
using Stump.DofusProtocol.D2oClasses;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace Stump.Server.WorldServer.Database.Items
{
    public class WeaponTemplateRelator
    {
        public static string FetchQuery = "SELECT * FROM items_templates_weapons";
    }

    [TableName("items_templates_weapons")]
    public class WeaponTemplate : ItemTemplate
    {
        public int ApCost
        {
            get;
            set;
        }

        public int MinRange
        {
            get;
            set;
        }

        public int WeaponRange
        {
            get;
            set;
        }

        public Boolean CastInLine
        {
            get;
            set;
        }

        public Boolean CastInDiagonal
        {
            get;
            set;
        }

        public Boolean CastTestLos
        {
            get;
            set;
        }

        public int CriticalHitProbability
        {
            get;
            set;
        }

        public int CriticalHitBonus
        {
            get;
            set;
        }

        public int CriticalFailureProbability
        {
            get;
            set;
        }

        public override void AssignFields(object d2oObject)
        {
            var weapon = (Weapon) d2oObject;
            ApCost = weapon.apCost;
            MinRange = weapon.minRange;
            WeaponRange = weapon.range;
            CastInLine = weapon.castInLine;
            CastInDiagonal = weapon.castInDiagonal;
            CastTestLos = weapon.castTestLos;
            CriticalHitProbability = weapon.criticalHitProbability;
            CriticalHitBonus = weapon.criticalHitBonus;
            CriticalFailureProbability = weapon.criticalFailureProbability;
        }
    }
}