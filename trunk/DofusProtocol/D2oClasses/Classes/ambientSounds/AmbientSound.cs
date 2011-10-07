using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("AmbientSounds")]
	[Serializable]
	public class AmbientSound
	{
		public const int AMBIENT_TYPE_ROLEPLAY = 1;
		public const int AMBIENT_TYPE_AMBIENT = 2;
		public const int AMBIENT_TYPE_FIGHT = 3;
		public const int AMBIENT_TYPE_BOSS = 4;
		private const String MODULE = "AmbientSounds";
		public int id;
		public uint volume;
		public int criterionId;
		public uint silenceMin;
		public uint silenceMax;
		public int channel;
		public int type_id;
	}
}
