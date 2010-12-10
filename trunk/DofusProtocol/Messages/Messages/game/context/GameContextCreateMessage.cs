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
	
	public class GameContextCreateMessage : Message
	{
		public const uint protocolId = 200;
		internal Boolean _isInitialized = false;
		public uint context = 1;
		
		public GameContextCreateMessage()
		{
		}
		
		public GameContextCreateMessage(uint arg1)
			: this()
		{
			initGameContextCreateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 200;
		}
		
		public GameContextCreateMessage initGameContextCreateMessage(uint arg1 = 1)
		{
			this.context = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.context = 1;
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
			this.serializeAs_GameContextCreateMessage(arg1);
		}
		
		public void serializeAs_GameContextCreateMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.context);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameContextCreateMessage(arg1);
		}
		
		public void deserializeAs_GameContextCreateMessage(BigEndianReader arg1)
		{
			this.context = (uint)arg1.ReadByte();
			if ( this.context < 0 )
			{
				throw new Exception("Forbidden value (" + this.context + ") on element of GameContextCreateMessage.context.");
			}
		}
		
	}
}
