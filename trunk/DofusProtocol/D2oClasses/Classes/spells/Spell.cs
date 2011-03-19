using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[D2OClass("Spells")]
	public class Spell : Object
	{
		internal const String MODULE = "Spells";
		internal Array _indexedParam;
		internal Array _indexedCriticalParam;
		public int id;
		public uint nameId;
		public uint descriptionId;
		public uint typeId;
		public String scriptParams;
		public String scriptParamsCritical;
		public int scriptId;
		public int scriptIdCritical;
		public uint iconId;
		public List<uint> spellLevels;
		public Boolean useParamCache = true;
		internal Array _spellLevels;
		
	}
}
