using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
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
