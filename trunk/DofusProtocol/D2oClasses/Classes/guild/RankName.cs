using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("RankNames")]
	public class RankName : Object
	{
		internal const String MODULE = "RankNames";
		public int id;
		public uint nameId;
		public int order;
		
	}
}
