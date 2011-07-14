using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[Serializable]
	public class NotificationButton
	{
		public String label;
		public String action;
		public Object @params;
		public int width = 150;
		public int height = 32;
	}
}
