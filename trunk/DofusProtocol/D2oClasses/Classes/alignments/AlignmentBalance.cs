using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.alignments
{
	
	[D2OClass("AlignmentBalance")]
	public class AlignmentBalance : Object
	{
		internal const String MODULE = "AlignmentBalance";
		public int id;
		public int startValue;
		public int endValue;
		public uint nameId;
		public uint descriptionId;
		
	}
}
