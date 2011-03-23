using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.misc
{
	
	[D2OClass("Titles")]
	public class Title : Object
	{
		internal const String MODULE = "Titles";
		public int id;
		public uint nameId;
		public String color;
		
	}
}
