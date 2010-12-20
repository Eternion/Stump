using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("Challenge")]
	public class Challenge : Object
	{
		internal const String MODULE = "Challenge";
		public int id;
		public uint nameId;
		public uint descriptionId;
		
	}
}
