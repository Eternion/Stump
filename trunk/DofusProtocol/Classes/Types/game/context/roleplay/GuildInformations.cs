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
	
	public class GuildInformations : Object
	{
		public const uint protocolId = 127;
		public String guildName = "";
		public GuildEmblem guildEmblem;
		
		public GuildInformations()
		{
			this.guildEmblem = new GuildEmblem();
		}
		
		public GuildInformations(String arg1, GuildEmblem arg2)
			: this()
		{
			initGuildInformations(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 127;
		}
		
		public GuildInformations initGuildInformations(String arg1 = "", GuildEmblem arg2 = null)
		{
			this.guildName = arg1;
			this.guildEmblem = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.guildName = "";
			this.guildEmblem = new GuildEmblem();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GuildInformations(arg1);
		}
		
		public void serializeAs_GuildInformations(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.guildName);
			this.guildEmblem.serializeAs_GuildEmblem(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildInformations(arg1);
		}
		
		public void deserializeAs_GuildInformations(BigEndianReader arg1)
		{
			this.guildName = (String)arg1.ReadUTF();
			this.guildEmblem = new GuildEmblem();
			this.guildEmblem.deserialize(arg1);
		}
		
	}
}
