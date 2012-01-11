using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[Serializable]
	public class MonsterGrade
	{
		public uint grade;
		public int monsterId;
		public uint level;
		public int paDodge;
		public int pmDodge;
		public int wisdom;
		public int earthResistance;
		public int airResistance;
		public int fireResistance;
		public int waterResistance;
		public int neutralResistance;
		public int gradeXp;
		public int lifePoints;
		public int actionPoints;
		public int movementPoints;
	}
}
