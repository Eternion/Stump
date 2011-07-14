using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("QuestObjectiveTypes")]
	[Serializable]
	public class QuestObjectiveType
	{
		private const String MODULE = "QuestObjectiveTypes";
		public uint id;
		public uint nameId;
	}
}
