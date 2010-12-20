using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("TaxCollectorNames")]
	public class TaxCollectorName : Object
	{
		internal const String MODULE = "TaxCollectorNames";
		public int id;
		public uint nameId;
		
	}
}
