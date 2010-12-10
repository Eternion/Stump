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
	
	public class FightTeamInformations : AbstractFightTeamInformations
	{
		public const uint protocolId = 33;
		public List<FightTeamMemberInformations> teamMembers;
		
		public FightTeamInformations()
		{
			this.teamMembers = new List<FightTeamMemberInformations>();
		}
		
		public FightTeamInformations(uint arg1, int arg2, int arg3, uint arg4, List<FightTeamMemberInformations> arg5)
			: this()
		{
			initFightTeamInformations(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getTypeId()
		{
			return 33;
		}
		
		public FightTeamInformations initFightTeamInformations(uint arg1 = 2, int arg2 = 0, int arg3 = 0, uint arg4 = 0, List<FightTeamMemberInformations> arg5 = null)
		{
			base.initAbstractFightTeamInformations(arg1, arg2, arg3, arg4);
			this.teamMembers = arg5;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.teamMembers = new List<FightTeamMemberInformations>();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightTeamInformations(arg1);
		}
		
		public void serializeAs_FightTeamInformations(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractFightTeamInformations(arg1);
			arg1.WriteShort((short)this.teamMembers.Count);
			var loc1 = 0;
			while ( loc1 < this.teamMembers.Count )
			{
				arg1.WriteShort((short)this.teamMembers[loc1].getTypeId());
				this.teamMembers[loc1].serialize(arg1);
				++loc1;
			}
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightTeamInformations(arg1);
		}
		
		public void deserializeAs_FightTeamInformations(BigEndianReader arg1)
		{
			var loc3 = 0;
			object loc4 = null;
			base.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = (ushort)arg1.ReadUShort();
				(( loc4 = ProtocolTypeManager.GetInstance<FightTeamMemberInformations>((uint)loc3)) as FightTeamMemberInformations).deserialize(arg1);
				this.teamMembers.Add((FightTeamMemberInformations)loc4);
				++loc2;
			}
		}
		
	}
}
