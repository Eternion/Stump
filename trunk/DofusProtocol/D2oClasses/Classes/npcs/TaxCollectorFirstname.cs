using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("TaxCollectorFirstnames")]
	[Serializable]
	public class TaxCollectorFirstname
	{
		private const String MODULE = "TaxCollectorFirstnames";
		public int id;
		public uint firstnameId;
	}
}
