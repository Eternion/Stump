using System;
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
	[D2OClass("Spells")]
	[Serializable]
	public class Spell
	{
		private const String MODULE = "Spells";
		public int id;
		public uint nameId;
		public uint descriptionId;
		public uint typeId;
		public String scriptParams;
		public String scriptParamsCritical;
		public int scriptId;
		public int scriptIdCritical;
		public int iconId;
		public List<uint> spellLevels;
		public Boolean useParamCache = true;
	}
}
