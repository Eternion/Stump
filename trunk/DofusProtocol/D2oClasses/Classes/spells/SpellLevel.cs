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
	
	[AttributeAssociatedFile("SpellLevels")]
	public class SpellLevel : Object
	{
		internal const String MODULE = "SpellLevels";
		public uint id;
		public uint spellId;
		public uint spellBreed;
		public uint apCost;
		public uint minRange;
		public uint range;
		public Boolean castInLine;
		public Boolean castInDiagonal;
		public Boolean castTestLos;
		public uint criticalHitProbability;
		public uint criticalFailureProbability;
		public Boolean needFreeCell;
		public Boolean needFreeTrapCell;
		public Boolean rangeCanBeBoosted;
		public uint maxCastPerTurn;
		public uint maxCastPerTarget;
		public uint minCastInterval;
		public uint minPlayerLevel;
		public Boolean criticalFailureEndsTurn;
		public List<int> statesRequired;
		public List<int> statesForbidden;
		public List<EffectInstanceDice> effects;
		public List<EffectInstanceDice> criticalEffect;
		public Boolean hideEffects;
		
	}
}
