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
	
	public class CharactersListMessage : Message
	{
		public const uint protocolId = 151;
		internal Boolean _isInitialized = false;
		public Boolean hasStartupActions = false;
		public Boolean tutorielAvailable = false;
		public List<CharacterBaseInformations> characters;
		
		public CharactersListMessage()
		{
			this.characters = new List<CharacterBaseInformations>();
		}
		
		public CharactersListMessage(Boolean arg1, Boolean arg2, List<CharacterBaseInformations> arg3)
			: this()
		{
			initCharactersListMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 151;
		}
		
		public CharactersListMessage initCharactersListMessage(Boolean arg1 = false, Boolean arg2 = false, List<CharacterBaseInformations> arg3 = null)
		{
			this.hasStartupActions = arg1;
			this.tutorielAvailable = arg2;
			this.characters = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.hasStartupActions = false;
			this.tutorielAvailable = false;
			this.characters = new List<CharacterBaseInformations>();
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
			this.serializeAs_CharactersListMessage(arg1);
		}
		
		public void serializeAs_CharactersListMessage(BigEndianWriter arg1)
		{
			var loc1 = 0;
			BooleanByteWrapper.SetFlag(loc1, 0, this.hasStartupActions);
			BooleanByteWrapper.SetFlag(loc1, 1, this.tutorielAvailable);
			arg1.WriteByte((byte)loc1);
			arg1.WriteShort((short)this.characters.Count);
			var loc2 = 0;
			while ( loc2 < this.characters.Count )
			{
				arg1.WriteShort((short)this.characters[loc2].getTypeId());
				this.characters[loc2].serialize(arg1);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharactersListMessage(arg1);
		}
		
		public void deserializeAs_CharactersListMessage(BigEndianReader arg1)
		{
			var loc4 = 0;
			object loc5 = null;
			var loc1 = arg1.ReadByte();
			this.hasStartupActions = (Boolean)BooleanByteWrapper.GetFlag(loc1, 0);
			this.tutorielAvailable = (Boolean)BooleanByteWrapper.GetFlag(loc1, 1);
			var loc2 = (ushort)arg1.ReadUShort();
			var loc3 = 0;
			while ( loc3 < loc2 )
			{
				loc4 = (ushort)arg1.ReadUShort();
				(( loc5 = ProtocolTypeManager.GetInstance<CharacterBaseInformations>((uint)loc4)) as CharacterBaseInformations).deserialize(arg1);
				this.characters.Add((CharacterBaseInformations)loc5);
				++loc3;
			}
		}
		
	}
}
