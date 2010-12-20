using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("Effects")]
	public class Effect : Object
	{
		internal const String MODULE = "Effects";
		public int id;
		public uint descriptionId;
		public uint iconId;
		public int characteristic;
		public uint category;
		public String @operator;
		public Boolean showInTooltip;
		public Boolean useDice;
		public Boolean showInSet;
		public int bonusType;
		
	}
}
