using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[D2OClass("Mounts")]
	public class Mount : Object
	{
		public uint id;
		public uint nameId;
		public String look;
		public String MODULE = "Mounts";
		
	}
}
