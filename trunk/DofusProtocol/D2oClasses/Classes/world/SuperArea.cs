using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[D2OClass("SuperAreas")]
	public class SuperArea : Object
	{
		internal const String MODULE = "SuperAreas";
		public int id;
		public uint nameId;
		public uint worldmapId;
		internal Array _allSuperAreas;
		
	}
}
