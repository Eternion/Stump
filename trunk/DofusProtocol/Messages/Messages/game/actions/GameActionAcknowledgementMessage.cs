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
	
	public class GameActionAcknowledgementMessage : Message
	{
		public const uint protocolId = 957;
		internal Boolean _isInitialized = false;
		public Boolean valid = false;
		public int actionId = 0;
		
		public GameActionAcknowledgementMessage()
		{
		}
		
		public GameActionAcknowledgementMessage(Boolean arg1, int arg2)
			: this()
		{
			initGameActionAcknowledgementMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 957;
		}
		
		public GameActionAcknowledgementMessage initGameActionAcknowledgementMessage(Boolean arg1 = false, int arg2 = 0)
		{
			this.valid = arg1;
			this.actionId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.valid = false;
			this.actionId = 0;
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
			this.serializeAs_GameActionAcknowledgementMessage(arg1);
		}
		
		public void serializeAs_GameActionAcknowledgementMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.valid);
			arg1.WriteByte((byte)this.actionId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionAcknowledgementMessage(arg1);
		}
		
		public void deserializeAs_GameActionAcknowledgementMessage(BigEndianReader arg1)
		{
			this.valid = (Boolean)arg1.ReadBoolean();
			this.actionId = (int)arg1.ReadByte();
		}
		
	}
}
