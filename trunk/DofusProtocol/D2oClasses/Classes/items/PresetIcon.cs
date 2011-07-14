using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("PresetIcons")]
	[Serializable]
	public class PresetIcon
	{
		private const String MODULE = "PresetIcons";
		public int id;
		public int order;
	}
}
