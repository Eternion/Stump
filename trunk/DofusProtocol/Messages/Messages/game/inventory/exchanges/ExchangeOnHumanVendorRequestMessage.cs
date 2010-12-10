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
	
	public class ExchangeOnHumanVendorRequestMessage : Message
	{
		public const uint protocolId = 5772;
		internal Boolean _isInitialized = false;
		public uint humanVendorId = 0;
		public uint humanVendorCell = 0;
		
		public ExchangeOnHumanVendorRequestMessage()
		{
		}
		
		public ExchangeOnHumanVendorRequestMessage(uint arg1, uint arg2)
			: this()
		{
			initExchangeOnHumanVendorRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5772;
		}
		
		public ExchangeOnHumanVendorRequestMessage initExchangeOnHumanVendorRequestMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.humanVendorId = arg1;
			this.humanVendorCell = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.humanVendorId = 0;
			this.humanVendorCell = 0;
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
			this.serializeAs_ExchangeOnHumanVendorRequestMessage(arg1);
		}
		
		public void serializeAs_ExchangeOnHumanVendorRequestMessage(BigEndianWriter arg1)
		{
			if ( this.humanVendorId < 0 )
			{
				throw new Exception("Forbidden value (" + this.humanVendorId + ") on element humanVendorId.");
			}
			arg1.WriteInt((int)this.humanVendorId);
			if ( this.humanVendorCell < 0 )
			{
				throw new Exception("Forbidden value (" + this.humanVendorCell + ") on element humanVendorCell.");
			}
			arg1.WriteInt((int)this.humanVendorCell);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeOnHumanVendorRequestMessage(arg1);
		}
		
		public void deserializeAs_ExchangeOnHumanVendorRequestMessage(BigEndianReader arg1)
		{
			this.humanVendorId = (uint)arg1.ReadInt();
			if ( this.humanVendorId < 0 )
			{
				throw new Exception("Forbidden value (" + this.humanVendorId + ") on element of ExchangeOnHumanVendorRequestMessage.humanVendorId.");
			}
			this.humanVendorCell = (uint)arg1.ReadInt();
			if ( this.humanVendorCell < 0 )
			{
				throw new Exception("Forbidden value (" + this.humanVendorCell + ") on element of ExchangeOnHumanVendorRequestMessage.humanVendorCell.");
			}
		}
		
	}
}
