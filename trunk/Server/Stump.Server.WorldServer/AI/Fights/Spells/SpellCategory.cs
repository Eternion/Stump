#region License GNU GPL
// SpellCategory.cs
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

using System;

namespace Stump.Server.WorldServer.AI.Fights.Spells
{
    [Flags]
    public enum SpellCategory
    {
        Healing = 0x0001,
        Teleport = 0x0002,
        Summoning = 0x0004,
        Buff = 0x0008,
        DamagesWater = 0x0010,
        DamagesEarth = 0x0020,
        DamagesAir = 0x0040,
        DamagesFire = 0x0080,
        DamagesNeutral = 0x0100,
        Curse = 0x0200,
        Damages = DamagesNeutral | DamagesFire | DamagesAir | DamagesEarth | DamagesWater,
        None = 0,
        All = 0x01FF,
    }
}