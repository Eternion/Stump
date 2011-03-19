using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[D2OClass("NpcActions")]
	public class NpcAction : Object
	{
		internal const String MODULE = "NpcActions";
		public int id;
		public uint nameId;
		
	}
}
