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
	
	public class FightTeamMemberInformations : Object
	{
		public const uint protocolId = 44;
		public int id = 0;
		
		public FightTeamMemberInformations()
		{
		}
		
		public FightTeamMemberInformations(int arg1)
			: this()
		{
			initFightTeamMemberInformations(arg1);
		}
		
		public virtual uint getTypeId()
		{
			return 44;
		}
		
		public FightTeamMemberInformations initFightTeamMemberInformations(int arg1 = 0)
		{
			this.id = arg1;
			return this;
		}
		
		public virtual void reset()
		{
			this.id = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightTeamMemberInformations(arg1);
		}
		
		public void serializeAs_FightTeamMemberInformations(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.id);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightTeamMemberInformations(arg1);
		}
		
		public void deserializeAs_FightTeamMemberInformations(BigEndianReader arg1)
		{
			this.id = (int)arg1.ReadInt();
		}
		
	}
}
