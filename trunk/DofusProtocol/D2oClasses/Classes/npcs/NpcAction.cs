using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.npcs
{
	
	[D2OClass("NpcActions")]
	public class NpcAction : Object
	{
		internal const String MODULE = "NpcActions";
		public int id;
		public uint nameId;
		
	}
}
