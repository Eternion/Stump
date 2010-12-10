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
	
	public class SpellMovementMessage : Message
	{
		public const uint protocolId = 5746;
		internal Boolean _isInitialized = false;
		public uint spellId = 0;
		public uint position = 0;
		
		public SpellMovementMessage()
		{
		}
		
		public SpellMovementMessage(uint arg1, uint arg2)
			: this()
		{
			initSpellMovementMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5746;
		}
		
		public SpellMovementMessage initSpellMovementMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.spellId = arg1;
			this.position = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.spellId = 0;
			this.position = 0;
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
			this.serializeAs_SpellMovementMessage(arg1);
		}
		
		public void serializeAs_SpellMovementMessage(BigEndianWriter arg1)
		{
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element spellId.");
			}
			arg1.WriteShort((short)this.spellId);
			if ( this.position < 63 || this.position > 255 )
			{
				throw new Exception("Forbidden value (" + this.position + ") on element position.");
			}
			arg1.WriteByte((byte)this.position);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SpellMovementMessage(arg1);
		}
		
		public void deserializeAs_SpellMovementMessage(BigEndianReader arg1)
		{
			this.spellId = (uint)arg1.ReadShort();
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element of SpellMovementMessage.spellId.");
			}
			this.position = (uint)arg1.ReadByte();
			if ( this.position < 63 || this.position > 255 )
			{
				throw new Exception("Forbidden value (" + this.position + ") on element of SpellMovementMessage.position.");
			}
		}
		
	}
}
