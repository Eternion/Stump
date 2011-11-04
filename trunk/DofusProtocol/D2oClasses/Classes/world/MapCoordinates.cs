using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("MapCoordinates")]
	[Serializable]
	public class MapCoordinates
	{
		private const String MODULE = "MapCoordinates";
		public int compressedCoords;
		public List<int> mapIds;
	}
}
