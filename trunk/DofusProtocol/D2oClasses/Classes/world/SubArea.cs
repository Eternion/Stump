using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("SubAreas")]
	[Serializable]
	public class SubArea
	{
		private const String MODULE = "SubAreas";
		public int id;
		public uint nameId;
		public int areaId;
		public List<AmbientSound> ambientSounds;
		public List<uint> mapIds;
		public Rectangle bounds;
		public List<int> shape;
		public List<uint> customWorldMap;
		public int packId;
	}
}
