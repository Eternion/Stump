using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.alignments
{
	
	[D2OClass("AlignmentRankJntGift")]
	public class AlignmentRankJntGift : Object
	{
		internal const String MODULE = "AlignmentRankJntGift";
		public int id;
		public List<int> gifts;
		public List<int> parameters;
		public List<int> levels;
		
	}
}
