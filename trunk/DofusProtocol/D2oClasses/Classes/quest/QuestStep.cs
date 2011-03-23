using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.quest
{
	
	[D2OClass("QuestSteps")]
	public class QuestStep : Object
	{
		internal const String MODULE = "QuestSteps";
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
