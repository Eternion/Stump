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
	
	public class SelectedServerRefusedMessage : Message
	{
		public const uint protocolId = 41;
		internal Boolean _isInitialized = false;
		public int serverId = 0;
		public uint error = 1;
		public uint serverStatus = 1;
		
		public SelectedServerRefusedMessage()
		{
		}
		
		public SelectedServerRefusedMessage(int arg1, uint arg2, uint arg3)
			: this()
		{
			initSelectedServerRefusedMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 41;
		}
		
		public SelectedServerRefusedMessage initSelectedServerRefusedMessage(int arg1 = 0, uint arg2 = 1, uint arg3 = 1)
		{
			this.serverId = arg1;
			this.error = arg2;
			this.serverStatus = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.serverId = 0;
			this.error = 1;
			this.serverStatus = 1;
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
			this.serializeAs_SelectedServerRefusedMessage(arg1);
		}
		
		public void serializeAs_SelectedServerRefusedMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.serverId);
			arg1.WriteByte((byte)this.error);
			arg1.WriteByte((byte)this.serverStatus);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SelectedServerRefusedMessage(arg1);
		}
		
		public void deserializeAs_SelectedServerRefusedMessage(BigEndianReader arg1)
		{
			this.serverId = (int)arg1.ReadShort();
			this.error = (uint)arg1.ReadByte();
			if ( this.error < 0 )
			{
				throw new Exception("Forbidden value (" + this.error + ") on element of SelectedServerRefusedMessage.error.");
			}
			this.serverStatus = (uint)arg1.ReadByte();
			if ( this.serverStatus < 0 )
			{
				throw new Exception("Forbidden value (" + this.serverStatus + ") on element of SelectedServerRefusedMessage.serverStatus.");
			}
		}
		
	}
}
