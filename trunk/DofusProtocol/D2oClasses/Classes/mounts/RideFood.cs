using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.mounts
{
	
	[D2OClass("RideFood")]
	public class RideFood : Object
	{
		public uint gid;
		public uint typeId;
		public String MODULE = "RideFood";
		
	}
}
