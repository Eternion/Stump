using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[Serializable]
	public class EffectInstanceDice : EffectInstanceInteger
	{
		public uint diceNum;
		public uint diceSide;
	}
}
