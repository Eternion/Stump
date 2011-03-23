using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.livingObjects
{
	
	[D2OClass("LivingObjectSkinJntMood")]
	public class LivingObjectSkinJntMood : Object
	{
		internal const String MODULE = "LivingObjectSkinJntMood";
		public int skinId;
		public List<List<int>> moods;
		
	}
}
