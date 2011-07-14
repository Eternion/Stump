using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Notifications")]
	[Serializable]
	public class Notification
	{
		private const String MODULE = "Notifications";
		public int id;
		public uint titleId;
		public uint messageId;
		public int iconId;
		public int typeId;
		public String trigger;
	}
}
