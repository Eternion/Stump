using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("EmblemSymbolCategories")]
	[Serializable]
	public class EmblemSymbolCategory
	{
		private const String MODULE = "EmblemSymbolCategories";
		public int id;
		public uint nameId;
	}
}
