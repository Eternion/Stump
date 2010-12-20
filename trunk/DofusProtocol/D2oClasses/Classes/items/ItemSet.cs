using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("ItemSets")]
	public class ItemSet : Object
	{
		internal const String MODULE = "ItemSets";
		public uint id;
		public List<uint> items;
		public uint nameId;
		public List<List<EffectInstance>> effects;
		public Boolean bonusIsSecret;
		
	}
}
