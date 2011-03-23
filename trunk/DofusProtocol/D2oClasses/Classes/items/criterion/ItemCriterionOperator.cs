using System;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.items.criterion
{
	
	public class ItemCriterionOperator : Object
	{
		public const String SUPERIOR = ">";
		public const String INFERIOR = "<";
		public const String EQUAL = "";
		public const String DIFFERENT = "!";
		public static Array OPERATORS_LIST = new [] { SUPERIOR, INFERIOR, EQUAL, DIFFERENT, "#", "~", "s", "S", "e", "E", "v", "i", "X", "/" };
		
	}
}
