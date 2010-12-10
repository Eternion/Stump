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
	
	public class ExchangeStartOkCraftWithInformationMessage : ExchangeStartOkCraftMessage
	{
		public const uint protocolId = 5941;
		internal Boolean _isInitialized = false;
		public uint nbCase = 0;
		public uint skillId = 0;
		
		public ExchangeStartOkCraftWithInformationMessage()
		{
		}
		
		public ExchangeStartOkCraftWithInformationMessage(uint arg1, uint arg2)
			: this()
		{
			initExchangeStartOkCraftWithInformationMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5941;
		}
		
		public ExchangeStartOkCraftWithInformationMessage initExchangeStartOkCraftWithInformationMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.nbCase = arg1;
			this.skillId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.nbCase = 0;
			this.skillId = 0;
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
			this.serializeAs_ExchangeStartOkCraftWithInformationMessage(arg1);
		}
		
		public void serializeAs_ExchangeStartOkCraftWithInformationMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ExchangeStartOkCraftMessage(arg1);
			if ( this.nbCase < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbCase + ") on element nbCase.");
			}
			arg1.WriteByte((byte)this.nbCase);
			if ( this.skillId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillId + ") on element skillId.");
			}
			arg1.WriteInt((int)this.skillId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeStartOkCraftWithInformationMessage(arg1);
		}
		
		public void deserializeAs_ExchangeStartOkCraftWithInformationMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.nbCase = (uint)arg1.ReadByte();
			if ( this.nbCase < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbCase + ") on element of ExchangeStartOkCraftWithInformationMessage.nbCase.");
			}
			this.skillId = (uint)arg1.ReadInt();
			if ( this.skillId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillId + ") on element of ExchangeStartOkCraftWithInformationMessage.skillId.");
			}
		}
		
	}
}
