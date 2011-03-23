using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.abuse
{
	
	[D2OClass("AbuseReasons")]
	public class AbuseReasons : Object
	{
		internal const String MODULE = "AbuseReasons";
		public uint _abuseReasonId;
		public uint _mask;
		public int _reasonTextId;
		
	}
}
