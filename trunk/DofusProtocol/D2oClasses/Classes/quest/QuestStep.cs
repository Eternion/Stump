using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("QuestSteps")]
	[Serializable]
	public class QuestStep
	{
		private const String MODULE = "QuestSteps";
		public uint id;
		public uint questId;
		public uint nameId;
		public uint descriptionId;
		public int dialogId;
		public uint optimalLevel;
		public float duration;
		public Boolean kamasScaleWithPlayerLevel;
		public float kamasRatio;
		public float xpRatio;
		public List<uint> objectiveIds;
		public List<uint> rewardsIds;
	}
}
