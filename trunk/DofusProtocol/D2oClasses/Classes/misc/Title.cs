using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Titles")]
	[Serializable]
	public class Title
	{
		private const String MODULE = "Titles";
		public int id;
		public uint nameId;
		public String color;
	}
}
