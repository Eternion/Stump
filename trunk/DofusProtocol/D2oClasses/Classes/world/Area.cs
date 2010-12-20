using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("Areas")]
	public class Area : Object
	{
		internal const String MODULE = "Areas";
		public int id;
		public uint nameId;
		public int superAreaId;
		public Boolean containHouses;
		public Boolean containPaddocks;
		public Rectangle bounds;
		internal Array _allAreas;
		
	}
}
