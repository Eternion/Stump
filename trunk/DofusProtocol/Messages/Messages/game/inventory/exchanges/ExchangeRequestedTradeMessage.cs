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
	
	public class ExchangeRequestedTradeMessage : ExchangeRequestedMessage
	{
		public const uint protocolId = 5523;
		internal Boolean _isInitialized = false;
		public uint source = 0;
		public uint target = 0;
		
		public ExchangeRequestedTradeMessage()
		{
		}
		
		public ExchangeRequestedTradeMessage(int arg1, uint arg2, uint arg3)
			: this()
		{
			initExchangeRequestedTradeMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5523;
		}
		
		public ExchangeRequestedTradeMessage initExchangeRequestedTradeMessage(int arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			base.initExchangeRequestedMessage(arg1);
			this.source = arg2;
			this.target = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.source = 0;
			this.target = 0;
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
			this.serializeAs_ExchangeRequestedTradeMessage(arg1);
		}
		
		public void serializeAs_ExchangeRequestedTradeMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ExchangeRequestedMessage(arg1);
			if ( this.source < 0 )
			{
				throw new Exception("Forbidden value (" + this.source + ") on element source.");
			}
			arg1.WriteInt((int)this.source);
			if ( this.target < 0 )
			{
				throw new Exception("Forbidden value (" + this.target + ") on element target.");
			}
			arg1.WriteInt((int)this.target);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeRequestedTradeMessage(arg1);
		}
		
		public void deserializeAs_ExchangeRequestedTradeMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.source = (uint)arg1.ReadInt();
			if ( this.source < 0 )
			{
				throw new Exception("Forbidden value (" + this.source + ") on element of ExchangeRequestedTradeMessage.source.");
			}
			this.target = (uint)arg1.ReadInt();
			if ( this.target < 0 )
			{
				throw new Exception("Forbidden value (" + this.target + ") on element of ExchangeRequestedTradeMessage.target.");
			}
		}
		
	}
}
