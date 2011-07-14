using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("SpeakingItemsTriggers")]
	[Serializable]
	public class SpeakingItemsTrigger
	{
		private const String MODULE = "SpeakingItemsTriggers";
		public int triggersId;
		public List<int> textIds;
		public List<int> states;
	}
}
