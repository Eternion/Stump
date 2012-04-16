using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("ItemTypes")]
	[Serializable]
	public class ItemType
	{
		private const String MODULE = "ItemTypes";
		public int id;
		public uint nameId;
		public uint superTypeId;
		public Boolean plural;
		public uint gender;
		public String rawZone;
		public Boolean needUseConfirm;
	}
}
