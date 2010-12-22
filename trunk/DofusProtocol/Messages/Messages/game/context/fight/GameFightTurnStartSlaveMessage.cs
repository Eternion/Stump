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
	
	public class GameFightTurnStartSlaveMessage : GameFightTurnStartMessage
	{
		public const uint protocolId = 6213;
		internal Boolean _isInitialized = false;
		public int idSummoner = 0;
		
		public GameFightTurnStartSlaveMessage()
		{
		}
		
		public GameFightTurnStartSlaveMessage(int arg1, uint arg2, int arg3)
			: this()
		{
			initGameFightTurnStartSlaveMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6213;
		}
		
		public GameFightTurnStartSlaveMessage initGameFightTurnStartSlaveMessage(int arg1 = 0, uint arg2 = 0, int arg3 = 0)
		{
			base.initGameFightTurnStartMessage(arg1, arg2);
			this.idSummoner = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.idSummoner = 0;
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameFightTurnStartSlaveMessage(arg1);
		}
		
		public void serializeAs_GameFightTurnStartSlaveMessage(BigEndianWriter arg1)
		{
			base.serializeAs_GameFightTurnStartMessage(arg1);
			arg1.WriteInt((int)this.idSummoner);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightTurnStartSlaveMessage(arg1);
		}
		
		public void deserializeAs_GameFightTurnStartSlaveMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.idSummoner = (int)arg1.ReadInt();
		}
		
	}
}