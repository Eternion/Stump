using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Months")]
	[Serializable]
	public class Month
	{
		private const String MODULE = "Months";
		public int id;
		public uint nameId;
	}
}
