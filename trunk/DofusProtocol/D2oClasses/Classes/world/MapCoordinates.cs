using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[D2OClass("MapCoordinates")]
	public class MapCoordinates : Object
	{
		internal const String MODULE = "MapCoordinates";
		internal const int UNDEFINED_COORD = int.MinValue;
		public uint compressedCoords;
		public List<int> mapIds;
		
	}
}
