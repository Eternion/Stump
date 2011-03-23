using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.npcs
{
	
	[D2OClass("Npcs")]
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
