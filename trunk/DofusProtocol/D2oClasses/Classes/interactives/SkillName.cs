using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[D2OClass("SkillNames")]
	public class SkillName : Object
	{
		internal const String MODULE = "SkillNames";
		public int id;
		public uint nameId;
		
	}
}
