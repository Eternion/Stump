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
	
	public class ExchangeGoldPaymentForCraftMessage : Message
	{
		public const uint protocolId = 5833;
		internal Boolean _isInitialized = false;
		public Boolean onlySuccess = false;
		public uint goldSum = 0;
		
		public ExchangeGoldPaymentForCraftMessage()
		{
		}
		
		public ExchangeGoldPaymentForCraftMessage(Boolean arg1, uint arg2)
			: this()
		{
			initExchangeGoldPaymentForCraftMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5833;
		}
		
		public ExchangeGoldPaymentForCraftMessage initExchangeGoldPaymentForCraftMessage(Boolean arg1 = false, uint arg2 = 0)
		{
			this.onlySuccess = arg1;
			this.goldSum = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.onlySuccess = false;
			this.goldSum = 0;
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
			this.serializeAs_ExchangeGoldPaymentForCraftMessage(arg1);
		}
		
		public void serializeAs_ExchangeGoldPaymentForCraftMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.onlySuccess);
			if ( this.goldSum < 0 )
			{
				throw new Exception("Forbidden value (" + this.goldSum + ") on element goldSum.");
			}
			arg1.WriteInt((int)this.goldSum);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeGoldPaymentForCraftMessage(arg1);
		}
		
		public void deserializeAs_ExchangeGoldPaymentForCraftMessage(BigEndianReader arg1)
		{
			this.onlySuccess = (Boolean)arg1.ReadBoolean();
			this.goldSum = (uint)arg1.ReadInt();
			if ( this.goldSum < 0 )
			{
				throw new Exception("Forbidden value (" + this.goldSum + ") on element of ExchangeGoldPaymentForCraftMessage.goldSum.");
			}
		}
		
	}
}
