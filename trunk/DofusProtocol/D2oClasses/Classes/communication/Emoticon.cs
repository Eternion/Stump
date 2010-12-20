using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("Emoticons")]
	public class Emoticon : Object
	{
		internal const String MODULE = "Emoticons";
		public uint id;
		public uint nameId;
		public uint shortcutId;
		public String defaultAnim;
		public Boolean instant;
		public Boolean eight_directions;
		public Boolean aura;
		public List<String> anims;
		
	}
}
