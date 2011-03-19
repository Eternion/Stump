using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[D2OClass("Months")]
	public class Month : Object
	{
		internal const String MODULE = "Months";
		public int id;
		public uint nameId;
		
	}
}
