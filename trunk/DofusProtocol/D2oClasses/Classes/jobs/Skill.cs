using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.jobs
{
	
	[D2OClass("Skills")]
	public class Skill : Object
	{
		internal const String MODULE = "Skills";
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
		
	}
}
