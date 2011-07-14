using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("CensoredWords")]
	[Serializable]
	public class CensoredWord
	{
		private const String MODULE = "CensoredWords";
		public uint id;
		public uint listId;
		public String language;
		public String word;
		public Boolean deepLooking;
	}
}
