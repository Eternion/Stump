using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Hints")]
	[Serializable]
	public class Hint
	{
		private const String MODULE = "Hints";
		public int id;
		public uint categoryId;
		public uint gfx;
		public uint nameId;
		public uint mapId;
		public uint realMapId;
	}
}
