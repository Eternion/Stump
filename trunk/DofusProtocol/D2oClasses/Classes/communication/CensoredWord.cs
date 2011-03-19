using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
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
