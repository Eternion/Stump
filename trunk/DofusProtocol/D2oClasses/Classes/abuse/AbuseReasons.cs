using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("AbuseReasons")]
	public class AbuseReasons : Object
	{
		internal const String MODULE = "AbuseReasons";
		public uint _abuseReasonId;
		public uint _mask;
		public int _reasonTextId;
		
	}
}
