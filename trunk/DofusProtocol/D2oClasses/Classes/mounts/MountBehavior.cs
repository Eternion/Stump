using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
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
