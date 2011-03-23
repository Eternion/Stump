using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.challenges
{
	
	[D2OClass("Challenge")]
	public class Challenge : Object
	{
		internal const String MODULE = "Challenge";
		public int id;
		public uint nameId;
		public uint descriptionId;
		
	}
}
