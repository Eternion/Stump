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
	
	public class CharacterLevelUpInformationMessage : CharacterLevelUpMessage
	{
		public const uint protocolId = 6076;
		internal Boolean _isInitialized = false;
		public String name = "";
		public uint id = 0;
		public int relationType = 0;
		
		public CharacterLevelUpInformationMessage()
		{
		}
		
		public CharacterLevelUpInformationMessage(uint arg1, String arg2, uint arg3, int arg4)
			: this()
		{
			initCharacterLevelUpInformationMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 6076;
		}
		
		public CharacterLevelUpInformationMessage initCharacterLevelUpInformationMessage(uint arg1 = 0, String arg2 = "", uint arg3 = 0, int arg4 = 0)
		{
			base.initCharacterLevelUpMessage(arg1);
			this.name = arg2;
			this.id = arg3;
			this.relationType = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.name = "";
			this.id = 0;
			this.relationType = 0;
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
			this.serializeAs_CharacterLevelUpInformationMessage(arg1);
		}
		
		public void serializeAs_CharacterLevelUpInformationMessage(BigEndianWriter arg1)
		{
			base.serializeAs_CharacterLevelUpMessage(arg1);
			arg1.WriteUTF((string)this.name);
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element id.");
			}
			arg1.WriteInt((int)this.id);
			arg1.WriteByte((byte)this.relationType);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterLevelUpInformationMessage(arg1);
		}
		
		public void deserializeAs_CharacterLevelUpInformationMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.name = (String)arg1.ReadUTF();
			this.id = (uint)arg1.ReadInt();
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element of CharacterLevelUpInformationMessage.id.");
			}
			this.relationType = (int)arg1.ReadByte();
		}
		
	}
}
