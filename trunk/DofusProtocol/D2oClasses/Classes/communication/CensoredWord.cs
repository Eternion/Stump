using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.communication
{
	
	[D2OClass("CensoredWords")]
	public class CensoredWord : Object
	{
		internal const String MODULE = "CensoredWords";
		public uint id;
		public uint listId;
		public String language;
		public String word;
		public Boolean deepLooking;
		
	}
}
