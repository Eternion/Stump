using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.jobs
{
	
	[D2OClass("Recipes")]
	public class Recipe : Object
	{
		internal const String MODULE = "Recipes";
		public int resultId;
		public uint resultLevel;
		public List<int> ingredientIds;
		public List<uint> quantities;
		
	}
}
