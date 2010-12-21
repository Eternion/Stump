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
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GuildInformations : BasicGuildInformations
	{
		public const uint protocolId = 127;
		public GuildEmblem guildEmblem;
		
		public GuildInformations()
		{
			this.guildEmblem = new GuildEmblem();
		}
		
		public GuildInformations(uint arg1, String arg2, GuildEmblem arg3)
			: this()
		{
			initGuildInformations(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 127;
		}
		
		public GuildInformations initGuildInformations(uint arg1 = 0, String arg2 = "", GuildEmblem arg3 = null)
		{
			base.initBasicGuildInformations(arg1, arg2);
			this.guildEmblem = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.guildEmblem = new GuildEmblem();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GuildInformations(arg1);
		}
		
		public void serializeAs_GuildInformations(BigEndianWriter arg1)
		{
			base.serializeAs_BasicGuildInformations(arg1);
			this.guildEmblem.serializeAs_GuildEmblem(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildInformations(arg1);
		}
		
		public void deserializeAs_GuildInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.guildEmblem = new GuildEmblem();
			this.guildEmblem.deserialize(arg1);
		}
		
	}
}
