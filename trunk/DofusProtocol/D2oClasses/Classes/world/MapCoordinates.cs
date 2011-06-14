using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("MapCoordinates")]
	public class MapCoordinates
	{
		private const String MODULE = "MapCoordinates";
		public uint compressedCoords;
		public List<int> mapIds;
	}
}
