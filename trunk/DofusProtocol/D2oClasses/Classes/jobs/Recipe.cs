using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Recipes")]
	[Serializable]
	public class Recipe
	{
		private const String MODULE = "Recipes";
		public int resultId;
		public uint resultLevel;
		public List<int> ingredientIds;
		public List<uint> quantities;
	}
}
