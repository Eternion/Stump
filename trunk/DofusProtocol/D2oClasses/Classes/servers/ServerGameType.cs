using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("ServerGameTypes")]
	[Serializable]
	public class ServerGameType
	{
		private const String MODULE = "ServerGameTypes";
		public int id;
		public uint nameId;
	}
}
