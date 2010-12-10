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
	
	public class ObjectUseOnCharacterMessage : ObjectUseMessage
	{
		public const uint protocolId = 3003;
		internal Boolean _isInitialized = false;
		public uint characterId = 0;
		
		public ObjectUseOnCharacterMessage()
		{
		}
		
		public ObjectUseOnCharacterMessage(uint arg1, uint arg2)
			: this()
		{
			initObjectUseOnCharacterMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 3003;
		}
		
		public ObjectUseOnCharacterMessage initObjectUseOnCharacterMessage(uint arg1 = 0, uint arg2 = 0)
		{
			base.initObjectUseMessage(arg1);
			this.characterId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.characterId = 0;
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectUseOnCharacterMessage(arg1);
		}
		
		public void serializeAs_ObjectUseOnCharacterMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectUseMessage(arg1);
			if ( this.characterId < 0 )
			{
				throw new Exception("Forbidden value (" + this.characterId + ") on element characterId.");
			}
			arg1.WriteInt((int)this.characterId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectUseOnCharacterMessage(arg1);
		}
		
		public void deserializeAs_ObjectUseOnCharacterMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.characterId = (uint)arg1.ReadInt();
			if ( this.characterId < 0 )
			{
				throw new Exception("Forbidden value (" + this.characterId + ") on element of ObjectUseOnCharacterMessage.characterId.");
			}
		}
		
	}
}
