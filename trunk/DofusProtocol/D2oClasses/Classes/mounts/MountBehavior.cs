using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.mounts
{
	
	[D2OClass("MountBehaviors")]
	public class MountBehavior : Object
	{
		public const String MODULE = "MountBehaviors";
		public uint id;
		public uint nameId;
		public uint descriptionId;
		
	}
}
