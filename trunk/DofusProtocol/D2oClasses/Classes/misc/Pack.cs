using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("Pack")]
	public class Pack : Object
	{
		internal const String MODULE = "Pack";
		public int id;
		public String name;
		public Boolean hasSubAreas;
		
	}
}
