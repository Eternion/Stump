// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Classes.ambientSounds;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.DofusProtocol.D2oClasses.Classes.world
{
	
	[D2OClass("MapPositions")]
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
        public int worldMap;
		public Boolean outdoor;
		
	}
}
