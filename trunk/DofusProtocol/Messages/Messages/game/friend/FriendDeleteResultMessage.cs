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
	
	public class FriendDeleteResultMessage : Message
	{
		public const uint protocolId = 5601;
		internal Boolean _isInitialized = false;
		public Boolean success = false;
		public String name = "";
		
		public FriendDeleteResultMessage()
		{
		}
		
		public FriendDeleteResultMessage(Boolean arg1, String arg2)
			: this()
		{
			initFriendDeleteResultMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5601;
		}
		
		public FriendDeleteResultMessage initFriendDeleteResultMessage(Boolean arg1 = false, String arg2 = "")
		{
			this.success = arg1;
			this.name = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.success = false;
			this.name = "";
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
			this.serializeAs_FriendDeleteResultMessage(arg1);
		}
		
		public void serializeAs_FriendDeleteResultMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.success);
			arg1.WriteUTF((string)this.name);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FriendDeleteResultMessage(arg1);
		}
		
		public void deserializeAs_FriendDeleteResultMessage(BigEndianReader arg1)
		{
			this.success = (Boolean)arg1.ReadBoolean();
			this.name = (String)arg1.ReadUTF();
		}
		
	}
}
