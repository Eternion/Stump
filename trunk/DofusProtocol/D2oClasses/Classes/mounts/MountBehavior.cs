using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("MountBehaviors")]
	[Serializable]
	public class MountBehavior
	{
		public const String MODULE = "MountBehaviors";
		public uint id;
		public uint nameId;
		public uint descriptionId;
	}
}
