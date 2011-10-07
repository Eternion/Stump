using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Incarnation")]
	[Serializable]
	public class Incarnation
	{
		private const String MODULE = "Incarnation";
		public uint id;
		public String lookMale;
		public String lookFemale;
	}
}
