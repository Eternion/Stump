using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[D2OClass("SpellTypes")]
	public class SpellType : Object
	{
		internal const String MODULE = "SpellTypes";
		public int id;
		public uint longNameId;
		public uint shortNameId;
		
	}
}
