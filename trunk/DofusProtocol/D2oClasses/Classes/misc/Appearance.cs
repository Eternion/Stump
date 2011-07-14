using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Appearances")]
	[Serializable]
	public class Appearance
	{
		public const String MODULE = "Appearances";
		public uint id;
		public uint type;
		public String data;
	}
}
