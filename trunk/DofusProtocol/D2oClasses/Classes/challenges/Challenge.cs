using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Challenge")]
	[Serializable]
	public class Challenge
	{
		private const String MODULE = "Challenge";
		public int id;
		public uint nameId;
		public uint descriptionId;
	}
}
