using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.guild
{
	
	[D2OClass("RankNames")]
	public class RankName : Object
	{
		internal const String MODULE = "RankNames";
		public int id;
		public uint nameId;
		public int order;
		
	}
}
