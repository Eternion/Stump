using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("ItemSets")]
	[Serializable]
	public class ItemSet
	{
		private const String MODULE = "ItemSets";
		public uint id;
		public List<uint> items;
		public uint nameId;
		public List<List<EffectInstance>> effects;
		public Boolean bonusIsSecret;
	}
}
