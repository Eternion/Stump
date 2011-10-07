using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Pets")]
	[Serializable]
	public class Pet
	{
		private const String MODULE = "Pets";
		public int id;
		public List<int> foodItems;
		public List<int> foodTypes;
	}
}
