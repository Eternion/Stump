using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.alignments
{
	
	[D2OClass("AlignmentGift")]
	public class AlignmentGift : Object
	{
		internal const String MODULE = "AlignmentGift";
		public int id;
		public uint nameId;
		public int effectId;
		public uint gfxId;
		
	}
}
