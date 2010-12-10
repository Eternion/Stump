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
	
	public class TextInformationMessage : Message
	{
		public const uint protocolId = 780;
		internal Boolean _isInitialized = false;
		public uint msgType = 0;
		public uint msgId = 0;
		public List<String> parameters;
		
		public TextInformationMessage()
		{
			this.parameters = new List<String>();
		}
		
		public TextInformationMessage(uint arg1, uint arg2, List<String> arg3)
			: this()
		{
			initTextInformationMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 780;
		}
		
		public TextInformationMessage initTextInformationMessage(uint arg1 = 0, uint arg2 = 0, List<String> arg3 = null)
		{
			this.msgType = arg1;
			this.msgId = arg2;
			this.parameters = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.msgType = 0;
			this.msgId = 0;
			this.parameters = new List<String>();
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
			this.serializeAs_TextInformationMessage(arg1);
		}
		
		public void serializeAs_TextInformationMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.msgType);
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
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TextInformationMessage(arg1);
		}
		
		public void deserializeAs_TextInformationMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.msgType = (uint)arg1.ReadByte();
			if ( this.msgType < 0 )
			{
				throw new Exception("Forbidden value (" + this.msgType + ") on element of TextInformationMessage.msgType.");
			}
			this.msgId = (uint)arg1.ReadShort();
			if ( this.msgId < 0 )
			{
				throw new Exception("Forbidden value (" + this.msgId + ") on element of TextInformationMessage.msgId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = arg1.ReadUTF();
				this.parameters.Add((String)loc3);
				++loc2;
			}
		}
		
	}
}
