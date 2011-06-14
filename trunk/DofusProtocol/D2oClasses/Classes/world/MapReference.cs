using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("MapReferences")]
	public class MapReference
	{
		private const String MODULE = "MapReferences";
		public int id;
		public uint mapId;
		public int cellId;
	}
}
