using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[D2OClass("SpellLevels")]
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
