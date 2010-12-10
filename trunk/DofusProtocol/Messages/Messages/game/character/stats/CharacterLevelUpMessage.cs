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
	
	public class CharacterLevelUpMessage : Message
	{
		public const uint protocolId = 5670;
		internal Boolean _isInitialized = false;
		public uint newLevel = 0;
		
		public CharacterLevelUpMessage()
		{
		}
		
		public CharacterLevelUpMessage(uint arg1)
			: this()
		{
			initCharacterLevelUpMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5670;
		}
		
		public CharacterLevelUpMessage initCharacterLevelUpMessage(uint arg1 = 0)
		{
			this.newLevel = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.newLevel = 0;
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
			this.serializeAs_CharacterLevelUpMessage(arg1);
		}
		
		public void serializeAs_CharacterLevelUpMessage(BigEndianWriter arg1)
		{
			if ( this.newLevel < 2 || this.newLevel > 200 )
			{
				throw new Exception("Forbidden value (" + this.newLevel + ") on element newLevel.");
			}
			arg1.WriteByte((byte)this.newLevel);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterLevelUpMessage(arg1);
		}
		
		public void deserializeAs_CharacterLevelUpMessage(BigEndianReader arg1)
		{
			this.newLevel = (uint)arg1.ReadByte();
			if ( this.newLevel < 2 || this.newLevel > 200 )
			{
				throw new Exception("Forbidden value (" + this.newLevel + ") on element of CharacterLevelUpMessage.newLevel.");
			}
		}
		
	}
}
