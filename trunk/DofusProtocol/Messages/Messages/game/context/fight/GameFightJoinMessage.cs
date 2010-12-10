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
	
	public class GameFightJoinMessage : Message
	{
		public const uint protocolId = 702;
		internal Boolean _isInitialized = false;
		public Boolean canBeCancelled = false;
		public Boolean canSayReady = false;
		public Boolean isSpectator = false;
		public Boolean isFightStarted = false;
		public uint timeMaxBeforeFightStart = 0;
		public uint fightType = 0;
		
		public GameFightJoinMessage()
		{
		}
		
		public GameFightJoinMessage(Boolean arg1, Boolean arg2, Boolean arg3, Boolean arg4, uint arg5, uint arg6)
			: this()
		{
			initGameFightJoinMessage(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getMessageId()
		{
			return 702;
		}
		
		public GameFightJoinMessage initGameFightJoinMessage(Boolean arg1 = false, Boolean arg2 = false, Boolean arg3 = false, Boolean arg4 = false, uint arg5 = 0, uint arg6 = 0)
		{
			this.canBeCancelled = arg1;
			this.canSayReady = arg2;
			this.isSpectator = arg3;
			this.isFightStarted = arg4;
			this.timeMaxBeforeFightStart = arg5;
			this.fightType = arg6;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.canBeCancelled = false;
			this.canSayReady = false;
			this.isSpectator = false;
			this.isFightStarted = false;
			this.timeMaxBeforeFightStart = 0;
			this.fightType = 0;
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
			this.serializeAs_GameFightJoinMessage(arg1);
		}
		
		public void serializeAs_GameFightJoinMessage(BigEndianWriter arg1)
		{
			var loc1 = 0;
			BooleanByteWrapper.SetFlag(loc1, 0, this.canBeCancelled);
			BooleanByteWrapper.SetFlag(loc1, 1, this.canSayReady);
			BooleanByteWrapper.SetFlag(loc1, 2, this.isSpectator);
			BooleanByteWrapper.SetFlag(loc1, 3, this.isFightStarted);
			arg1.WriteByte((byte)loc1);
			if ( this.timeMaxBeforeFightStart < 0 )
			{
				throw new Exception("Forbidden value (" + this.timeMaxBeforeFightStart + ") on element timeMaxBeforeFightStart.");
			}
			arg1.WriteInt((int)this.timeMaxBeforeFightStart);
			arg1.WriteByte((byte)this.fightType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightJoinMessage(arg1);
		}
		
		public void deserializeAs_GameFightJoinMessage(BigEndianReader arg1)
		{
			var loc1 = arg1.ReadByte();
			this.canBeCancelled = (Boolean)BooleanByteWrapper.GetFlag(loc1, 0);
			this.canSayReady = (Boolean)BooleanByteWrapper.GetFlag(loc1, 1);
			this.isSpectator = (Boolean)BooleanByteWrapper.GetFlag(loc1, 2);
			this.isFightStarted = (Boolean)BooleanByteWrapper.GetFlag(loc1, 3);
			this.timeMaxBeforeFightStart = (uint)arg1.ReadInt();
			if ( this.timeMaxBeforeFightStart < 0 )
			{
				throw new Exception("Forbidden value (" + this.timeMaxBeforeFightStart + ") on element of GameFightJoinMessage.timeMaxBeforeFightStart.");
			}
			this.fightType = (uint)arg1.ReadByte();
			if ( this.fightType < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightType + ") on element of GameFightJoinMessage.fightType.");
			}
		}
		
	}
}
