using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("AlignmentRank")]
	public class AlignmentRank : Object
	{
		internal const String MODULE = "AlignmentRank";
		public int id;
		public uint orderId;
		public uint nameId;
		public uint descriptionId;
		public int minimumAlignment;
		public int objectsStolen;
		public List<int> gifts;
		
	}
}
