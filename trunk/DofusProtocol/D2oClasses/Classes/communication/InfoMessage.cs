using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("InfoMessages")]
	[Serializable]
	public class InfoMessage
	{
		private const String MODULE = "InfoMessages";
		public uint typeId;
		public uint messageId;
		public uint textId;
	}
}
