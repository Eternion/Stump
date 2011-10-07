using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("OptionalFeatures")]
	[Serializable]
	public class OptionalFeature
	{
		public const String MODULE = "OptionalFeatures";
		public int id;
		public String keyword;
	}
}
