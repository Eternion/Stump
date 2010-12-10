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
	
	public class ExchangeStartOkMulticraftCustomerMessage : Message
	{
		public const uint protocolId = 5817;
		internal Boolean _isInitialized = false;
		public uint maxCase = 0;
		public uint skillId = 0;
		public uint crafterJobLevel = 0;
		
		public ExchangeStartOkMulticraftCustomerMessage()
		{
		}
		
		public ExchangeStartOkMulticraftCustomerMessage(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initExchangeStartOkMulticraftCustomerMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5817;
		}
		
		public ExchangeStartOkMulticraftCustomerMessage initExchangeStartOkMulticraftCustomerMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			this.maxCase = arg1;
			this.skillId = arg2;
			this.crafterJobLevel = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.maxCase = 0;
			this.skillId = 0;
			this.crafterJobLevel = 0;
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
			this.serializeAs_ExchangeStartOkMulticraftCustomerMessage(arg1);
		}
		
		public void serializeAs_ExchangeStartOkMulticraftCustomerMessage(BigEndianWriter arg1)
		{
			if ( this.maxCase < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxCase + ") on element maxCase.");
			}
			arg1.WriteByte((byte)this.maxCase);
			if ( this.skillId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillId + ") on element skillId.");
			}
			arg1.WriteInt((int)this.skillId);
			if ( this.crafterJobLevel < 0 )
			{
				throw new Exception("Forbidden value (" + this.crafterJobLevel + ") on element crafterJobLevel.");
			}
			arg1.WriteByte((byte)this.crafterJobLevel);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeStartOkMulticraftCustomerMessage(arg1);
		}
		
		public void deserializeAs_ExchangeStartOkMulticraftCustomerMessage(BigEndianReader arg1)
		{
			this.maxCase = (uint)arg1.ReadByte();
			if ( this.maxCase < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxCase + ") on element of ExchangeStartOkMulticraftCustomerMessage.maxCase.");
			}
			this.skillId = (uint)arg1.ReadInt();
			if ( this.skillId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillId + ") on element of ExchangeStartOkMulticraftCustomerMessage.skillId.");
			}
			this.crafterJobLevel = (uint)arg1.ReadByte();
			if ( this.crafterJobLevel < 0 )
			{
				throw new Exception("Forbidden value (" + this.crafterJobLevel + ") on element of ExchangeStartOkMulticraftCustomerMessage.crafterJobLevel.");
			}
		}
		
	}
}
