using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("QuestCategory")]
	[Serializable]
	public class QuestCategory
	{
		private const String MODULE = "QuestCategory";
		public uint id;
		public uint nameId;
		public uint order;
		public List<uint> questIds;
	}
}
