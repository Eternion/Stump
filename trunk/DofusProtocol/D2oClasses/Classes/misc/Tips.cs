using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Tips")]
	[Serializable]
	public class Tips
	{
		private const String MODULE = "Tips";
		public int id;
		public uint descId;
	}
}
