using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.servers
{
	
	[D2OClass("ServerCommunities")]
	public class ServerCommunity : Object
	{
		internal const String MODULE = "ServerCommunities";
		public int id;
		public uint nameId;
		public List<String> defaultCountries;
		
	}
}
