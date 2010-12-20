using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("AmbientSounds")]
	public class AmbientSound : Object
	{
		internal const String MODULE = "AmbientSounds";
		public int id;
		public uint volume;
		public int criterionId;
		public uint silenceMin;
		public uint silenceMax;
		public int channel;
		
	}
}
