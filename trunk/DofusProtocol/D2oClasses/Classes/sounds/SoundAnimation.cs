using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("SoundAnimations")]
	[Serializable]
	public class SoundAnimation
	{
		public uint id;
		public String name;
		public String label;
		public String filename;
		public int volume;
		public int rolloff;
		public int automationDuration;
		public int automationVolume;
		public int automationFadeIn;
		public int automationFadeOut;
		public Boolean noCutSilence;
		public uint startFrame;
		public String MODULE = "SoundAnimations";
	}
}
