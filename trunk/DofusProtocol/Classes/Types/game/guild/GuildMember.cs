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
	
	public class GuildMember : CharacterMinimalInformations
	{
		public const uint protocolId = 88;
		public int breed = 0;
		public Boolean sex = false;
		public uint rank = 0;
		public double givenExperience = 0;
		public uint experienceGivenPercent = 0;
		public uint rights = 0;
		public uint connected = 99;
		public int alignmentSide = 0;
		public uint hoursSinceLastConnection = 0;
		public int moodSmileyId = 0;
		
		public GuildMember()
		{
		}
		
		public GuildMember(uint arg1, uint arg2, String arg3, int arg4, Boolean arg5, uint arg6, double arg7, uint arg8, uint arg9, uint arg10, int arg11, uint arg12, int arg13)
			: this()
		{
			initGuildMember(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
		}
		
		public override uint getTypeId()
		{
			return 88;
		}
		
		public GuildMember initGuildMember(uint arg1 = 0, uint arg2 = 0, String arg3 = "", int arg4 = 0, Boolean arg5 = false, uint arg6 = 0, double arg7 = 0, uint arg8 = 0, uint arg9 = 0, uint arg10 = 99, int arg11 = 0, uint arg12 = 0, int arg13 = 0)
		{
			base.initCharacterMinimalInformations(arg1, arg2, arg3);
			this.breed = arg4;
			this.sex = arg5;
			this.rank = arg6;
			this.givenExperience = arg7;
			this.experienceGivenPercent = arg8;
			this.rights = arg9;
			this.connected = arg10;
			this.alignmentSide = arg11;
			this.hoursSinceLastConnection = arg12;
			this.moodSmileyId = arg13;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.breed = 0;
			this.sex = false;
			this.rank = 0;
			this.givenExperience = 0;
			this.experienceGivenPercent = 0;
			this.rights = 0;
			this.connected = 99;
			this.alignmentSide = 0;
			this.hoursSinceLastConnection = 0;
			this.moodSmileyId = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GuildMember(arg1);
		}
		
		public void serializeAs_GuildMember(BigEndianWriter arg1)
		{
			base.serializeAs_CharacterMinimalInformations(arg1);
			arg1.WriteByte((byte)this.breed);
			arg1.WriteBoolean(this.sex);
			if ( this.rank < 0 )
			{
				throw new Exception("Forbidden value (" + this.rank + ") on element rank.");
			}
			arg1.WriteShort((short)this.rank);
			if ( this.givenExperience < 0 )
			{
				throw new Exception("Forbidden value (" + this.givenExperience + ") on element givenExperience.");
			}
			arg1.WriteDouble(this.givenExperience);
			if ( this.experienceGivenPercent < 0 || this.experienceGivenPercent > 100 )
			{
				throw new Exception("Forbidden value (" + this.experienceGivenPercent + ") on element experienceGivenPercent.");
			}
			arg1.WriteByte((byte)this.experienceGivenPercent);
			if ( this.rights < 0 || this.rights > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.rights + ") on element rights.");
			}
			arg1.WriteUInt((uint)this.rights);
			arg1.WriteByte((byte)this.connected);
			arg1.WriteByte((byte)this.alignmentSide);
			if ( this.hoursSinceLastConnection < 0 || this.hoursSinceLastConnection > 65535 )
			{
				throw new Exception("Forbidden value (" + this.hoursSinceLastConnection + ") on element hoursSinceLastConnection.");
			}
			arg1.WriteShort((short)this.hoursSinceLastConnection);
			arg1.WriteByte((byte)this.moodSmileyId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildMember(arg1);
		}
		
		public void deserializeAs_GuildMember(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.breed = (int)arg1.ReadByte();
			this.sex = (Boolean)arg1.ReadBoolean();
			this.rank = (uint)arg1.ReadShort();
			if ( this.rank < 0 )
			{
				throw new Exception("Forbidden value (" + this.rank + ") on element of GuildMember.rank.");
			}
			this.givenExperience = (double)arg1.ReadDouble();
			if ( this.givenExperience < 0 )
			{
				throw new Exception("Forbidden value (" + this.givenExperience + ") on element of GuildMember.givenExperience.");
			}
			this.experienceGivenPercent = (uint)arg1.ReadByte();
			if ( this.experienceGivenPercent < 0 || this.experienceGivenPercent > 100 )
			{
				throw new Exception("Forbidden value (" + this.experienceGivenPercent + ") on element of GuildMember.experienceGivenPercent.");
			}
			this.rights = (uint)arg1.ReadUInt();
			if ( this.rights < 0 || this.rights > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.rights + ") on element of GuildMember.rights.");
			}
			this.connected = (uint)arg1.ReadByte();
			if ( this.connected < 0 )
			{
				throw new Exception("Forbidden value (" + this.connected + ") on element of GuildMember.connected.");
			}
			this.alignmentSide = (int)arg1.ReadByte();
			this.hoursSinceLastConnection = (uint)arg1.ReadUShort();
			if ( this.hoursSinceLastConnection < 0 || this.hoursSinceLastConnection > 65535 )
			{
				throw new Exception("Forbidden value (" + this.hoursSinceLastConnection + ") on element of GuildMember.hoursSinceLastConnection.");
			}
			this.moodSmileyId = (int)arg1.ReadByte();
		}
		
	}
}
