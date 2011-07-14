using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Pack")]
	[Serializable]
	public class Pack
	{
		private const String MODULE = "Pack";
		public int id;
		public String name;
		public Boolean hasSubAreas;
	}
}
