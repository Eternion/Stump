using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("SkillNames")]
	[Serializable]
	public class SkillName
	{
		private const String MODULE = "SkillNames";
		public int id;
		public uint nameId;
	}
}
