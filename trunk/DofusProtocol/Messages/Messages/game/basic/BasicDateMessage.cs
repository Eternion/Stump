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
	
	public class BasicDateMessage : Message
	{
		public const uint protocolId = 177;
		internal Boolean _isInitialized = false;
		public uint day = 0;
		public uint month = 0;
		public uint year = 0;
		
		public BasicDateMessage()
		{
		}
		
		public BasicDateMessage(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initBasicDateMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 177;
		}
		
		public BasicDateMessage initBasicDateMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			this.day = arg1;
			this.month = arg2;
			this.year = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.day = 0;
			this.month = 0;
			this.year = 0;
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
			this.serializeAs_BasicDateMessage(arg1);
		}
		
		public void serializeAs_BasicDateMessage(BigEndianWriter arg1)
		{
			if ( this.day < 0 )
			{
				throw new Exception("Forbidden value (" + this.day + ") on element day.");
			}
			arg1.WriteByte((byte)this.day);
			if ( this.month < 0 )
			{
				throw new Exception("Forbidden value (" + this.month + ") on element month.");
			}
			arg1.WriteByte((byte)this.month);
			if ( this.year < 0 )
			{
				throw new Exception("Forbidden value (" + this.year + ") on element year.");
			}
			arg1.WriteShort((short)this.year);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_BasicDateMessage(arg1);
		}
		
		public void deserializeAs_BasicDateMessage(BigEndianReader arg1)
		{
			this.day = (uint)arg1.ReadByte();
			if ( this.day < 0 )
			{
				throw new Exception("Forbidden value (" + this.day + ") on element of BasicDateMessage.day.");
			}
			this.month = (uint)arg1.ReadByte();
			if ( this.month < 0 )
			{
				throw new Exception("Forbidden value (" + this.month + ") on element of BasicDateMessage.month.");
			}
			this.year = (uint)arg1.ReadShort();
			if ( this.year < 0 )
			{
				throw new Exception("Forbidden value (" + this.year + ") on element of BasicDateMessage.year.");
			}
		}
		
	}
}
