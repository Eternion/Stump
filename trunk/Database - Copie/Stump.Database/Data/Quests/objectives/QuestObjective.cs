
using System;
using System.Collections.Generic;

namespace Stump.Database.Data.Quests.objectives
{
	public class QuestObjective : Object
	{
		public uint id;
		public uint stepId;
		public uint typeId;
		public List<uint> parameters;		public Point coords;		
	}
}
