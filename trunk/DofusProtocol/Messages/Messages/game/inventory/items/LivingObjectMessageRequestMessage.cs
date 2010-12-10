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
	
	public class LivingObjectMessageRequestMessage : Message
	{
		public const uint protocolId = 6066;
		internal Boolean _isInitialized = false;
		public uint msgId = 0;
		public List<String> parameters;
		public uint livingObject = 0;
		
		public LivingObjectMessageRequestMessage()
		{
			this.parameters = new List<String>();
		}
		
		public LivingObjectMessageRequestMessage(uint arg1, List<String> arg2, uint arg3)
			: this()
		{
			initLivingObjectMessageRequestMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6066;
		}
		
		public LivingObjectMessageRequestMessage initLivingObjectMessageRequestMessage(uint arg1 = 0, List<String> arg2 = null, uint arg3 = 0)
		{
			this.msgId = arg1;
			this.parameters = arg2;
			this.livingObject = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.msgId = 0;
			this.parameters = new List<String>();
			this.livingObject = 0;
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
			this.serializeAs_LivingObjectMessageRequestMessage(arg1);
		}
		
		public void serializeAs_LivingObjectMessageRequestMessage(BigEndianWriter arg1)
		{
			if ( this.msgId < 0 )
			{
				throw new Exception("Forbidden value (" + this.msgId + ") on element msgId.");
			}
			arg1.WriteShort((short)this.msgId);
			arg1.WriteShort((short)this.parameters.Count);
			var loc1 = 0;
			while ( loc1 < this.parameters.Count )
			{
				arg1.WriteUTF((string)this.parameters[loc1]);
				++loc1;
			}
			if ( this.livingObject < 0 || this.livingObject > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.livingObject + ") on element livingObject.");
			}
			arg1.WriteUInt((uint)this.livingObject);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_LivingObjectMessageRequestMessage(arg1);
		}
		
		public void deserializeAs_LivingObjectMessageRequestMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.msgId = (uint)arg1.ReadShort();
			if ( this.msgId < 0 )
			{
				throw new Exception("Forbidden value (" + this.msgId + ") on element of LivingObjectMessageRequestMessage.msgId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = arg1.ReadUTF();
				this.parameters.Add((String)loc3);
				++loc2;
			}
			this.livingObject = (uint)arg1.ReadUInt();
			if ( this.livingObject < 0 || this.livingObject > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.livingObject + ") on element of LivingObjectMessageRequestMessage.livingObject.");
			}
		}
		
	}
}
