using System;

namespace Stump.DofusProtocol.D2oClasses
{
	public class EffectInstance
	{
		public uint effectId;
		public int targetId;
		public int duration;
		public int random;
		public int modificator;
		public Boolean trigger;
		public Boolean hidden;
	}
}
