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
	
	public class CheckFileMessage : Message
	{
		public const uint protocolId = 6156;
		internal Boolean _isInitialized = false;
		public String filenameHash = "";
		public uint type = 0;
		public String value = "";
		
		public CheckFileMessage()
		{
		}
		
		public CheckFileMessage(String arg1, uint arg2, String arg3)
			: this()
		{
			initCheckFileMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6156;
		}
		
		public CheckFileMessage initCheckFileMessage(String arg1 = "", uint arg2 = 0, String arg3 = "")
		{
			this.filenameHash = arg1;
			this.type = arg2;
			this.value = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.filenameHash = "";
			this.type = 0;
			this.value = "";
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
			this.serializeAs_CheckFileMessage(arg1);
		}
		
		public void serializeAs_CheckFileMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.filenameHash);
			arg1.WriteByte((byte)this.type);
			arg1.WriteUTF((string)this.value);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CheckFileMessage(arg1);
		}
		
		public void deserializeAs_CheckFileMessage(BigEndianReader arg1)
		{
			this.filenameHash = (String)arg1.ReadUTF();
			this.type = (uint)arg1.ReadByte();
			if ( this.type < 0 )
			{
				throw new Exception("Forbidden value (" + this.type + ") on element of CheckFileMessage.type.");
			}
			this.value = (String)arg1.ReadUTF();
		}
		
	}
}
