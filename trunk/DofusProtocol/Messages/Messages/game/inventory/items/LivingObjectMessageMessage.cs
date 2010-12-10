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
	
	public class LivingObjectMessageMessage : Message
	{
		public const uint protocolId = 6065;
		internal Boolean _isInitialized = false;
		public uint msgId = 0;
		public uint timeStamp = 0;
		public String owner = "";
		public uint objectGenericId = 0;
		
		public LivingObjectMessageMessage()
		{
		}
		
		public LivingObjectMessageMessage(uint arg1, uint arg2, String arg3, uint arg4)
			: this()
		{
			initLivingObjectMessageMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 6065;
		}
		
		public LivingObjectMessageMessage initLivingObjectMessageMessage(uint arg1 = 0, uint arg2 = 0, String arg3 = "", uint arg4 = 0)
		{
			this.msgId = arg1;
			this.timeStamp = arg2;
			this.owner = arg3;
			this.@objectGenericId = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.msgId = 0;
			this.timeStamp = 0;
			this.owner = "";
			this.@objectGenericId = 0;
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
			this.serializeAs_LivingObjectMessageMessage(arg1);
		}
		
		public void serializeAs_LivingObjectMessageMessage(BigEndianWriter arg1)
		{
			if ( this.msgId < 0 )
			{
				throw new Exception("Forbidden value (" + this.msgId + ") on element msgId.");
			}
			arg1.WriteShort((short)this.msgId);
			if ( this.timeStamp < 0 || this.timeStamp > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.timeStamp + ") on element timeStamp.");
			}
			arg1.WriteUInt((uint)this.timeStamp);
			arg1.WriteUTF((string)this.owner);
			if ( this.@objectGenericId < 0 || this.@objectGenericId > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.@objectGenericId + ") on element objectGenericId.");
			}
			arg1.WriteUInt((uint)this.@objectGenericId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_LivingObjectMessageMessage(arg1);
		}
		
		public void deserializeAs_LivingObjectMessageMessage(BigEndianReader arg1)
		{
			this.msgId = (uint)arg1.ReadShort();
			if ( this.msgId < 0 )
			{
				throw new Exception("Forbidden value (" + this.msgId + ") on element of LivingObjectMessageMessage.msgId.");
			}
			this.timeStamp = (uint)arg1.ReadUInt();
			if ( this.timeStamp < 0 || this.timeStamp > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.timeStamp + ") on element of LivingObjectMessageMessage.timeStamp.");
			}
			this.owner = (String)arg1.ReadUTF();
			this.@objectGenericId = (uint)arg1.ReadUInt();
			if ( this.@objectGenericId < 0 || this.@objectGenericId > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.@objectGenericId + ") on element of LivingObjectMessageMessage.objectGenericId.");
			}
		}
		
	}
}
