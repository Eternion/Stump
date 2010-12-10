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
	
	public class PaddockPrivateInformations : PaddockAbandonnedInformations
	{
		public const uint protocolId = 131;
		public String guildName = "";
		public GuildEmblem guildEmblem;
		
		public PaddockPrivateInformations()
		{
			this.guildEmblem = new GuildEmblem();
		}
		
		public PaddockPrivateInformations(uint arg1, uint arg2, uint arg3, uint arg4, String arg5, GuildEmblem arg6)
			: this()
		{
			initPaddockPrivateInformations(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getTypeId()
		{
			return 131;
		}
		
		public PaddockPrivateInformations initPaddockPrivateInformations(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0, String arg5 = "", GuildEmblem arg6 = null)
		{
			base.initPaddockAbandonnedInformations(arg1, arg2, arg3, arg4);
			this.guildName = arg5;
			this.guildEmblem = arg6;
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
			this.serializeAs_PaddockPrivateInformations(arg1);
		}
		
		public void serializeAs_PaddockPrivateInformations(BigEndianWriter arg1)
		{
			base.serializeAs_PaddockAbandonnedInformations(arg1);
			arg1.WriteUTF((string)this.guildName);
			this.guildEmblem.serializeAs_GuildEmblem(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockPrivateInformations(arg1);
		}
		
		public void deserializeAs_PaddockPrivateInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.guildName = (String)arg1.ReadUTF();
			this.guildEmblem = new GuildEmblem();
			this.guildEmblem.deserialize(arg1);
		}
		
	}
}
