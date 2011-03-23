using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.world
{
	
	[D2OClass("Hints")]
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
