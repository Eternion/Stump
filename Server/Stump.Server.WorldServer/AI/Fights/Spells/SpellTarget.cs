#region License GNU GPL
// SpellImpact.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
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

using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.AI.Fights.Spells
{
    public class SpellTarget
    {
        public double MinFire, MaxFire,
                 MinWater, MaxWater,
                 MinEarth, MaxEarth,
                 MinAir, MaxAir,
                 MinNeutral, MaxNeutral,
                 MinHeal, MaxHeal;

        public double Fire
        {
            get
            {
                return ( MinFire + MaxFire ) / 2;
            }
        }
        public double Air
        {
            get
            {
                return ( MinAir + MaxAir ) / 2;
            }
        }
        public double Earth
        {
            get
            {
                return ( MinEarth + MaxEarth ) / 2;
            }
        }
        public double Water
        {
            get
            {
                return ( MinWater + MaxWater ) / 2;
            }
        }
        public double Neutral
        {
            get
            {
                return ( MinEarth + MaxEarth ) / 2;
            }
        }
        public double Heal
        {
            get
            {
                return ( MinHeal + MaxHeal ) / 2;
            }
        }
        public double Curse
        {
            get;
            set;
        }
        public double Boost
        {
            get;
            set;
        }

        //public string Comment { get; set; }
        // Min total damage            
        public double MinDamage
        {
            get
            {
                return MinFire + MinAir + MinEarth + MinWater + MinNeutral + MaxHeal + Curse + Boost;
            }
        }

        // Max total damage            
        public double MaxDamage
        {
            get
            {
                return MaxFire + MaxAir + MaxEarth + MaxWater + MaxNeutral + MinHeal + Curse + Boost;
            }
        }

        /// <summary>
        /// Return positive values for bad effects (curses and spellImpact) and négative values for good effects (heals and boosts)
        /// </summary>
        public double Damage
        {
            get
            {
                return ( MinDamage + MaxDamage ) / 2;
            }
        }

        /// <summary>
        /// Can be null
        /// </summary>
        public FightActor Target
        {
            get;
            set;
        }

        public Cell CastCell
        {
            get;
            set;
        }

        public void Add(SpellTarget dmg)
        {
            MinFire += dmg.MinFire;
            MaxFire += dmg.MaxFire;
            MinWater += dmg.MinWater;
            MaxWater += dmg.MaxWater;
            MinEarth += dmg.MinEarth;
            MaxEarth += dmg.MaxEarth;
            MinAir += dmg.MinAir;
            MaxAir += dmg.MaxAir;
            MinNeutral += dmg.MinNeutral;
            MaxNeutral += dmg.MaxNeutral;
            MinHeal += dmg.MinHeal;
            MaxHeal += dmg.MaxHeal;
            Curse += dmg.Curse;
            Boost += dmg.Boost;
        }

        public void Multiply(double ratio)
        {
            MinFire *= ratio;
            MaxFire *= ratio;
            MinWater *= ratio;
            MaxWater *= ratio;
            MinEarth *= ratio;
            MaxEarth *= ratio;
            MinAir *= ratio;
            MaxAir *= ratio;
            MinNeutral *= ratio;
            MaxNeutral *= ratio;
            MinHeal *= ratio;
            MaxHeal *= ratio;
            Curse *= ratio;
            Boost *= ratio;
        }

    }
}