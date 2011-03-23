using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.mounts
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
