using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.quest
{
	
	[D2OClass("QuestObjectiveTypes")]
	public class QuestObjectiveType : Object
	{
		internal const String MODULE = "QuestObjectiveTypes";
		public uint id;
		public uint nameId;
		
	}
}
