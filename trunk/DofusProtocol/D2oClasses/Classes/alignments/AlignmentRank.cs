using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.alignments
{
	
	[D2OClass("AlignmentRank")]
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
