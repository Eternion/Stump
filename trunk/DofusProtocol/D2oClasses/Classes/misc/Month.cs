using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.misc
{
	
	[D2OClass("Months")]
	public class Month : Object
	{
		internal const String MODULE = "Months";
		public int id;
		public uint nameId;
		
	}
}
