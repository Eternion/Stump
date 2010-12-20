using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("MonsterRaces")]
	public class MonsterRace : Object
	{
		internal const String MODULE = "MonsterRaces";
		public int id;
		public int superRaceId;
		public uint nameId;
		
	}
}
