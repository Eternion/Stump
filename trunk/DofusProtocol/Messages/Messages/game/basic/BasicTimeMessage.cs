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
	
	public class BasicTimeMessage : Message
	{
		public const uint protocolId = 175;
		internal Boolean _isInitialized = false;
		public uint timestamp = 0;
		public int timezoneOffset = 0;
		
		public BasicTimeMessage()
		{
		}
		
		public BasicTimeMessage(uint arg1, int arg2)
			: this()
		{
			initBasicTimeMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 175;
		}
		
		public BasicTimeMessage initBasicTimeMessage(uint arg1 = 0, int arg2 = 0)
		{
			this.timestamp = arg1;
			this.timezoneOffset = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.timestamp = 0;
			this.timezoneOffset = 0;
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
			this.serializeAs_BasicTimeMessage(arg1);
		}
		
		public void serializeAs_BasicTimeMessage(BigEndianWriter arg1)
		{
			if ( this.timestamp < 0 )
			{
				throw new Exception("Forbidden value (" + this.timestamp + ") on element timestamp.");
			}
			arg1.WriteInt((int)this.timestamp);
			arg1.WriteShort((short)this.timezoneOffset);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_BasicTimeMessage(arg1);
		}
		
		public void deserializeAs_BasicTimeMessage(BigEndianReader arg1)
		{
			this.timestamp = (uint)arg1.ReadInt();
			if ( this.timestamp < 0 )
			{
				throw new Exception("Forbidden value (" + this.timestamp + ") on element of BasicTimeMessage.timestamp.");
			}
			this.timezoneOffset = (int)arg1.ReadShort();
		}
		
	}
}
