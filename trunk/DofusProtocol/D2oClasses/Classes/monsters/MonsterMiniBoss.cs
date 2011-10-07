using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("MonsterMiniBoss")]
	[Serializable]
	public class MonsterMiniBoss
	{
		private const String MODULE = "MonsterMiniBoss";
		public int id;
		public int monsterReplacingId;
	}
}
