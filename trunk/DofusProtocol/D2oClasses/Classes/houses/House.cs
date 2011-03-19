using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[D2OClass("Houses")]
	public class House : Object
	{
		internal const String MODULE = "Houses";
		public int typeId;
		public uint defaultPrice;
		public int nameId;
		public int descriptionId;
		public int gfxId;
		
	}
}
