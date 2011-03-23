using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Classes.effects;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.items
{
	
	[D2OClass("ItemSets")]
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
