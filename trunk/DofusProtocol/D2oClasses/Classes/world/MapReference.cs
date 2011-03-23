using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.world
{
	
	[D2OClass("MapReferences")]
	public class MapReference : Object
	{
		internal const String MODULE = "MapReferences";
		public int id;
		public uint mapId;
		public int cellId;
		
	}
}
