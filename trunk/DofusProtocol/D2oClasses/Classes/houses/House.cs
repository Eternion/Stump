using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Houses")]
	public class House
	{
		private const String MODULE = "Houses";
		public int typeId;
		public uint defaultPrice;
		public int nameId;
		public int descriptionId;
		public int gfxId;
	}
}
