using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("ActionDescriptions")]
	[Serializable]
	public class ActionDescription
	{
		public const String MODULE = "ActionDescriptions";
		public uint id;
		public uint typeId;
		public String name;
		public uint descriptionId;
		public Boolean trusted;
		public Boolean needInteraction;
		public uint maxUsePerFrame;
		public uint minimalUseInterval;
		public Boolean needConfirmation;
	}
}
