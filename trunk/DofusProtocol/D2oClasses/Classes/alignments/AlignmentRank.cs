using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("AlignmentRank")]
	[Serializable]
	public class AlignmentRank
	{
		private const String MODULE = "AlignmentRank";
		public int id;
		public uint orderId;
		public uint nameId;
		public uint descriptionId;
		public int minimumAlignment;
		public int objectsStolen;
		public List<int> gifts;
	}
}
