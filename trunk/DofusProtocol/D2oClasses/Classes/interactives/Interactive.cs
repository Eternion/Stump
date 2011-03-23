using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.interactives
{
	
	[D2OClass("Interactives")]
	public class Interactive : Object
	{
		internal const String MODULE = "Interactives";
		public int id;
		public uint nameId;
		public int actionId;
		
	}
}
