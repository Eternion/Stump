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
	
	public class FightTeamMemberCharacterInformations : FightTeamMemberInformations
	{
		public const uint protocolId = 13;
		public String name = "";
		public uint level = 0;
		
		public FightTeamMemberCharacterInformations()
		{
		}
		
		public FightTeamMemberCharacterInformations(int arg1, String arg2, uint arg3)
			: this()
		{
			initFightTeamMemberCharacterInformations(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 13;
		}
		
		public FightTeamMemberCharacterInformations initFightTeamMemberCharacterInformations(int arg1 = 0, String arg2 = "", uint arg3 = 0)
		{
			base.initFightTeamMemberInformations(arg1);
			this.name = arg2;
			this.level = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.name = "";
			this.level = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightTeamMemberCharacterInformations(arg1);
		}
		
		public void serializeAs_FightTeamMemberCharacterInformations(BigEndianWriter arg1)
		{
			base.serializeAs_FightTeamMemberInformations(arg1);
			arg1.WriteUTF((string)this.name);
			if ( this.level < 0 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element level.");
			}
			arg1.WriteShort((short)this.level);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightTeamMemberCharacterInformations(arg1);
		}
		
		public void deserializeAs_FightTeamMemberCharacterInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.name = (String)arg1.ReadUTF();
			this.level = (uint)arg1.ReadShort();
			if ( this.level < 0 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element of FightTeamMemberCharacterInformations.level.");
			}
		}
		
	}
}
