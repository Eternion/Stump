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
	
	public class FriendOnlineInformations : FriendInformations
	{
		public const uint protocolId = 92;
		public String playerName = "";
		public uint level = 0;
		public int alignmentSide = 0;
		public int breed = 0;
		public Boolean sex = false;
		public String guildName = "";
		public int moodSmileyId = 0;
		
		public FriendOnlineInformations()
		{
		}
		
		public FriendOnlineInformations(String arg1, uint arg2, uint arg3, String arg4, uint arg5, int arg6, int arg7, Boolean arg8, String arg9, int arg10)
			: this()
		{
			initFriendOnlineInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
		}
		
		public override uint getTypeId()
		{
			return 92;
		}
		
		public FriendOnlineInformations initFriendOnlineInformations(String arg1 = "", uint arg2 = 99, uint arg3 = 0, String arg4 = "", uint arg5 = 0, int arg6 = 0, int arg7 = 0, Boolean arg8 = false, String arg9 = "", int arg10 = 0)
		{
			base.initFriendInformations(arg1, arg2, arg3);
			this.playerName = arg4;
			this.level = arg5;
			this.alignmentSide = arg6;
			this.breed = arg7;
			this.sex = arg8;
			this.guildName = arg9;
			this.moodSmileyId = arg10;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.playerName = "";
			this.level = 0;
			this.alignmentSide = 0;
			this.breed = 0;
			this.sex = false;
			this.guildName = "";
			this.moodSmileyId = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FriendOnlineInformations(arg1);
		}
		
		public void serializeAs_FriendOnlineInformations(BigEndianWriter arg1)
		{
			base.serializeAs_FriendInformations(arg1);
			arg1.WriteUTF((string)this.playerName);
			if ( this.level < 0 || this.level > 200 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element level.");
			}
			arg1.WriteShort((short)this.level);
			arg1.WriteByte((byte)this.alignmentSide);
			arg1.WriteByte((byte)this.breed);
			arg1.WriteBoolean(this.sex);
			arg1.WriteUTF((string)this.guildName);
			arg1.WriteByte((byte)this.moodSmileyId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FriendOnlineInformations(arg1);
		}
		
		public void deserializeAs_FriendOnlineInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.playerName = (String)arg1.ReadUTF();
			this.level = (uint)arg1.ReadShort();
			if ( this.level < 0 || this.level > 200 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element of FriendOnlineInformations.level.");
			}
			this.alignmentSide = (int)arg1.ReadByte();
			this.breed = (int)arg1.ReadByte();
			if ( this.breed < (int)Stump.DofusProtocol.Enums.BreedEnum.Feca || this.breed > (int)Stump.DofusProtocol.Enums.BreedEnum.Pandawa )
			{
				throw new Exception("Forbidden value (" + this.breed + ") on element of FriendOnlineInformations.breed.");
			}
			this.sex = (Boolean)arg1.ReadBoolean();
			this.guildName = (String)arg1.ReadUTF();
			this.moodSmileyId = (int)arg1.ReadByte();
		}
		
	}
}
