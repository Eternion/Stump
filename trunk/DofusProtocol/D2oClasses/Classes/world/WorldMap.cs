using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("WorldMaps")]
	[Serializable]
	public class WorldMap
	{
		private const String MODULE = "WorldMaps";
		public int id;
		public int origineX;
		public int origineY;
		public float mapWidth;
		public float mapHeight;
		public uint horizontalChunck;
		public uint verticalChunck;
		public Boolean viewableEverywhere;
		public float minScale;
		public float maxScale;
		public float startScale;
		public int centerX;
		public int centerY;
		public int totalWidth;
		public int totalHeight;
		public List<String> zoom;
	}
}
