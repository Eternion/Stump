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
	
	public class CharactersListWithModificationsMessage : CharactersListMessage
	{
		public const uint protocolId = 6120;
		internal Boolean _isInitialized = false;
		public List<CharacterToRecolorInformation> charactersToRecolor;
		public List<int> charactersToRename;
		public List<int> unusableCharacters;
		
		public CharactersListWithModificationsMessage()
		{
			this.charactersToRecolor = new List<CharacterToRecolorInformation>();
			this.charactersToRename = new List<int>();
			this.unusableCharacters = new List<int>();
		}
		
		public CharactersListWithModificationsMessage(Boolean arg1, Boolean arg2, List<CharacterBaseInformations> arg3, List<CharacterToRecolorInformation> arg4, List<int> arg5, List<int> arg6)
			: this()
		{
			initCharactersListWithModificationsMessage(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getMessageId()
		{
			return 6120;
		}
		
		public CharactersListWithModificationsMessage initCharactersListWithModificationsMessage(Boolean arg1 = false, Boolean arg2 = false, List<CharacterBaseInformations> arg3 = null, List<CharacterToRecolorInformation> arg4 = null, List<int> arg5 = null, List<int> arg6 = null)
		{
			base.initCharactersListMessage(arg1, arg2, arg3);
			this.charactersToRecolor = arg4;
			this.charactersToRename = arg5;
			this.unusableCharacters = arg6;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.charactersToRecolor = new List<CharacterToRecolorInformation>();
			this.charactersToRename = new List<int>();
			this.unusableCharacters = new List<int>();
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
			this.serializeAs_CharactersListWithModificationsMessage(arg1);
		}
		
		public void serializeAs_CharactersListWithModificationsMessage(BigEndianWriter arg1)
		{
			base.serializeAs_CharactersListMessage(arg1);
			arg1.WriteShort((short)this.charactersToRecolor.Count);
			var loc1 = 0;
			while ( loc1 < this.charactersToRecolor.Count )
			{
				arg1.WriteShort((short)this.charactersToRecolor[loc1].getTypeId());
				this.charactersToRecolor[loc1].serialize(arg1);
				++loc1;
			}
			arg1.WriteShort((short)this.charactersToRename.Count);
			var loc2 = 0;
			while ( loc2 < this.charactersToRename.Count )
			{
				arg1.WriteInt((int)this.charactersToRename[loc2]);
				++loc2;
			}
			arg1.WriteShort((short)this.unusableCharacters.Count);
			var loc3 = 0;
			while ( loc3 < this.unusableCharacters.Count )
			{
				arg1.WriteInt((int)this.unusableCharacters[loc3]);
				++loc3;
			}
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharactersListWithModificationsMessage(arg1);
		}
		
		public void deserializeAs_CharactersListWithModificationsMessage(BigEndianReader arg1)
		{
			var loc7 = 0;
			object loc8 = null;
			var loc9 = 0;
			var loc10 = 0;
			base.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc7 = (ushort)arg1.ReadUShort();
				(( loc8 = ProtocolTypeManager.GetInstance<CharacterToRecolorInformation>((uint)loc7)) as CharacterToRecolorInformation).deserialize(arg1);
				this.charactersToRecolor.Add((CharacterToRecolorInformation)loc8);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				loc9 = arg1.ReadInt();
				this.charactersToRename.Add((int)loc9);
				++loc4;
			}
			var loc5 = (ushort)arg1.ReadUShort();
			var loc6 = 0;
			while ( loc6 < loc5 )
			{
				loc10 = arg1.ReadInt();
				this.unusableCharacters.Add((int)loc10);
				++loc6;
			}
		}
		
	}
}
