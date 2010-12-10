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
namespace Stump.DofusProtocol.Classes
{
	
	public class CharacterToRecolorInformation : Object
	{
		public const uint protocolId = 212;
		public uint id = 0;
		public List<int> colors;
		
		public CharacterToRecolorInformation()
		{
			this.colors = new List<int>();
		}
		
		public CharacterToRecolorInformation(uint arg1, List<int> arg2)
			: this()
		{
			initCharacterToRecolorInformation(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 212;
		}
		
		public CharacterToRecolorInformation initCharacterToRecolorInformation(uint arg1 = 0, List<int> arg2 = null)
		{
			this.id = arg1;
			this.colors = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.id = 0;
			this.colors = new List<int>();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_CharacterToRecolorInformation(arg1);
		}
		
		public void serializeAs_CharacterToRecolorInformation(BigEndianWriter arg1)
		{
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element id.");
			}
			arg1.WriteInt((int)this.id);
			arg1.WriteShort((short)this.colors.Count);
			var loc1 = 0;
			while ( loc1 < this.colors.Count )
			{
				arg1.WriteInt((int)this.colors[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterToRecolorInformation(arg1);
		}
		
		public void deserializeAs_CharacterToRecolorInformation(BigEndianReader arg1)
		{
			var loc3 = 0;
			this.id = (uint)arg1.ReadInt();
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element of CharacterToRecolorInformation.id.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = arg1.ReadInt();
				this.colors.Add((int)loc3);
				++loc2;
			}
		}
		
	}
}
