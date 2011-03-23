using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.npcs
{
	
	[D2OClass("NpcMessages")]
	public class NpcMessage : Object
	{
		internal const String MODULE = "NpcMessages";
		public int id;
		public uint messageId;
		public String messageParams;
		
	}
}
