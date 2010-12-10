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
	
	public class AbstractGameActionMessage : Message
	{
		public const uint protocolId = 1000;
		internal Boolean _isInitialized = false;
		public uint actionId = 0;
		public int sourceId = 0;
		
		public AbstractGameActionMessage()
		{
		}
		
		public AbstractGameActionMessage(uint arg1, int arg2)
			: this()
		{
			initAbstractGameActionMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 1000;
		}
		
		public AbstractGameActionMessage initAbstractGameActionMessage(uint arg1 = 0, int arg2 = 0)
		{
			this.actionId = arg1;
			this.sourceId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.actionId = 0;
			this.sourceId = 0;
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
			this.serializeAs_AbstractGameActionMessage(arg1);
		}
		
		public void serializeAs_AbstractGameActionMessage(BigEndianWriter arg1)
		{
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element actionId.");
			}
			arg1.WriteShort((short)this.actionId);
			arg1.WriteInt((int)this.sourceId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AbstractGameActionMessage(arg1);
		}
		
		public void deserializeAs_AbstractGameActionMessage(BigEndianReader arg1)
		{
			this.actionId = (uint)arg1.ReadShort();
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element of AbstractGameActionMessage.actionId.");
			}
			this.sourceId = (int)arg1.ReadInt();
		}
		
	}
}
