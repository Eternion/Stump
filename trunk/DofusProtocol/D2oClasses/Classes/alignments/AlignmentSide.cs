using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("AlignmentSides")]
	public class AlignmentSide : Object
	{
		internal const String MODULE = "AlignmentSides";
		public int id;
		public uint nameId;
		public Boolean canConquest;
		
	}
}
