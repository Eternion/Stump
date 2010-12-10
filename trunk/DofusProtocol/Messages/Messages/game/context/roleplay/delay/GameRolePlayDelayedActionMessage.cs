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
	
	public class GameRolePlayDelayedActionMessage : Message
	{
		public const uint protocolId = 6153;
		internal Boolean _isInitialized = false;
		public uint delayTypeId = 0;
		public uint delayDuration = 0;
		
		public GameRolePlayDelayedActionMessage()
		{
		}
		
		public GameRolePlayDelayedActionMessage(uint arg1, uint arg2)
			: this()
		{
			initGameRolePlayDelayedActionMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6153;
		}
		
		public GameRolePlayDelayedActionMessage initGameRolePlayDelayedActionMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.delayTypeId = arg1;
			this.delayDuration = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.delayTypeId = 0;
			this.delayDuration = 0;
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
			this.serializeAs_GameRolePlayDelayedActionMessage(arg1);
		}
		
		public void serializeAs_GameRolePlayDelayedActionMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.delayTypeId);
			if ( this.delayDuration < 0 )
			{
				throw new Exception("Forbidden value (" + this.delayDuration + ") on element delayDuration.");
			}
			arg1.WriteInt((int)this.delayDuration);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayDelayedActionMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlayDelayedActionMessage(BigEndianReader arg1)
		{
			this.delayTypeId = (uint)arg1.ReadByte();
			if ( this.delayTypeId < 0 )
			{
				throw new Exception("Forbidden value (" + this.delayTypeId + ") on element of GameRolePlayDelayedActionMessage.delayTypeId.");
			}
			this.delayDuration = (uint)arg1.ReadInt();
			if ( this.delayDuration < 0 )
			{
				throw new Exception("Forbidden value (" + this.delayDuration + ") on element of GameRolePlayDelayedActionMessage.delayDuration.");
			}
		}
		
	}
}
