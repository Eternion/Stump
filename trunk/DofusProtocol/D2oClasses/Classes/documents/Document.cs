using System;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Documents")]
	public class Document
	{
		private const String MODULE = "Documents";
		public int id;
		public uint typeId;
		public uint titleId;
		public uint authorId;
		public uint subTitleId;
		public uint contentId;
	}
}
