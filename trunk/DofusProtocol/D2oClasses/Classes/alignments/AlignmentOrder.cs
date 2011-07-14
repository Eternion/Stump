using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("AlignmentOrder")]
	[Serializable]
	public class AlignmentOrder
	{
		private const String MODULE = "AlignmentOrder";
		public int id;
		public uint nameId;
		public uint sideId;
	}
}
