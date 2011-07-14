using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("NpcActions")]
	[Serializable]
	public class NpcAction
	{
		private const String MODULE = "NpcActions";
		public int id;
		public uint nameId;
	}
}
