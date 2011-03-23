using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.interactives
{
	
	[D2OClass("SkillNames")]
	public class SkillName : Object
	{
		internal const String MODULE = "SkillNames";
		public int id;
		public uint nameId;
		
	}
}
