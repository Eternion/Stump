using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("ServerCommunities")]
	public class ServerCommunity : Object
	{
		internal const String MODULE = "ServerCommunities";
		public int id;
		public uint nameId;
		public List<String> defaultCountries;
		
	}
}
