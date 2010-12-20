using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("Npcs")]
	public class Npc : Object
	{
		internal const String MODULE = "Npcs";
		public int id;
		public uint nameId;
		public List<List<int>> dialogMessages;
		public List<List<int>> dialogReplies;
		public List<uint> actions;
		public uint gender;
		public String look;
		
	}
}
