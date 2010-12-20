using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	public class EffectInstance : Object
	{
		internal const String UNKNOWN_NAME = "???";
		internal const int UNDEFINED_CATEGORY = -2;
		internal const int UNDEFINED_SHOW = -1;
		internal const String UNDEFINED_DESCRIPTION = "undefined";
		public uint effectId;
		public int targetId;
		public int duration;
		public int random;
		public Boolean trigger;
		public uint zoneSize;
		public uint zoneShape;
		public int modificator;
		
	}
}
