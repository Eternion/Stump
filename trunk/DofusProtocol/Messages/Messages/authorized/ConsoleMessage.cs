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
	
	public class ConsoleMessage : Message
	{
		public const uint protocolId = 75;
		internal Boolean _isInitialized = false;
		public uint type = 0;
		public String content = "";
		
		public ConsoleMessage()
		{
		}
		
		public ConsoleMessage(uint arg1, String arg2)
			: this()
		{
			initConsoleMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 75;
		}
		
		public ConsoleMessage initConsoleMessage(uint arg1 = 0, String arg2 = "")
		{
			this.type = arg1;
			this.content = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.type = 0;
			this.content = "";
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
			this.serializeAs_ConsoleMessage(arg1);
		}
		
		public void serializeAs_ConsoleMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.type);
			arg1.WriteUTF((string)this.content);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ConsoleMessage(arg1);
		}
		
		public void deserializeAs_ConsoleMessage(BigEndianReader arg1)
		{
			this.type = (uint)arg1.ReadByte();
			if ( this.type < 0 )
			{
				throw new Exception("Forbidden value (" + this.type + ") on element of ConsoleMessage.type.");
			}
			this.content = (String)arg1.ReadUTF();
		}
		
	}
}
