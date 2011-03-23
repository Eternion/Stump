using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.jobs
{
	
	[D2OClass("Jobs")]
	public class Job : Object
	{
		internal const String MODULE = "Jobs";
		public int id;
		public uint nameId;
		public int specializationOfId;
		public int iconId;
		public List<int> toolIds;
		
	}
}
