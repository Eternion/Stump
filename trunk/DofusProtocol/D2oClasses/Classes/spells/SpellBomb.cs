using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.spells
{
	
	[D2OClass("SpellBombs")]
	public class SpellBomb : Object
	{
		internal const String MODULE = "SpellBombs";
		public int id;
		public int chainReactionSpellId;
		public int explodSpellId;
		public int wallId;
		public int instantSpellId;
		public int comboCoeff;
		
	}
}
