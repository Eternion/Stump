using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.quest
{
	
	[D2OClass("QuestObjectives")]
	public class QuestObjective : Object
	{
		internal const String MODULE = "QuestObjectives";
		public uint id;
		public uint stepId;
		public uint typeId;
		public List<uint> parameters;
		public Point coords;
		
	}
}
