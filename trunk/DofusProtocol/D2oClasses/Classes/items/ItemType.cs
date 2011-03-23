using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.items
{
	
	[D2OClass("ItemTypes")]
	public class ItemType : Object
	{
		internal const String MODULE = "ItemTypes";
		public int id;
		public uint nameId;
		public uint superTypeId;
		public Boolean plural;
		public uint gender;
		public uint zoneSize;
		public uint zoneShape;
		public Boolean needUseConfirm;
		
	}
}
