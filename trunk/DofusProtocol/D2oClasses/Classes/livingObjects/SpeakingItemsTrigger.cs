using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.livingObjects
{
	
	[D2OClass("SpeakingItemsTriggers")]
	public class SpeakingItemsTrigger : Object
	{
		internal const String MODULE = "SpeakingItemsTriggers";
		public int triggersId;
		public List<int> textIds;
		public List<int> states;
		
	}
}
