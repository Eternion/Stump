using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[D2OClass("AlignmentEffect")]
	public class AlignmentEffect : Object
	{
		internal const String MODULE = "AlignmentEffect";
		public int id;
		public uint characteristicId;
		public uint descriptionId;
		
	}
}
