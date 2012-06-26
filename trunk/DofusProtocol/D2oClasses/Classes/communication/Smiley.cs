using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Smileys")]
	[Serializable]
	public class Smiley
	{
		private const String MODULE = "Smileys";
		public uint id;
		public uint order;
		public String gfxId;
		public Boolean forPlayers;
	}
}
