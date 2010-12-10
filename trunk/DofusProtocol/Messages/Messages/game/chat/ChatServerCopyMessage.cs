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
	
	public class ChatServerCopyMessage : ChatAbstractServerMessage
	{
		public const uint protocolId = 882;
		internal Boolean _isInitialized = false;
		public uint receiverId = 0;
		public String receiverName = "";
		
		public ChatServerCopyMessage()
		{
		}
		
		public ChatServerCopyMessage(uint arg1, String arg2, uint arg3, String arg4, uint arg5, String arg6)
			: this()
		{
			initChatServerCopyMessage(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getMessageId()
		{
			return 882;
		}
		
		public ChatServerCopyMessage initChatServerCopyMessage(uint arg1 = 0, String arg2 = "", uint arg3 = 0, String arg4 = "", uint arg5 = 0, String arg6 = "")
		{
			base.initChatAbstractServerMessage(arg1, arg2, arg3, arg4);
			this.receiverId = arg5;
			this.receiverName = arg6;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.receiverId = 0;
			this.receiverName = "";
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ChatServerCopyMessage(arg1);
		}
		
		public void serializeAs_ChatServerCopyMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ChatAbstractServerMessage(arg1);
			if ( this.receiverId < 0 )
			{
				throw new Exception("Forbidden value (" + this.receiverId + ") on element receiverId.");
			}
			arg1.WriteInt((int)this.receiverId);
			arg1.WriteUTF((string)this.receiverName);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChatServerCopyMessage(arg1);
		}
		
		public void deserializeAs_ChatServerCopyMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.receiverId = (uint)arg1.ReadInt();
			if ( this.receiverId < 0 )
			{
				throw new Exception("Forbidden value (" + this.receiverId + ") on element of ChatServerCopyMessage.receiverId.");
			}
			this.receiverName = (String)arg1.ReadUTF();
		}
		
	}
}
