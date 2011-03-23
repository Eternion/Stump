using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Classes.ambientSounds;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.world
{
	
	[D2OClass("SubAreas")]
	public class SubArea : Object
	{
		internal const String MODULE = "SubAreas";
		public int id;
		public uint nameId;
		public int areaId;
		public List<AmbientSound> ambientSounds;
		public List<uint> mapIds;
		public Rectangle bounds;
		public List<int> shape;
		public List<uint> customWorldMap;
		public int packId;
		internal Array _allSubAreas;
		
	}
}
