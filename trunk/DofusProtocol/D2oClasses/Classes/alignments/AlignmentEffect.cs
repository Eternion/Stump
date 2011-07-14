using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("AlignmentEffect")]
	[Serializable]
	public class AlignmentEffect
	{
		private const String MODULE = "AlignmentEffect";
		public int id;
		public uint characteristicId;
		public uint descriptionId;
	}
}
