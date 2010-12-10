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
	
	public class SequenceEndMessage : Message
	{
		public const uint protocolId = 956;
		internal Boolean _isInitialized = false;
		public uint actionId = 0;
		public int authorId = 0;
		public int sequenceType = 0;
		
		public SequenceEndMessage()
		{
		}
		
		public SequenceEndMessage(uint arg1, int arg2, int arg3)
			: this()
		{
			initSequenceEndMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 956;
		}
		
		public SequenceEndMessage initSequenceEndMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0)
		{
			this.actionId = arg1;
			this.authorId = arg2;
			this.sequenceType = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.actionId = 0;
			this.authorId = 0;
			this.sequenceType = 0;
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
			this.serializeAs_SequenceEndMessage(arg1);
		}
		
		public void serializeAs_SequenceEndMessage(BigEndianWriter arg1)
		{
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element actionId.");
			}
			arg1.WriteShort((short)this.actionId);
			arg1.WriteInt((int)this.authorId);
			arg1.WriteByte((byte)this.sequenceType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SequenceEndMessage(arg1);
		}
		
		public void deserializeAs_SequenceEndMessage(BigEndianReader arg1)
		{
			this.actionId = (uint)arg1.ReadShort();
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element of SequenceEndMessage.actionId.");
			}
			this.authorId = (int)arg1.ReadInt();
			this.sequenceType = (int)arg1.ReadByte();
		}
		
	}
}
