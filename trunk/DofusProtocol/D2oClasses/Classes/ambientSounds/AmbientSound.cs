using System;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.ambientSounds
{
	
	[D2OClass("AmbientSounds")]
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
