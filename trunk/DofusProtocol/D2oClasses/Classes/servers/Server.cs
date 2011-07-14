using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Servers")]
	[Serializable]
	public class Server
	{
		private const String MODULE = "Servers";
		public int id;
		public uint nameId;
		public uint commentId;
		public float openingDate;
		public String language;
		public int populationId;
		public uint gameTypeId;
		public int communityId;
		public List<String> restrictedToLanguages;
	}
}
