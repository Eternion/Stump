using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
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
