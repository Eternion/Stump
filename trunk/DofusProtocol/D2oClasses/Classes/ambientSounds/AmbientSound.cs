using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("AmbientSounds")]
	public class AmbientSound
	{
		private const String MODULE = "AmbientSounds";
		public int id;
		public uint volume;
		public int criterionId;
		public uint silenceMin;
		public uint silenceMax;
		public int channel;
	}
}
