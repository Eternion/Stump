using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("SpeakingItemsText")]
	[Serializable]
	public class SpeakingItemText
	{
		private const String MODULE = "SpeakingItemsText";
		public int textId;
		public float textProba;
		public uint textStringId;
		public int textLevel;
		public int textSound;
		public String textRestriction;
	}
}
