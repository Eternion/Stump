using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[Serializable]
	public class EffectInstanceDate : EffectInstance
	{
		public uint year;
		public uint month;
		public uint day;
		public uint hour;
		public uint minute;
	}
}
