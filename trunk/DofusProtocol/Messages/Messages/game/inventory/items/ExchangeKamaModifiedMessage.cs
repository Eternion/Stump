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
	
	public class ExchangeKamaModifiedMessage : ExchangeObjectMessage
	{
		public const uint protocolId = 5521;
		internal Boolean _isInitialized = false;
		public uint quantity = 0;
		
		public ExchangeKamaModifiedMessage()
		{
		}
		
		public ExchangeKamaModifiedMessage(Boolean arg1, uint arg2)
			: this()
		{
			initExchangeKamaModifiedMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5521;
		}
		
		public ExchangeKamaModifiedMessage initExchangeKamaModifiedMessage(Boolean arg1 = false, uint arg2 = 0)
		{
			base.initExchangeObjectMessage(arg1);
			this.quantity = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.quantity = 0;
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
			this.serializeAs_ExchangeKamaModifiedMessage(arg1);
		}
		
		public void serializeAs_ExchangeKamaModifiedMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ExchangeObjectMessage(arg1);
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element quantity.");
			}
			arg1.WriteInt((int)this.quantity);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeKamaModifiedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeKamaModifiedMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.quantity = (uint)arg1.ReadInt();
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element of ExchangeKamaModifiedMessage.quantity.");
			}
		}
		
	}
}
