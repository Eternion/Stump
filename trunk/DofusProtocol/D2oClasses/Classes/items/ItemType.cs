using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("ItemTypes")]
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
