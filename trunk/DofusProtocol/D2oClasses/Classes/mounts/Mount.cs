using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Mounts")]
	[Serializable]
	public class Mount
	{
		public uint id;
		public uint nameId;
		public String look;
		private String MODULE = "Mounts";
	}
}
