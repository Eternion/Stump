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
	
	public class PartyUpdateLightMessage : Message
	{
		public const uint protocolId = 6054;
		internal Boolean _isInitialized = false;
		public uint id = 0;
		public uint lifePoints = 0;
		public uint maxLifePoints = 0;
		public uint prospecting = 0;
		public uint regenRate = 0;
		
		public PartyUpdateLightMessage()
		{
		}
		
		public PartyUpdateLightMessage(uint arg1, uint arg2, uint arg3, uint arg4, uint arg5)
			: this()
		{
			initPartyUpdateLightMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 6054;
		}
		
		public PartyUpdateLightMessage initPartyUpdateLightMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0, uint arg5 = 0)
		{
			this.id = arg1;
			this.lifePoints = arg2;
			this.maxLifePoints = arg3;
			this.prospecting = arg4;
			this.regenRate = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.id = 0;
			this.lifePoints = 0;
			this.maxLifePoints = 0;
			this.prospecting = 0;
			this.regenRate = 0;
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
			this.serializeAs_PartyUpdateLightMessage(arg1);
		}
		
		public void serializeAs_PartyUpdateLightMessage(BigEndianWriter arg1)
		{
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element id.");
			}
			arg1.WriteInt((int)this.id);
			if ( this.lifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.lifePoints + ") on element lifePoints.");
			}
			arg1.WriteInt((int)this.lifePoints);
			if ( this.maxLifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxLifePoints + ") on element maxLifePoints.");
			}
			arg1.WriteInt((int)this.maxLifePoints);
			if ( this.prospecting < 0 )
			{
				throw new Exception("Forbidden value (" + this.prospecting + ") on element prospecting.");
			}
			arg1.WriteShort((short)this.prospecting);
			if ( this.regenRate < 0 || this.regenRate > 255 )
			{
				throw new Exception("Forbidden value (" + this.regenRate + ") on element regenRate.");
			}
			arg1.WriteByte((byte)this.regenRate);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartyUpdateLightMessage(arg1);
		}
		
		public void deserializeAs_PartyUpdateLightMessage(BigEndianReader arg1)
		{
			this.id = (uint)arg1.ReadInt();
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element of PartyUpdateLightMessage.id.");
			}
			this.lifePoints = (uint)arg1.ReadInt();
			if ( this.lifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.lifePoints + ") on element of PartyUpdateLightMessage.lifePoints.");
			}
			this.maxLifePoints = (uint)arg1.ReadInt();
			if ( this.maxLifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxLifePoints + ") on element of PartyUpdateLightMessage.maxLifePoints.");
			}
			this.prospecting = (uint)arg1.ReadShort();
			if ( this.prospecting < 0 )
			{
				throw new Exception("Forbidden value (" + this.prospecting + ") on element of PartyUpdateLightMessage.prospecting.");
			}
			this.regenRate = (uint)arg1.ReadByte();
			if ( this.regenRate < 0 || this.regenRate > 255 )
			{
				throw new Exception("Forbidden value (" + this.regenRate + ") on element of PartyUpdateLightMessage.regenRate.");
			}
		}
		
	}
}
