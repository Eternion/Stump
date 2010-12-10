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
	
	public class ExchangeOkMultiCraftMessage : Message
	{
		public const uint protocolId = 5768;
		internal Boolean _isInitialized = false;
		public uint initiatorId = 0;
		public uint otherId = 0;
		public int role = 0;
		
		public ExchangeOkMultiCraftMessage()
		{
		}
		
		public ExchangeOkMultiCraftMessage(uint arg1, uint arg2, int arg3)
			: this()
		{
			initExchangeOkMultiCraftMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5768;
		}
		
		public ExchangeOkMultiCraftMessage initExchangeOkMultiCraftMessage(uint arg1 = 0, uint arg2 = 0, int arg3 = 0)
		{
			this.initiatorId = arg1;
			this.otherId = arg2;
			this.role = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.initiatorId = 0;
			this.otherId = 0;
			this.role = 0;
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
			this.serializeAs_ExchangeOkMultiCraftMessage(arg1);
		}
		
		public void serializeAs_ExchangeOkMultiCraftMessage(BigEndianWriter arg1)
		{
			if ( this.initiatorId < 0 )
			{
				throw new Exception("Forbidden value (" + this.initiatorId + ") on element initiatorId.");
			}
			arg1.WriteInt((int)this.initiatorId);
			if ( this.otherId < 0 )
			{
				throw new Exception("Forbidden value (" + this.otherId + ") on element otherId.");
			}
			arg1.WriteInt((int)this.otherId);
			arg1.WriteByte((byte)this.role);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeOkMultiCraftMessage(arg1);
		}
		
		public void deserializeAs_ExchangeOkMultiCraftMessage(BigEndianReader arg1)
		{
			this.initiatorId = (uint)arg1.ReadInt();
			if ( this.initiatorId < 0 )
			{
				throw new Exception("Forbidden value (" + this.initiatorId + ") on element of ExchangeOkMultiCraftMessage.initiatorId.");
			}
			this.otherId = (uint)arg1.ReadInt();
			if ( this.otherId < 0 )
			{
				throw new Exception("Forbidden value (" + this.otherId + ") on element of ExchangeOkMultiCraftMessage.otherId.");
			}
			this.role = (int)arg1.ReadByte();
		}
		
	}
}
