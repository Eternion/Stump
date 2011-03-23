using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.servers
{
	
	[D2OClass("ServerPopulations")]
	public class ServerPopulation : Object
	{
		internal const String MODULE = "ServerPopulations";
		public int id;
		public uint nameId;
		public int weight;
		
	}
}
