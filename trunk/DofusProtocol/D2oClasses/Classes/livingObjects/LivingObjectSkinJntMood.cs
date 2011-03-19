using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[D2OClass("LivingObjectSkinJntMood")]
	public class LivingObjectSkinJntMood : Object
	{
		internal const String MODULE = "LivingObjectSkinJntMood";
		public int skinId;
		public List<List<int>> moods;
		
	}
}
