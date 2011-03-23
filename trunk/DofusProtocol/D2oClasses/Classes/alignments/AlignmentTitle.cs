using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.alignments
{
	
	[D2OClass("AlignmentTitles")]
	public class AlignmentTitle : Object
	{
		internal const String MODULE = "AlignmentTitles";
		public int sideId;
		public List<int> namesId;
		public List<int> shortsId;
		
	}
}
