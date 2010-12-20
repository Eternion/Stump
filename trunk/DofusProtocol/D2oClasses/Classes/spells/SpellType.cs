using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("SpellTypes")]
	public class SpellType : Object
	{
		internal const String MODULE = "SpellTypes";
		public int id;
		public uint longNameId;
		public uint shortNameId;
		
	}
}
