using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("QuestStepRewards")]
	[Serializable]
	public class QuestStepRewards
	{
		private const String MODULE = "QuestStepRewards";
		public uint id;
		public uint stepId;
		public int levelMin;
		public int levelMax;
		public List<List<uint>> itemsReward;
		public List<uint> emotesReward;
		public List<uint> jobsReward;
		public List<uint> spellsReward;
	}
}
