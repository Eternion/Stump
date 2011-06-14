using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("SpellTypes")]
	public class SpellType
	{
		private const String MODULE = "SpellTypes";
		public int id;
		public uint longNameId;
		public uint shortNameId;
	}
}
