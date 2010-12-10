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
	
	public class ExchangeMountFreeFromPaddockMessage : Message
	{
		public const uint protocolId = 6055;
		internal Boolean _isInitialized = false;
		public String name = "";
		public int worldX = 0;
		public int worldY = 0;
		public String liberator = "";
		
		public ExchangeMountFreeFromPaddockMessage()
		{
		}
		
		public ExchangeMountFreeFromPaddockMessage(String arg1, int arg2, int arg3, String arg4)
			: this()
		{
			initExchangeMountFreeFromPaddockMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 6055;
		}
		
		public ExchangeMountFreeFromPaddockMessage initExchangeMountFreeFromPaddockMessage(String arg1 = "", int arg2 = 0, int arg3 = 0, String arg4 = "")
		{
			this.name = arg1;
			this.worldX = arg2;
			this.worldY = arg3;
			this.liberator = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.name = "";
			this.worldX = 0;
			this.worldY = 0;
			this.liberator = "";
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
			this.serializeAs_ExchangeMountFreeFromPaddockMessage(arg1);
		}
		
		public void serializeAs_ExchangeMountFreeFromPaddockMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.name);
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element worldX.");
			}
			arg1.WriteShort((short)this.worldX);
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element worldY.");
			}
			arg1.WriteShort((short)this.worldY);
			arg1.WriteUTF((string)this.liberator);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeMountFreeFromPaddockMessage(arg1);
		}
		
		public void deserializeAs_ExchangeMountFreeFromPaddockMessage(BigEndianReader arg1)
		{
			this.name = (String)arg1.ReadUTF();
			this.worldX = (int)arg1.ReadShort();
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element of ExchangeMountFreeFromPaddockMessage.worldX.");
			}
			this.worldY = (int)arg1.ReadShort();
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element of ExchangeMountFreeFromPaddockMessage.worldY.");
			}
			this.liberator = (String)arg1.ReadUTF();
		}
		
	}
}
