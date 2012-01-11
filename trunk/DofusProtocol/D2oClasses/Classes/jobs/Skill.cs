using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Skills")]
	[Serializable]
	public class Skill
	{
		private const String MODULE = "Skills";
		public int id;
		public uint nameId;
		public int parentJobId;
		public Boolean isForgemagus;
		public int modifiableItemType;
		public int gatheredRessourceItem;
		public List<int> craftableItemIds;
		public int interactiveId;
		public String useAnimation;
		public Boolean isRepair;
		public int cursor;
		public Boolean availableInHouse;
		public uint levelMin;
	}
}
