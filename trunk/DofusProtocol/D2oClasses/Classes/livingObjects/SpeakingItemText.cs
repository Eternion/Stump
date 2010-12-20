using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("SpeakingItemsText")]
	public class SpeakingItemText : Object
	{
		internal const String MODULE = "SpeakingItemsText";
		public int textId;
		public double textProba;
		public uint textStringId;
		public int textLevel;
		public int textSound;
		public String textRestriction;
		
	}
}
