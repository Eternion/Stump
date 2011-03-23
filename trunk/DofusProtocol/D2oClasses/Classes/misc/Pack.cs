using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.misc
{
	
	[D2OClass("Pack")]
	public class Pack : Object
	{
		internal const String MODULE = "Pack";
		public int id;
		public String name;
		public Boolean hasSubAreas;
		
	}
}
