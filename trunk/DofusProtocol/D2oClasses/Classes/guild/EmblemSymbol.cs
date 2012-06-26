using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("EmblemSymbols")]
	[Serializable]
	public class EmblemSymbol
	{
		private const String MODULE = "EmblemSymbols";
		public int id;
		public int iconId;
		public int skinId;
		public int order;
		public int categoryId;
	}
}
