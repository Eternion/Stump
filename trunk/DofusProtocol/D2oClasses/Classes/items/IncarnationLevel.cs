using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("IncarnationLevels")]
	[Serializable]
	public class IncarnationLevel
	{
		private const String MODULE = "IncarnationLevels";
		public int id;
		public int incarnationId;
		public int level;
		public uint requiredXp;
	}
}
