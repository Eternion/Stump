using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("CensoredContents")]
	[Serializable]
	public class CensoredContent
	{
		public const String MODULE = "CensoredContents";
        public int type;
        public int oldValue;
        public int newValue;
        public string lang;
	}
}
