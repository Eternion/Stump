using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.communication
{
	
	[D2OClass("InfoMessages")]
	public class InfoMessage : Object
	{
		internal const String MODULE = "InfoMessages";
		public uint typeId;
		public uint messageId;
		public uint textId;
		
	}
}
