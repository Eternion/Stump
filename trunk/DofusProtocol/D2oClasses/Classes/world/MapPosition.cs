using System;
using System.Collections.Generic;
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("MapPositions")]
	public class MapPosition : Object
	{
		internal const int CAPABILITY_ALLOW_FIGHT_CHALLENGES = 4096;
		internal const int CAPABILITY_ALLOW_TELEPORT_EVERYWHERE = 2048;
		internal const int CAPABILITY_ALLOW_COLLECTOR = 64;
		internal const String MODULE = "MapPositions";

		internal const int CAPABILITY_ALLOW_CHALLENGE = 1;
		internal const int CAPABILITY_ALLOW_AGGRESSION = 2;
		internal const int CAPABILITY_ALLOW_TELEPORT_TO = 4;
		internal const int CAPABILITY_ALLOW_TELEPORT_FROM = 8;
		internal const int CAPABILITY_ALLOW_EXCHANGES_BETWEEN_PLAYERS = 16;
		internal const int CAPABILITY_ALLOW_HUMAN_VENDOR = 32;
		internal const int CAPABILITY_ALLOW_SOUL_CAPTURE = 128;
		internal const int CAPABILITY_ALLOW_SOUL_SUMMON = 256;
		internal const int CAPABILITY_ALLOW_TAVERN_REGEN = 512;
		internal const int CAPABILITY_ALLOW_TOMB_MODE = 1024;
		public int id;
		public int posX;
		public int posY;
		public List<AmbientSound> sounds;
		public int capabilities;
		public int subAreaId;
		public int nameId;
		public Boolean outdoor;
		
	}
}
