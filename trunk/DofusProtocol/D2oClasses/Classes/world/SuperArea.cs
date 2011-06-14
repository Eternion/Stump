using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("SuperAreas")]
	public class SuperArea
	{
		private const String MODULE = "SuperAreas";
		public int id;
		public uint nameId;
		public uint worldmapId;
	}
}
