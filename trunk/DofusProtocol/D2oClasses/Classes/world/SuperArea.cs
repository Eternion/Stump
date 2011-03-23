using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.world
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
