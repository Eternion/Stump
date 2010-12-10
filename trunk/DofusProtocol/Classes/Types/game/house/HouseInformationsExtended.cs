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
	
	public class HouseInformationsExtended : HouseInformations
	{
		public const uint protocolId = 112;
		public String guildName = "";
		public GuildEmblem guildEmblem;
		
		public HouseInformationsExtended()
		{
			this.guildEmblem = new GuildEmblem();
		}
		
		public HouseInformationsExtended(uint arg1, List<uint> arg2, String arg3, Boolean arg4, uint arg5, String arg6, GuildEmblem arg7)
			: this()
		{
			initHouseInformationsExtended(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getTypeId()
		{
			return 112;
		}
		
		public HouseInformationsExtended initHouseInformationsExtended(uint arg1 = 0, List<uint> arg2 = null, String arg3 = "", Boolean arg4 = false, uint arg5 = 0, String arg6 = "", GuildEmblem arg7 = null)
		{
			base.initHouseInformations(arg1, arg2, arg3, arg4, arg5);
			this.guildName = arg6;
			this.guildEmblem = arg7;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.guildName = "";
			this.guildEmblem = new GuildEmblem();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_HouseInformationsExtended(arg1);
		}
		
		public void serializeAs_HouseInformationsExtended(BigEndianWriter arg1)
		{
			base.serializeAs_HouseInformations(arg1);
			arg1.WriteUTF((string)this.guildName);
			this.guildEmblem.serializeAs_GuildEmblem(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseInformationsExtended(arg1);
		}
		
		public void deserializeAs_HouseInformationsExtended(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.guildName = (String)arg1.ReadUTF();
			this.guildEmblem = new GuildEmblem();
			this.guildEmblem.deserialize(arg1);
		}
		
	}
}
