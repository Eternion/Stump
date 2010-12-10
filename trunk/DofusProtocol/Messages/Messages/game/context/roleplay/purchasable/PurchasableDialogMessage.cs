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
	
	public class PurchasableDialogMessage : Message
	{
		public const uint protocolId = 5739;
		internal Boolean _isInitialized = false;
		public Boolean buyOrSell = false;
		public uint purchasableId = 0;
		public uint price = 0;
		
		public PurchasableDialogMessage()
		{
		}
		
		public PurchasableDialogMessage(Boolean arg1, uint arg2, uint arg3)
			: this()
		{
			initPurchasableDialogMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5739;
		}
		
		public PurchasableDialogMessage initPurchasableDialogMessage(Boolean arg1 = false, uint arg2 = 0, uint arg3 = 0)
		{
			this.buyOrSell = arg1;
			this.purchasableId = arg2;
			this.price = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.buyOrSell = false;
			this.purchasableId = 0;
			this.price = 0;
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
			this.serializeAs_PurchasableDialogMessage(arg1);
		}
		
		public void serializeAs_PurchasableDialogMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.buyOrSell);
			if ( this.purchasableId < 0 )
			{
				throw new Exception("Forbidden value (" + this.purchasableId + ") on element purchasableId.");
			}
			arg1.WriteInt((int)this.purchasableId);
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element price.");
			}
			arg1.WriteInt((int)this.price);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PurchasableDialogMessage(arg1);
		}
		
		public void deserializeAs_PurchasableDialogMessage(BigEndianReader arg1)
		{
			this.buyOrSell = (Boolean)arg1.ReadBoolean();
			this.purchasableId = (uint)arg1.ReadInt();
			if ( this.purchasableId < 0 )
			{
				throw new Exception("Forbidden value (" + this.purchasableId + ") on element of PurchasableDialogMessage.purchasableId.");
			}
			this.price = (uint)arg1.ReadInt();
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element of PurchasableDialogMessage.price.");
			}
		}
		
	}
}
