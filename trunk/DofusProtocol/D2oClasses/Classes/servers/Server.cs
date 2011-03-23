using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.servers
{
	
	[D2OClass("Servers")]
	public class Server : Object
	{
		internal const String MODULE = "Servers";
		public int id;
		public uint nameId;
		public uint commentId;
		public double openingDate;
		public String language;
		public int populationId;
		public uint gameTypeId;
		public int communityId;
		public List<String> restrictedToLanguages;
		
	}
}
