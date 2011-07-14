using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("RideFood")]
	[Serializable]
	public class RideFood
	{
		public uint gid;
		public uint typeId;
		public String MODULE = "RideFood";
	}
}
