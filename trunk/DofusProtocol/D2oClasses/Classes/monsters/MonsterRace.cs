using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.monsters
{
	
	[D2OClass("MonsterRaces")]
	public class MonsterRace : Object
	{
		internal const String MODULE = "MonsterRaces";
		public int id;
		public int superRaceId;
		public uint nameId;
		
	}
}
