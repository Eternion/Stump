using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("Documents")]
	public class Document : Object
	{
		internal const String MODULE = "Documents";
		internal const String PAGEFEED = "<pagefeed/>";
		public int id;
		public uint typeId;
		public uint titleId;
		public uint authorId;
		public uint subTitleId;
		public uint contentId;
		internal Array _pages;
		
	}
}
