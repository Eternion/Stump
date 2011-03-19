using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[D2OClass("Appearances")]
	public class Appearance : Object
	{
		public const String MODULE = "Appearances";
		public uint id;
		public uint type;
		public String data;
		
	}
}
