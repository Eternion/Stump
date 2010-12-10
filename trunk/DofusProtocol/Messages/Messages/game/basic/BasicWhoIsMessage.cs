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
	
	public class BasicWhoIsMessage : Message
	{
		public const uint protocolId = 180;
		internal Boolean _isInitialized = false;
		public Boolean self = false;
		public int position = 0;
		public String accountNickname = "";
		public String characterName = "";
		public int areaId = 0;
		
		public BasicWhoIsMessage()
		{
		}
		
		public BasicWhoIsMessage(Boolean arg1, int arg2, String arg3, String arg4, int arg5)
			: this()
		{
			initBasicWhoIsMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 180;
		}
		
		public BasicWhoIsMessage initBasicWhoIsMessage(Boolean arg1 = false, int arg2 = 0, String arg3 = "", String arg4 = "", int arg5 = 0)
		{
			this.self = arg1;
			this.position = arg2;
			this.accountNickname = arg3;
			this.characterName = arg4;
			this.areaId = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.self = false;
			this.position = 0;
			this.accountNickname = "";
			this.characterName = "";
			this.areaId = 0;
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
			this.serializeAs_BasicWhoIsMessage(arg1);
		}
		
		public void serializeAs_BasicWhoIsMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.self);
			arg1.WriteByte((byte)this.position);
			arg1.WriteUTF((string)this.accountNickname);
			arg1.WriteUTF((string)this.characterName);
			arg1.WriteShort((short)this.areaId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_BasicWhoIsMessage(arg1);
		}
		
		public void deserializeAs_BasicWhoIsMessage(BigEndianReader arg1)
		{
			this.self = (Boolean)arg1.ReadBoolean();
			this.position = (int)arg1.ReadByte();
			this.accountNickname = (String)arg1.ReadUTF();
			this.characterName = (String)arg1.ReadUTF();
			this.areaId = (int)arg1.ReadShort();
		}
		
	}
}
