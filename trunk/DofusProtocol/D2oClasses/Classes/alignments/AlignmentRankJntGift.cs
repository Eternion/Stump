using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("AlignmentRankJntGift")]
	public class AlignmentRankJntGift : Object
	{
		internal const String MODULE = "AlignmentRankJntGift";
		public int id;
		public List<int> gifts;
		public List<int> parameters;
		public List<int> levels;
		
	}
}
