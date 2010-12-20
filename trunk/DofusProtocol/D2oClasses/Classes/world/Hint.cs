using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("Hints")]
	public class Hint : Object
	{
		internal const String MODULE = "Hints";
		public int id;
		public uint categoryId;
		public uint gfx;
		public uint nameId;
		public uint mapId;
		public uint realMapId;
		
	}
}
