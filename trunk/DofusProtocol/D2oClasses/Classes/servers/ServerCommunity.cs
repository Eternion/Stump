using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("ServerCommunities")]
	public class ServerCommunity
	{
		private const String MODULE = "ServerCommunities";
		public int id;
		public uint nameId;
		public List<String> defaultCountries;
	}
}
