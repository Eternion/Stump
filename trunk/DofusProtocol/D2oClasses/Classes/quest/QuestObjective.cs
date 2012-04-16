using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("QuestObjectives")]
	[Serializable]
	public class QuestObjective
	{
		private const String MODULE = "QuestObjectives";
		public uint id;
		public uint stepId;
		public uint typeId;
		public int dialogId;
		public List<uint> parameters;
		public Point coords;
	}
}
