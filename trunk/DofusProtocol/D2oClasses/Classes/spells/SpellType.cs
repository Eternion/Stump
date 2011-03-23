using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.spells
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
