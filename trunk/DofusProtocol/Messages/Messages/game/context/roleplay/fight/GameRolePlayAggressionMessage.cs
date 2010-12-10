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
	
	public class GameRolePlayAggressionMessage : Message
	{
		public const uint protocolId = 6073;
		internal Boolean _isInitialized = false;
		public uint attackerId = 0;
		public uint defenderId = 0;
		
		public GameRolePlayAggressionMessage()
		{
		}
		
		public GameRolePlayAggressionMessage(uint arg1, uint arg2)
			: this()
		{
			initGameRolePlayAggressionMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6073;
		}
		
		public GameRolePlayAggressionMessage initGameRolePlayAggressionMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.attackerId = arg1;
			this.defenderId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.attackerId = 0;
			this.defenderId = 0;
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
			this.serializeAs_GameRolePlayAggressionMessage(arg1);
		}
		
		public void serializeAs_GameRolePlayAggressionMessage(BigEndianWriter arg1)
		{
			if ( this.attackerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.attackerId + ") on element attackerId.");
			}
			arg1.WriteInt((int)this.attackerId);
			if ( this.defenderId < 0 )
			{
				throw new Exception("Forbidden value (" + this.defenderId + ") on element defenderId.");
			}
			arg1.WriteInt((int)this.defenderId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayAggressionMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlayAggressionMessage(BigEndianReader arg1)
		{
			this.attackerId = (uint)arg1.ReadInt();
			if ( this.attackerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.attackerId + ") on element of GameRolePlayAggressionMessage.attackerId.");
			}
			this.defenderId = (uint)arg1.ReadInt();
			if ( this.defenderId < 0 )
			{
				throw new Exception("Forbidden value (" + this.defenderId + ") on element of GameRolePlayAggressionMessage.defenderId.");
			}
		}
		
	}
}
