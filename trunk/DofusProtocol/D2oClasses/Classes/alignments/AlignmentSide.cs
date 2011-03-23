using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.alignments
{
	
	[D2OClass("AlignmentSides")]
	public class AlignmentSide : Object
	{
		internal const String MODULE = "AlignmentSides";
		public int id;
		public uint nameId;
		public Boolean canConquest;
		
	}
}
