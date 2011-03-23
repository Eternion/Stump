using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.misc
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
