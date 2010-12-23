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
	
	public class CharacterCreationRequestMessage : Message
	{
		public const uint protocolId = 160;
		internal Boolean _isInitialized = false;
		public String name = "";
		public int breed = 0;
		public Boolean sex = false;
		public List<int> colors;
		
		public CharacterCreationRequestMessage()
		{
			this.colors = new List<int>(new int[6]);
		}
		
		public CharacterCreationRequestMessage(String arg1, int arg2, Boolean arg3, List<int> arg4)
			: this()
		{
			initCharacterCreationRequestMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 160;
		}
		
		public CharacterCreationRequestMessage initCharacterCreationRequestMessage(String arg1 = "", int arg2 = 0, Boolean arg3 = false, List<int> arg4 = null)
		{
			this.name = arg1;
			this.breed = arg2;
			this.sex = arg3;
			this.colors = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.name = "";
			this.breed = 0;
			this.sex = false;
			this.colors = new List<int>(new int[6]);
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
			this.serializeAs_CharacterCreationRequestMessage(arg1);
		}
		
		public void serializeAs_CharacterCreationRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.name);
			arg1.WriteByte((byte)this.breed);
			arg1.WriteBoolean(this.sex);
			var loc1 = 0;
			while ( loc1 < 6 )
			{
				arg1.WriteInt((int)this.colors[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterCreationRequestMessage(arg1);
		}
		
		public void deserializeAs_CharacterCreationRequestMessage(BigEndianReader arg1)
		{
			this.name = (String)arg1.ReadUTF();
			this.breed = (int)arg1.ReadByte();
			if ( this.breed < (int)Stump.DofusProtocol.Enums.BreedEnum.Feca || this.breed > (int)Stump.DofusProtocol.Enums.BreedEnum.Zobal )
			{
				throw new Exception("Forbidden value (" + this.breed + ") on element of CharacterCreationRequestMessage.breed.");
			}
			this.sex = (Boolean)arg1.ReadBoolean();
			var loc1 = 0;
			while ( loc1 < 6 )
			{
				this.colors[loc1] = arg1.ReadInt();
				++loc1;
			}
		}
		
	}
}
