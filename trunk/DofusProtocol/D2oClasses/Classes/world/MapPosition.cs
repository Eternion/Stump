using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("MapPositions")]
	[Serializable]
	public class MapPosition
	{
		private const String MODULE = "MapPositions";
		public int id;
		public int posX;
		public int posY;
		public Boolean outdoor;
		public int capabilities;
		public int nameId;
		public List<AmbientSound> sounds;
		public int subAreaId;
		public int worldMap;
		public Boolean hasPriorityOnWorldmap;
	}
}
