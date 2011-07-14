using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("MonsterSuperRaces")]
	[Serializable]
	public class MonsterSuperRace
	{
		private const String MODULE = "MonsterSuperRaces";
		public int id;
		public uint nameId;
	}
}
