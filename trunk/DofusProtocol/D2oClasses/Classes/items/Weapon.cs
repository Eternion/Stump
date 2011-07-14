using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[Serializable]
	public class Weapon : Item
	{
		public int apCost;
		public int minRange;
		public int range;
		public Boolean castInLine;
		public Boolean castInDiagonal;
		public Boolean castTestLos;
		public int criticalHitProbability;
		public int criticalHitBonus;
		public int criticalFailureProbability;
	}
}
