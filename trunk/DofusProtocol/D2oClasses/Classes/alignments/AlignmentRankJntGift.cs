using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("AlignmentRankJntGift")]
	[Serializable]
	public class AlignmentRankJntGift
	{
		private const String MODULE = "AlignmentRankJntGift";
		public int id;
		public List<int> gifts;
		public List<int> parameters;
		public List<int> levels;
	}
}
