using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("ServerGameTypes")]
	public class ServerGameType : Object
	{
		internal const String MODULE = "ServerGameTypes";
		public int id;
		public uint nameId;
		
	}
}
