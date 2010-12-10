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
	
	public class GuildPaddockRemovedMessage : Message
	{
		public const uint protocolId = 5955;
		internal Boolean _isInitialized = false;
		public int worldX = 0;
		public int worldY = 0;
		
		public GuildPaddockRemovedMessage()
		{
		}
		
		public GuildPaddockRemovedMessage(int arg1, int arg2)
			: this()
		{
			initGuildPaddockRemovedMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5955;
		}
		
		public GuildPaddockRemovedMessage initGuildPaddockRemovedMessage(int arg1 = 0, int arg2 = 0)
		{
			this.worldX = arg1;
			this.worldY = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.worldX = 0;
			this.worldY = 0;
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
			this.serializeAs_GuildPaddockRemovedMessage(arg1);
		}
		
		public void serializeAs_GuildPaddockRemovedMessage(BigEndianWriter arg1)
		{
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
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildPaddockRemovedMessage(arg1);
		}
		
		public void deserializeAs_GuildPaddockRemovedMessage(BigEndianReader arg1)
		{
			this.worldX = (int)arg1.ReadShort();
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element of GuildPaddockRemovedMessage.worldX.");
			}
			this.worldY = (int)arg1.ReadShort();
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element of GuildPaddockRemovedMessage.worldY.");
			}
		}
		
	}
}
