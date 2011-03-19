using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[D2OClass("Challenge")]
	public class Challenge : Object
	{
		internal const String MODULE = "Challenge";
		public int id;
		public uint nameId;
		public uint descriptionId;
		
	}
}
