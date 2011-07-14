using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("SpellStates")]
	[Serializable]
	public class SpellState
	{
		private const String MODULE = "SpellStates";
		public int id;
		public uint nameId;
		public Boolean preventsSpellCast;
		public Boolean preventsFight;
		public Boolean critical;
	}
}
