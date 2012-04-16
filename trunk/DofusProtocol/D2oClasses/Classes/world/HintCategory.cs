using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("HintCategory")]
	[Serializable]
	public class HintCategory
	{
		private const String MODULE = "HintCategory";
		public int id;
		public uint nameId;
	}
}
