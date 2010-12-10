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
	
	public class ExchangeReplyTaxVendorMessage : Message
	{
		public const uint protocolId = 5787;
		internal Boolean _isInitialized = false;
		public uint objectValue = 0;
		public uint totalTaxValue = 0;
		
		public ExchangeReplyTaxVendorMessage()
		{
		}
		
		public ExchangeReplyTaxVendorMessage(uint arg1, uint arg2)
			: this()
		{
			initExchangeReplyTaxVendorMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5787;
		}
		
		public ExchangeReplyTaxVendorMessage initExchangeReplyTaxVendorMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.@objectValue = arg1;
			this.totalTaxValue = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectValue = 0;
			this.totalTaxValue = 0;
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
			this.serializeAs_ExchangeReplyTaxVendorMessage(arg1);
		}
		
		public void serializeAs_ExchangeReplyTaxVendorMessage(BigEndianWriter arg1)
		{
			if ( this.@objectValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectValue + ") on element objectValue.");
			}
			arg1.WriteInt((int)this.@objectValue);
			if ( this.totalTaxValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.totalTaxValue + ") on element totalTaxValue.");
			}
			arg1.WriteInt((int)this.totalTaxValue);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeReplyTaxVendorMessage(arg1);
		}
		
		public void deserializeAs_ExchangeReplyTaxVendorMessage(BigEndianReader arg1)
		{
			this.@objectValue = (uint)arg1.ReadInt();
			if ( this.@objectValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectValue + ") on element of ExchangeReplyTaxVendorMessage.objectValue.");
			}
			this.totalTaxValue = (uint)arg1.ReadInt();
			if ( this.totalTaxValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.totalTaxValue + ") on element of ExchangeReplyTaxVendorMessage.totalTaxValue.");
			}
		}
		
	}
}
