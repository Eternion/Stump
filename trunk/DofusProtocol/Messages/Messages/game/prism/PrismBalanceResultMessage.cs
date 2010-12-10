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
	
	public class PrismBalanceResultMessage : Message
	{
		public const uint protocolId = 5841;
		internal Boolean _isInitialized = false;
		public uint totalBalanceValue = 0;
		public uint subAreaBalanceValue = 0;
		
		public PrismBalanceResultMessage()
		{
		}
		
		public PrismBalanceResultMessage(uint arg1, uint arg2)
			: this()
		{
			initPrismBalanceResultMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5841;
		}
		
		public PrismBalanceResultMessage initPrismBalanceResultMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.totalBalanceValue = arg1;
			this.subAreaBalanceValue = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.totalBalanceValue = 0;
			this.subAreaBalanceValue = 0;
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
			this.serializeAs_PrismBalanceResultMessage(arg1);
		}
		
		public void serializeAs_PrismBalanceResultMessage(BigEndianWriter arg1)
		{
			if ( this.totalBalanceValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.totalBalanceValue + ") on element totalBalanceValue.");
			}
			arg1.WriteByte((byte)this.totalBalanceValue);
			if ( this.subAreaBalanceValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.subAreaBalanceValue + ") on element subAreaBalanceValue.");
			}
			arg1.WriteByte((byte)this.subAreaBalanceValue);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismBalanceResultMessage(arg1);
		}
		
		public void deserializeAs_PrismBalanceResultMessage(BigEndianReader arg1)
		{
			this.totalBalanceValue = (uint)arg1.ReadByte();
			if ( this.totalBalanceValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.totalBalanceValue + ") on element of PrismBalanceResultMessage.totalBalanceValue.");
			}
			this.subAreaBalanceValue = (uint)arg1.ReadByte();
			if ( this.subAreaBalanceValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.subAreaBalanceValue + ") on element of PrismBalanceResultMessage.subAreaBalanceValue.");
			}
		}
		
	}
}
