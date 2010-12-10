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
	
	public class ExchangePlayerRequestMessage : ExchangeRequestMessage
	{
		public const uint protocolId = 5773;
		internal Boolean _isInitialized = false;
		public uint target = 0;
		
		public ExchangePlayerRequestMessage()
		{
		}
		
		public ExchangePlayerRequestMessage(int arg1, uint arg2)
			: this()
		{
			initExchangePlayerRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5773;
		}
		
		public ExchangePlayerRequestMessage initExchangePlayerRequestMessage(int arg1 = 0, uint arg2 = 0)
		{
			base.initExchangeRequestMessage(arg1);
			this.target = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
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
			this.serializeAs_ExchangePlayerRequestMessage(arg1);
		}
		
		public void serializeAs_ExchangePlayerRequestMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ExchangeRequestMessage(arg1);
			if ( this.target < 0 )
			{
				throw new Exception("Forbidden value (" + this.target + ") on element target.");
			}
			arg1.WriteInt((int)this.target);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangePlayerRequestMessage(arg1);
		}
		
		public void deserializeAs_ExchangePlayerRequestMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.target = (uint)arg1.ReadInt();
			if ( this.target < 0 )
			{
				throw new Exception("Forbidden value (" + this.target + ") on element of ExchangePlayerRequestMessage.target.");
			}
		}
		
	}
}
