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
	
	public class HumanWithGuildInformations : HumanInformations
	{
		public const uint protocolId = 153;
		public GuildInformations guildInformations;
		
		public HumanWithGuildInformations()
		{
			this.guildInformations = new GuildInformations();
		}
		
		public HumanWithGuildInformations(List<EntityLook> arg1, int arg2, uint arg3, ActorRestrictionsInformations arg4, uint arg5, String arg6, GuildInformations arg7)
			: this()
		{
			initHumanWithGuildInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getTypeId()
		{
			return 153;
		}
		
		public HumanWithGuildInformations initHumanWithGuildInformations(List<EntityLook> arg1, int arg2 = 0, uint arg3 = 0, ActorRestrictionsInformations arg4 = null, uint arg5 = 0, String arg6 = "", GuildInformations arg7 = null)
		{
			base.initHumanInformations(arg1, arg2, arg3, arg4, arg5, arg6);
			this.guildInformations = arg7;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.guildInformations = new GuildInformations();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_HumanWithGuildInformations(arg1);
		}
		
		public void serializeAs_HumanWithGuildInformations(BigEndianWriter arg1)
		{
			base.serializeAs_HumanInformations(arg1);
			this.guildInformations.serializeAs_GuildInformations(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HumanWithGuildInformations(arg1);
		}
		
		public void deserializeAs_HumanWithGuildInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.guildInformations = new GuildInformations();
			this.guildInformations.deserialize(arg1);
		}
		
	}
}
