using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Interactives")]
	[Serializable]
	public class Interactive
	{
		private const String MODULE = "Interactives";
		public int id;
		public uint nameId;
		public int actionId;
		public Boolean displayTooltip;
	}
}
