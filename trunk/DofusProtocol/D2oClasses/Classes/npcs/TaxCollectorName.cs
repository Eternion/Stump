using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.npcs
{
	
	[D2OClass("TaxCollectorNames")]
	public class TaxCollectorName : Object
	{
		internal const String MODULE = "TaxCollectorNames";
		public int id;
		public uint nameId;
		
	}
}
