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
	
	public class GameRolePlayTaxCollectorInformations : GameRolePlayActorInformations
	{
		public const uint protocolId = 148;
		public uint firstNameId = 0;
		public uint lastNameId = 0;
		public GuildInformations guildIdentity;
		public uint guildLevel = 0;
		
		public GameRolePlayTaxCollectorInformations()
		{
			this.guildIdentity = new GuildInformations();
		}
		
		public GameRolePlayTaxCollectorInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, uint arg4, uint arg5, GuildInformations arg6, uint arg7)
			: this()
		{
			initGameRolePlayTaxCollectorInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getTypeId()
		{
			return 148;
		}
		
		public GameRolePlayTaxCollectorInformations initGameRolePlayTaxCollectorInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, uint arg4 = 0, uint arg5 = 0, GuildInformations arg6 = null, uint arg7 = 0)
		{
			base.initGameRolePlayActorInformations(arg1, arg2, arg3);
			this.firstNameId = arg4;
			this.lastNameId = arg5;
			this.guildIdentity = arg6;
			this.guildLevel = arg7;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.firstNameId = 0;
			this.lastNameId = 0;
			this.guildIdentity = new GuildInformations();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameRolePlayTaxCollectorInformations(arg1);
		}
		
		public void serializeAs_GameRolePlayTaxCollectorInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameRolePlayActorInformations(arg1);
			if ( this.firstNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstNameId + ") on element firstNameId.");
			}
			arg1.WriteShort((short)this.firstNameId);
			if ( this.lastNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastNameId + ") on element lastNameId.");
			}
			arg1.WriteShort((short)this.lastNameId);
			this.guildIdentity.serializeAs_GuildInformations(arg1);
			if ( this.guildLevel < 0 || this.guildLevel > 255 )
			{
				throw new Exception("Forbidden value (" + this.guildLevel + ") on element guildLevel.");
			}
			arg1.WriteByte((byte)this.guildLevel);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayTaxCollectorInformations(arg1);
		}
		
		public void deserializeAs_GameRolePlayTaxCollectorInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.firstNameId = (uint)arg1.ReadShort();
			if ( this.firstNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstNameId + ") on element of GameRolePlayTaxCollectorInformations.firstNameId.");
			}
			this.lastNameId = (uint)arg1.ReadShort();
			if ( this.lastNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastNameId + ") on element of GameRolePlayTaxCollectorInformations.lastNameId.");
			}
			this.guildIdentity = new GuildInformations();
			this.guildIdentity.deserialize(arg1);
			this.guildLevel = (uint)arg1.ReadByte();
			if ( this.guildLevel < 0 || this.guildLevel > 255 )
			{
				throw new Exception("Forbidden value (" + this.guildLevel + ") on element of GameRolePlayTaxCollectorInformations.guildLevel.");
			}
		}
		
	}
}
