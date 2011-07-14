using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Areas")]
	[Serializable]
	public class Area
	{
		private const String MODULE = "Areas";
		public int id;
		public uint nameId;
		public int superAreaId;
		public Boolean containHouses;
		public Boolean containPaddocks;
		public Rectangle bounds;
	}
}
