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
	
	public class GameFightOptionStateUpdateMessage : Message
	{
		public const uint protocolId = 5927;
		internal Boolean _isInitialized = false;
		public uint fightId = 0;
		public uint teamId = 2;
		public uint option = 3;
		public Boolean state = false;
		
		public GameFightOptionStateUpdateMessage()
		{
		}
		
		public GameFightOptionStateUpdateMessage(uint arg1, uint arg2, uint arg3, Boolean arg4)
			: this()
		{
			initGameFightOptionStateUpdateMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5927;
		}
		
		public GameFightOptionStateUpdateMessage initGameFightOptionStateUpdateMessage(uint arg1 = 0, uint arg2 = 2, uint arg3 = 3, Boolean arg4 = false)
		{
			this.fightId = arg1;
			this.teamId = arg2;
			this.option = arg3;
			this.state = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.teamId = 2;
			this.option = 3;
			this.state = false;
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
			this.serializeAs_GameFightOptionStateUpdateMessage(arg1);
		}
		
		public void serializeAs_GameFightOptionStateUpdateMessage(BigEndianWriter arg1)
		{
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element fightId.");
			}
			arg1.WriteShort((short)this.fightId);
			arg1.WriteByte((byte)this.teamId);
			arg1.WriteByte((byte)this.option);
			arg1.WriteBoolean(this.state);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightOptionStateUpdateMessage(arg1);
		}
		
		public void deserializeAs_GameFightOptionStateUpdateMessage(BigEndianReader arg1)
		{
			this.fightId = (uint)arg1.ReadShort();
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element of GameFightOptionStateUpdateMessage.fightId.");
			}
			this.teamId = (uint)arg1.ReadByte();
			if ( this.teamId < 0 )
			{
				throw new Exception("Forbidden value (" + this.teamId + ") on element of GameFightOptionStateUpdateMessage.teamId.");
			}
			this.option = (uint)arg1.ReadByte();
			if ( this.option < 0 )
			{
				throw new Exception("Forbidden value (" + this.option + ") on element of GameFightOptionStateUpdateMessage.option.");
			}
			this.state = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
