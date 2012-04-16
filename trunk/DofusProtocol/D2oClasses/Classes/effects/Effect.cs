using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Effects")]
	[Serializable]
	public class Effect
	{
		private const String MODULE = "Effects";
		public int id;
		public uint descriptionId;
		public int iconId;
		public int characteristic;
		public uint category;
		public String @operator;
		public Boolean showInTooltip;
		public Boolean useDice;
		public Boolean forceMinMax;
		public Boolean boost;
		public Boolean active;
		public Boolean showInSet;
		public int bonusType;
		public Boolean useInFight;
	}
}
