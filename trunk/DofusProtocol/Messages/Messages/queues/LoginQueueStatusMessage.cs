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
	
	public class LoginQueueStatusMessage : Message
	{
		public const uint protocolId = 10;
		internal Boolean _isInitialized = false;
		public uint position = 0;
		public uint total = 0;
		
		public LoginQueueStatusMessage()
		{
		}
		
		public LoginQueueStatusMessage(uint arg1, uint arg2)
			: this()
		{
			initLoginQueueStatusMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 10;
		}
		
		public LoginQueueStatusMessage initLoginQueueStatusMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.position = arg1;
			this.total = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.position = 0;
			this.total = 0;
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
			this.serializeAs_LoginQueueStatusMessage(arg1);
		}
		
		public void serializeAs_LoginQueueStatusMessage(BigEndianWriter arg1)
		{
			if ( this.position < 0 || this.position > 65535 )
			{
				throw new Exception("Forbidden value (" + this.position + ") on element position.");
			}
			arg1.WriteShort((short)this.position);
			if ( this.total < 0 || this.total > 65535 )
			{
				throw new Exception("Forbidden value (" + this.total + ") on element total.");
			}
			arg1.WriteShort((short)this.total);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_LoginQueueStatusMessage(arg1);
		}
		
		public void deserializeAs_LoginQueueStatusMessage(BigEndianReader arg1)
		{
			this.position = (uint)arg1.ReadUShort();
			if ( this.position < 0 || this.position > 65535 )
			{
				throw new Exception("Forbidden value (" + this.position + ") on element of LoginQueueStatusMessage.position.");
			}
			this.total = (uint)arg1.ReadUShort();
			if ( this.total < 0 || this.total > 65535 )
			{
				throw new Exception("Forbidden value (" + this.total + ") on element of LoginQueueStatusMessage.total.");
			}
		}
		
	}
}
