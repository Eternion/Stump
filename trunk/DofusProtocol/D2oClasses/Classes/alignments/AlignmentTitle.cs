using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("AlignmentTitles")]
	public class AlignmentTitle : Object
	{
		internal const String MODULE = "AlignmentTitles";
		public int sideId;
		public List<int> namesId;
		public List<int> shortsId;
		
	}
}
