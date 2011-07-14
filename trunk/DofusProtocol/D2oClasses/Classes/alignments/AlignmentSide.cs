using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("AlignmentSides")]
	[Serializable]
	public class AlignmentSide
	{
		private const String MODULE = "AlignmentSides";
		public int id;
		public uint nameId;
		public Boolean canConquest;
	}
}
