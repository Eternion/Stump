using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.npcs
{
	
	[D2OClass("TaxCollectorFirstnames")]
	public class TaxCollectorFirstname : Object
	{
		internal const String MODULE = "TaxCollectorFirstnames";
		public int id;
		public uint firstnameId;
		
	}
}
