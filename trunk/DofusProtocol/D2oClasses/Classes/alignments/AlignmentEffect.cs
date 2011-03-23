using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.alignments
{
	
	[D2OClass("AlignmentEffect")]
	public class AlignmentEffect : Object
	{
		internal const String MODULE = "AlignmentEffect";
		public int id;
		public uint characteristicId;
		public uint descriptionId;
		
	}
}
