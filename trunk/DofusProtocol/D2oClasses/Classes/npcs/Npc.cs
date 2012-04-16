using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Npcs")]
	[Serializable]
	public class Npc
	{
		private const String MODULE = "Npcs";
		public int id;
		public uint nameId;
		public List<List<int>> dialogMessages;
		public List<List<int>> dialogReplies;
		public List<uint> actions;
		public uint gender;
		public String look;
		public int tokenShop;
		public List<AnimFunNpcData> animFunList;
	}
}
