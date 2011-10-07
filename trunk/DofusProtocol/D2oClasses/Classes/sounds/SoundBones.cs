using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("SoundBones")]
	[Serializable]
	public class SoundBones
	{
		public uint id;
		public List<String> keys;
		public List<List<SoundAnimation>> values;
		public String MODULE = "SoundBones";
	}
}
