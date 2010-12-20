using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("ServerPopulations")]
	public class ServerPopulation : Object
	{
		internal const String MODULE = "ServerPopulations";
		public int id;
		public uint nameId;
		public int weight;
		
	}
}
