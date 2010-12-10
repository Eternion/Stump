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
	
	public class ExchangeHandleMountStableMessage : Message
	{
		public const uint protocolId = 5965;
		internal Boolean _isInitialized = false;
		public int actionType = 0;
		public uint rideId = 0;
		
		public ExchangeHandleMountStableMessage()
		{
		}
		
		public ExchangeHandleMountStableMessage(int arg1, uint arg2)
			: this()
		{
			initExchangeHandleMountStableMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5965;
		}
		
		public ExchangeHandleMountStableMessage initExchangeHandleMountStableMessage(int arg1 = 0, uint arg2 = 0)
		{
			this.actionType = arg1;
			this.rideId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.actionType = 0;
			this.rideId = 0;
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
			this.serializeAs_ExchangeHandleMountStableMessage(arg1);
		}
		
		public void serializeAs_ExchangeHandleMountStableMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.actionType);
			if ( this.rideId < 0 )
			{
				throw new Exception("Forbidden value (" + this.rideId + ") on element rideId.");
			}
			arg1.WriteInt((int)this.rideId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeHandleMountStableMessage(arg1);
		}
		
		public void deserializeAs_ExchangeHandleMountStableMessage(BigEndianReader arg1)
		{
			this.actionType = (int)arg1.ReadByte();
			this.rideId = (uint)arg1.ReadInt();
			if ( this.rideId < 0 )
			{
				throw new Exception("Forbidden value (" + this.rideId + ") on element of ExchangeHandleMountStableMessage.rideId.");
			}
		}
		
	}
}
