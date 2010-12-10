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
	
	public class ChannelEnablingChangeMessage : Message
	{
		public const uint protocolId = 891;
		internal Boolean _isInitialized = false;
		public uint channel = 0;
		public Boolean enable = false;
		
		public ChannelEnablingChangeMessage()
		{
		}
		
		public ChannelEnablingChangeMessage(uint arg1, Boolean arg2)
			: this()
		{
			initChannelEnablingChangeMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 891;
		}
		
		public ChannelEnablingChangeMessage initChannelEnablingChangeMessage(uint arg1 = 0, Boolean arg2 = false)
		{
			this.channel = arg1;
			this.enable = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.channel = 0;
			this.enable = false;
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
			this.serializeAs_ChannelEnablingChangeMessage(arg1);
		}
		
		public void serializeAs_ChannelEnablingChangeMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.channel);
			arg1.WriteBoolean(this.enable);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChannelEnablingChangeMessage(arg1);
		}
		
		public void deserializeAs_ChannelEnablingChangeMessage(BigEndianReader arg1)
		{
			this.channel = (uint)arg1.ReadByte();
			if ( this.channel < 0 )
			{
				throw new Exception("Forbidden value (" + this.channel + ") on element of ChannelEnablingChangeMessage.channel.");
			}
			this.enable = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
