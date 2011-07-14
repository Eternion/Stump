using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("RankNames")]
	[Serializable]
	public class RankName
	{
		private const String MODULE = "RankNames";
		public int id;
		public uint nameId;
		public int order;
	}
}
