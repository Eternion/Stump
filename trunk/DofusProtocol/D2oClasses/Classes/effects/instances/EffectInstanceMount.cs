using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[Serializable]
	public class EffectInstanceMount : EffectInstance
	{
		public float date;
		public uint modelId;
		public uint mountId;
	}
}
