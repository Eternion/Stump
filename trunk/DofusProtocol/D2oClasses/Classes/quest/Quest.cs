using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.quest
{
	
	[D2OClass("Quests")]
	public class Quest : Object
	{
		internal const String MODULE = "Quests";
		public uint id;
		public uint nameId;
		public List<uint> stepIds;
		
	}
}
