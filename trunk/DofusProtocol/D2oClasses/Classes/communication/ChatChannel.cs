using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("ChatChannels")]
	[Serializable]
	public class ChatChannel
	{
		private const String MODULE = "ChatChannels";
		public uint id;
		public uint nameId;
		public uint descriptionId;
		public String shortcut;
		public String shortcutKey;
		public Boolean isPrivate;
		public Boolean allowObjects;
	}
}
