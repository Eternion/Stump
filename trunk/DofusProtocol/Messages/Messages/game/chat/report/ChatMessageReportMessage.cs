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
	
	public class ChatMessageReportMessage : Message
	{
		public const uint protocolId = 821;
		internal Boolean _isInitialized = false;
		public String senderName = "";
		public String content = "";
		public uint timestamp = 0;
		public uint channel = 0;
		public String fingerprint = "";
		public uint reason = 0;
		
		public ChatMessageReportMessage()
		{
		}
		
		public ChatMessageReportMessage(String arg1, String arg2, uint arg3, uint arg4, String arg5, uint arg6)
			: this()
		{
			initChatMessageReportMessage(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getMessageId()
		{
			return 821;
		}
		
		public ChatMessageReportMessage initChatMessageReportMessage(String arg1 = "", String arg2 = "", uint arg3 = 0, uint arg4 = 0, String arg5 = "", uint arg6 = 0)
		{
			this.senderName = arg1;
			this.content = arg2;
			this.timestamp = arg3;
			this.channel = arg4;
			this.fingerprint = arg5;
			this.reason = arg6;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.senderName = "";
			this.content = "";
			this.timestamp = 0;
			this.channel = 0;
			this.fingerprint = "";
			this.reason = 0;
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
			this.serializeAs_ChatMessageReportMessage(arg1);
		}
		
		public void serializeAs_ChatMessageReportMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.senderName);
			arg1.WriteUTF((string)this.content);
			if ( this.timestamp < 0 )
			{
				throw new Exception("Forbidden value (" + this.timestamp + ") on element timestamp.");
			}
			arg1.WriteInt((int)this.timestamp);
			arg1.WriteByte((byte)this.channel);
			arg1.WriteUTF((string)this.fingerprint);
			if ( this.reason < 0 )
			{
				throw new Exception("Forbidden value (" + this.reason + ") on element reason.");
			}
			arg1.WriteByte((byte)this.reason);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChatMessageReportMessage(arg1);
		}
		
		public void deserializeAs_ChatMessageReportMessage(BigEndianReader arg1)
		{
			this.senderName = (String)arg1.ReadUTF();
			this.content = (String)arg1.ReadUTF();
			this.timestamp = (uint)arg1.ReadInt();
			if ( this.timestamp < 0 )
			{
				throw new Exception("Forbidden value (" + this.timestamp + ") on element of ChatMessageReportMessage.timestamp.");
			}
			this.channel = (uint)arg1.ReadByte();
			if ( this.channel < 0 )
			{
				throw new Exception("Forbidden value (" + this.channel + ") on element of ChatMessageReportMessage.channel.");
			}
			this.fingerprint = (String)arg1.ReadUTF();
			this.reason = (uint)arg1.ReadByte();
			if ( this.reason < 0 )
			{
				throw new Exception("Forbidden value (" + this.reason + ") on element of ChatMessageReportMessage.reason.");
			}
		}
		
	}
}
