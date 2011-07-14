using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("MonsterRaces")]
	[Serializable]
	public class MonsterRace
	{
		private const String MODULE = "MonsterRaces";
		public int id;
		public int superRaceId;
		public uint nameId;
	}
}
