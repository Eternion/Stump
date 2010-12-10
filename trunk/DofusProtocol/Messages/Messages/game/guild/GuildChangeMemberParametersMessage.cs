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
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildChangeMemberParametersMessage : Message
	{
		public const uint protocolId = 5549;
		internal Boolean _isInitialized = false;
		public uint memberId = 0;
		public uint rank = 0;
		public uint experienceGivenPercent = 0;
		public uint rights = 0;
		
		public GuildChangeMemberParametersMessage()
		{
		}
		
		public GuildChangeMemberParametersMessage(uint arg1, uint arg2, uint arg3, uint arg4)
			: this()
		{
			initGuildChangeMemberParametersMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5549;
		}
		
		public GuildChangeMemberParametersMessage initGuildChangeMemberParametersMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0)
		{
			this.memberId = arg1;
			this.rank = arg2;
			this.experienceGivenPercent = arg3;
			this.rights = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.memberId = 0;
			this.rank = 0;
			this.experienceGivenPercent = 0;
			this.rights = 0;
			this._isInitialized = false;
		}
		
		public override void pack(BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack(BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GuildChangeMemberParametersMessage(arg1);
		}
		
		public void serializeAs_GuildChangeMemberParametersMessage(BigEndianWriter arg1)
		{
			if ( this.memberId < 0 )
			{
				throw new Exception("Forbidden value (" + this.memberId + ") on element memberId.");
			}
			arg1.WriteInt((int)this.memberId);
			if ( this.rank < 0 )
			{
				throw new Exception("Forbidden value (" + this.rank + ") on element rank.");
			}
			arg1.WriteShort((short)this.rank);
			if ( this.experienceGivenPercent < 0 || this.experienceGivenPercent > 100 )
			{
				throw new Exception("Forbidden value (" + this.experienceGivenPercent + ") on element experienceGivenPercent.");
			}
			arg1.WriteByte((byte)this.experienceGivenPercent);
			if ( this.rights < 0 || this.rights > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.rights + ") on element rights.");
			}
			arg1.WriteUInt((uint)this.rights);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildChangeMemberParametersMessage(arg1);
		}
		
		public void deserializeAs_GuildChangeMemberParametersMessage(BigEndianReader arg1)
		{
			this.memberId = (uint)arg1.ReadInt();
			if ( this.memberId < 0 )
			{
				throw new Exception("Forbidden value (" + this.memberId + ") on element of GuildChangeMemberParametersMessage.memberId.");
			}
			this.rank = (uint)arg1.ReadShort();
			if ( this.rank < 0 )
			{
				throw new Exception("Forbidden value (" + this.rank + ") on element of GuildChangeMemberParametersMessage.rank.");
			}
			this.experienceGivenPercent = (uint)arg1.ReadByte();
			if ( this.experienceGivenPercent < 0 || this.experienceGivenPercent > 100 )
			{
				throw new Exception("Forbidden value (" + this.experienceGivenPercent + ") on element of GuildChangeMemberParametersMessage.experienceGivenPercent.");
			}
			this.rights = (uint)arg1.ReadUInt();
			if ( this.rights < 0 || this.rights > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.rights + ") on element of GuildChangeMemberParametersMessage.rights.");
			}
		}
		
	}
}
