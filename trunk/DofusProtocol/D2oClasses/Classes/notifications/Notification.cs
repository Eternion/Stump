using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
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
