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
	
	public class GuildGetInformationsMessage : Message
	{
		public const uint protocolId = 5550;
		internal Boolean _isInitialized = false;
		public uint infoType = 0;
		
		public GuildGetInformationsMessage()
		{
		}
		
		public GuildGetInformationsMessage(uint arg1)
			: this()
		{
			initGuildGetInformationsMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5550;
		}
		
		public GuildGetInformationsMessage initGuildGetInformationsMessage(uint arg1 = 0)
		{
			this.infoType = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.infoType = 0;
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
			this.serializeAs_GuildGetInformationsMessage(arg1);
		}
		
		public void serializeAs_GuildGetInformationsMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.infoType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildGetInformationsMessage(arg1);
		}
		
		public void deserializeAs_GuildGetInformationsMessage(BigEndianReader arg1)
		{
			this.infoType = (uint)arg1.ReadByte();
			if ( this.infoType < 0 )
			{
				throw new Exception("Forbidden value (" + this.infoType + ") on element of GuildGetInformationsMessage.infoType.");
			}
		}
		
	}
}
