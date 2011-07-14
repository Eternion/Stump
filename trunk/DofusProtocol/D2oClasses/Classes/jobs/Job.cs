using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Jobs")]
	[Serializable]
	public class Job
	{
		private const String MODULE = "Jobs";
		public int id;
		public uint nameId;
		public int specializationOfId;
		public int iconId;
		public List<int> toolIds;
	}
}
