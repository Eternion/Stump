using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.spells
{
	
	[D2OClass("SpellStates")]
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
