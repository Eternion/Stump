using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Url")]
	[Serializable]
	public class Url
	{
		private const String MODULE = "Url";
		public int id;
		public int browserId;
		public String url;
		public String param;
		public String method;
	}
}
