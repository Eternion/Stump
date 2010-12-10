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
	
	public class FightExternalInformations : Object
	{
		public const uint protocolId = 117;
		public int fightId = 0;
		public uint fightStart = 0;
		public Boolean fightSpectatorLocked = false;
		public List<FightTeamLightInformations> fightTeams;
		
		public FightExternalInformations()
		{
			this.fightTeams = new List<FightTeamLightInformations>(2);
		}
		
		public FightExternalInformations(int arg1, uint arg2, Boolean arg3, List<FightTeamLightInformations> arg4)
			: this()
		{
			initFightExternalInformations(arg1, arg2, arg3, arg4);
		}
		
		public virtual uint getTypeId()
		{
			return 117;
		}
		
		public FightExternalInformations initFightExternalInformations(int arg1 = 0, uint arg2 = 0, Boolean arg3 = false, List<FightTeamLightInformations> arg4 = null)
		{
			this.fightId = arg1;
			this.fightStart = arg2;
			this.fightSpectatorLocked = arg3;
			this.fightTeams = arg4;
			return this;
		}
		
		public virtual void reset()
		{
			this.fightId = 0;
			this.fightStart = 0;
			this.fightSpectatorLocked = false;
			this.fightTeams = new List<FightTeamLightInformations>(2);
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightExternalInformations(arg1);
		}
		
		public void serializeAs_FightExternalInformations(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.fightId);
			if ( this.fightStart < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightStart + ") on element fightStart.");
			}
			arg1.WriteInt((int)this.fightStart);
			arg1.WriteBoolean(this.fightSpectatorLocked);
			var loc1 = 0;
			while ( loc1 < 2 )
			{
				this.fightTeams[loc1].serializeAs_FightTeamLightInformations(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightExternalInformations(arg1);
		}
		
		public void deserializeAs_FightExternalInformations(BigEndianReader arg1)
		{
			this.fightId = (int)arg1.ReadInt();
			this.fightStart = (uint)arg1.ReadInt();
			if ( this.fightStart < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightStart + ") on element of FightExternalInformations.fightStart.");
			}
			this.fightSpectatorLocked = (Boolean)arg1.ReadBoolean();
			var loc1 = 0;
			while ( loc1 < 2 )
			{
				this.fightTeams[loc1] = new FightTeamLightInformations();
				this.fightTeams[loc1].deserialize(arg1);
				++loc1;
			}
		}
		
	}
}
