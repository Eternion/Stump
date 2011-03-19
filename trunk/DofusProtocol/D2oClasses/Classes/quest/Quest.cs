using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
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
