using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("MapReferences")]
	public class MapReference : Object
	{
		internal const String MODULE = "MapReferences";
		public int id;
		public uint mapId;
		public int cellId;
		
	}
}
