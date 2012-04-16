using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("TypeActions")]
	[Serializable]
	public class TypeAction
	{
		public const String MODULE = "TypeActions";
		public int id;
		public String elementName;
		public int elementId;
	}
}
