using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("EmblemBackgrounds")]
	[Serializable]
	public class EmblemBackground
	{
		private const String MODULE = "EmblemBackgrounds";
		public int id;
		public int order;
	}
}
