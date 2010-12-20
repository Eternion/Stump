using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("Monsters")]
	public class Monster : Object
	{
		internal const String MODULE = "Monsters";
		public int id;
		public uint nameId;
		public uint gfxId;
		public int race;
		public List<MonsterGrade> grades;
		public String look;
		
	}
}
