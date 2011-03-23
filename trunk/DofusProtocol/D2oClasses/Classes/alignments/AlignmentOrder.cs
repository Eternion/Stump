using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.alignments
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
