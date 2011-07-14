using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[Serializable]
	public class EffectInstanceDuration : EffectInstance
	{
		public uint days;
		public uint hours;
		public uint minutes;
	}
}
