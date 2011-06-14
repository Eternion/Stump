using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("AbuseReasons")]
	public class AbuseReasons
	{
		private const String MODULE = "AbuseReasons";
		public uint _abuseReasonId;
		public uint _mask;
		public int _reasonTextId;
	}
}
