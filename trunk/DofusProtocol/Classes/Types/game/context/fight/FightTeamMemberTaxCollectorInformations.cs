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
	
	public class FightTeamMemberTaxCollectorInformations : FightTeamMemberInformations
	{
		public const uint protocolId = 177;
		public uint firstNameId = 0;
		public uint lastNameId = 0;
		public uint level = 0;
		
		public FightTeamMemberTaxCollectorInformations()
		{
		}
		
		public FightTeamMemberTaxCollectorInformations(int arg1, uint arg2, uint arg3, uint arg4)
			: this()
		{
			initFightTeamMemberTaxCollectorInformations(arg1, arg2, arg3, arg4);
		}
		
		public override uint getTypeId()
		{
			return 177;
		}
		
		public FightTeamMemberTaxCollectorInformations initFightTeamMemberTaxCollectorInformations(int arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0)
		{
			base.initFightTeamMemberInformations(arg1);
			this.firstNameId = arg2;
			this.lastNameId = arg3;
			this.level = arg4;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.firstNameId = 0;
			this.lastNameId = 0;
			this.level = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightTeamMemberTaxCollectorInformations(arg1);
		}
		
		public void serializeAs_FightTeamMemberTaxCollectorInformations(BigEndianWriter arg1)
		{
			base.serializeAs_FightTeamMemberInformations(arg1);
			if ( this.firstNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstNameId + ") on element firstNameId.");
			}
			arg1.WriteShort((short)this.firstNameId);
			if ( this.lastNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastNameId + ") on element lastNameId.");
			}
			arg1.WriteShort((short)this.lastNameId);
			if ( this.level < 1 || this.level > 200 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element level.");
			}
			arg1.WriteByte((byte)this.level);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightTeamMemberTaxCollectorInformations(arg1);
		}
		
		public void deserializeAs_FightTeamMemberTaxCollectorInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.firstNameId = (uint)arg1.ReadShort();
			if ( this.firstNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstNameId + ") on element of FightTeamMemberTaxCollectorInformations.firstNameId.");
			}
			this.lastNameId = (uint)arg1.ReadShort();
			if ( this.lastNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastNameId + ") on element of FightTeamMemberTaxCollectorInformations.lastNameId.");
			}
			this.level = (uint)arg1.ReadByte();
			if ( this.level < 1 || this.level > 200 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element of FightTeamMemberTaxCollectorInformations.level.");
			}
		}
		
	}
}
