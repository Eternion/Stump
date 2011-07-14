using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Quests")]
	[Serializable]
	public class Quest
	{
		private const String MODULE = "Quests";
		public uint id;
		public uint nameId;
		public List<uint> stepIds;
	}
}
