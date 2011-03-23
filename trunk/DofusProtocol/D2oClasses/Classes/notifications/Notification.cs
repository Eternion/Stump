using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.notifications
{
	
	[D2OClass("Notifications")]
	public class Notification : Object
	{
		internal const String MODULE = "Notifications";
		public int id;
		public uint titleId;
		public uint messageId;
		public int iconId;
		public int typeId;
		public String trigger;
		
	}
}
