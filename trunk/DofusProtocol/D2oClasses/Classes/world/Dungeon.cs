using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Dungeons")]
	[Serializable]
	public class Dungeon
	{
		private const String MODULE = "Dungeons";
		public int id;
		public uint nameId;
		public int optimalPlayerLevel;
	}
}
