using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[D2OClass("AlignmentOrder")]
	public class AlignmentOrder : Object
	{
		internal const String MODULE = "AlignmentOrder";
		public int id;
		public uint nameId;
		public uint sideId;
		
	}
}
