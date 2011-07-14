using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("AlignmentBalance")]
	[Serializable]
	public class AlignmentBalance
	{
		private const String MODULE = "AlignmentBalance";
		public int id;
		public int startValue;
		public int endValue;
		public uint nameId;
		public uint descriptionId;
	}
}
