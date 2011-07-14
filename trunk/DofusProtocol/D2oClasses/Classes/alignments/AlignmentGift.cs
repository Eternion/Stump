using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("AlignmentGift")]
	[Serializable]
	public class AlignmentGift
	{
		private const String MODULE = "AlignmentGift";
		public int id;
		public uint nameId;
		public int effectId;
		public uint gfxId;
	}
}
