using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.servers
{
	
	[D2OClass("ServerGameTypes")]
	public class ServerGameType : Object
	{
		internal const String MODULE = "ServerGameTypes";
		public int id;
		public uint nameId;
		
	}
}
