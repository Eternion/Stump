using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("AlignmentTitles")]
	[Serializable]
	public class AlignmentTitle
	{
		private const String MODULE = "AlignmentTitles";
		public int sideId;
		public List<int> namesId;
		public List<int> shortsId;
	}
}
