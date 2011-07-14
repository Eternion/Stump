using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("SpellBombs")]
	[Serializable]
	public class SpellBomb
	{
		private const String MODULE = "SpellBombs";
		public int id;
		public int chainReactionSpellId;
		public int explodSpellId;
		public int wallId;
		public int instantSpellId;
		public int comboCoeff;
	}
}
