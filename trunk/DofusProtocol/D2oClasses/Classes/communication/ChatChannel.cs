using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
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
