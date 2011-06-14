using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("QuestSteps")]
	public class QuestStep
	{
		private const String MODULE = "QuestSteps";
		public uint id;
		public uint questId;
		public uint nameId;
		public uint descriptionId;
		public int dialogId;
		public uint optimalLevel;
		public uint experienceReward;
		public uint kamasReward;
		public List<List<uint>> itemsReward;
		public List<uint> emotesReward;
		public List<uint> jobsReward;
		public List<uint> spellsReward;
		public List<uint> objectiveIds;
	}
}
