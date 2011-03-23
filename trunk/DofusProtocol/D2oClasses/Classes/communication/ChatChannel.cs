using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.communication
{
	
	[D2OClass("ChatChannels")]
	public class ChatChannel : Object
	{
		internal const String MODULE = "ChatChannels";
		public uint id;
		public uint nameId;
		public uint descriptionId;
		public String shortcut;
		public String shortcutKey;
		public Boolean isPrivate;
		public Boolean allowObjects;
		
	}
}
