using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("Titles")]
	public class Title : Object
	{
		internal const String MODULE = "Titles";
		public int id;
		public uint nameId;
		public String color;
		
	}
}
