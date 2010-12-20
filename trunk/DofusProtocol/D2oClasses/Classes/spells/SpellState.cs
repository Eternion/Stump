using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("SpellStates")]
	public class SpellState : Object
	{
		internal const String MODULE = "SpellStates";
		public int id;
		public uint nameId;
		public Boolean preventsSpellCast;
		public Boolean preventsFight;
		public Boolean critical;
		
	}
}
