using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("ServerPopulations")]
	public class ServerPopulation
	{
		private const String MODULE = "ServerPopulations";
		public int id;
		public uint nameId;
		public int weight;
	}
}
