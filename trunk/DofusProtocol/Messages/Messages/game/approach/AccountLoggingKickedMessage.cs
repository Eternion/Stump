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
	
	public class AccountLoggingKickedMessage : Message
	{
		public const uint protocolId = 6029;
		internal Boolean _isInitialized = false;
		public uint days = 0;
		public uint hours = 0;
		public uint minutes = 0;
		
		public AccountLoggingKickedMessage()
		{
		}
		
		public AccountLoggingKickedMessage(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initAccountLoggingKickedMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6029;
		}
		
		public AccountLoggingKickedMessage initAccountLoggingKickedMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			this.days = arg1;
			this.hours = arg2;
			this.minutes = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.days = 0;
			this.hours = 0;
			this.minutes = 0;
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
			this.serializeAs_AccountLoggingKickedMessage(arg1);
		}
		
		public void serializeAs_AccountLoggingKickedMessage(BigEndianWriter arg1)
		{
			if ( this.days < 0 )
			{
				throw new Exception("Forbidden value (" + this.days + ") on element days.");
			}
			arg1.WriteInt((int)this.days);
			if ( this.hours < 0 )
			{
				throw new Exception("Forbidden value (" + this.hours + ") on element hours.");
			}
			arg1.WriteInt((int)this.hours);
			if ( this.minutes < 0 )
			{
				throw new Exception("Forbidden value (" + this.minutes + ") on element minutes.");
			}
			arg1.WriteInt((int)this.minutes);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AccountLoggingKickedMessage(arg1);
		}
		
		public void deserializeAs_AccountLoggingKickedMessage(BigEndianReader arg1)
		{
			this.days = (uint)arg1.ReadInt();
			if ( this.days < 0 )
			{
				throw new Exception("Forbidden value (" + this.days + ") on element of AccountLoggingKickedMessage.days.");
			}
			this.hours = (uint)arg1.ReadInt();
			if ( this.hours < 0 )
			{
				throw new Exception("Forbidden value (" + this.hours + ") on element of AccountLoggingKickedMessage.hours.");
			}
			this.minutes = (uint)arg1.ReadInt();
			if ( this.minutes < 0 )
			{
				throw new Exception("Forbidden value (" + this.minutes + ") on element of AccountLoggingKickedMessage.minutes.");
			}
		}
		
	}
}
