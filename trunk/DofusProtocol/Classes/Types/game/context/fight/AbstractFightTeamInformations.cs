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
	
	public class AbstractFightTeamInformations : Object
	{
		public const uint protocolId = 116;
		public uint teamId = 2;
		public int leaderId = 0;
		public int teamSide = 0;
		public uint teamTypeId = 0;
		
		public AbstractFightTeamInformations()
		{
		}
		
		public AbstractFightTeamInformations(uint arg1, int arg2, int arg3, uint arg4)
			: this()
		{
			initAbstractFightTeamInformations(arg1, arg2, arg3, arg4);
		}
		
		public virtual uint getTypeId()
		{
			return 116;
		}
		
		public AbstractFightTeamInformations initAbstractFightTeamInformations(uint arg1 = 2, int arg2 = 0, int arg3 = 0, uint arg4 = 0)
		{
			this.teamId = arg1;
			this.leaderId = arg2;
			this.teamSide = arg3;
			this.teamTypeId = arg4;
			return this;
		}
		
		public virtual void reset()
		{
			this.teamId = 2;
			this.leaderId = 0;
			this.teamSide = 0;
			this.teamTypeId = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_AbstractFightTeamInformations(arg1);
		}
		
		public void serializeAs_AbstractFightTeamInformations(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.teamId);
			arg1.WriteInt((int)this.leaderId);
			arg1.WriteByte((byte)this.teamSide);
			arg1.WriteByte((byte)this.teamTypeId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AbstractFightTeamInformations(arg1);
		}
		
		public void deserializeAs_AbstractFightTeamInformations(BigEndianReader arg1)
		{
			this.teamId = (uint)arg1.ReadByte();
			if ( this.teamId < 0 )
			{
				throw new Exception("Forbidden value (" + this.teamId + ") on element of AbstractFightTeamInformations.teamId.");
			}
			this.leaderId = (int)arg1.ReadInt();
			this.teamSide = (int)arg1.ReadByte();
			this.teamTypeId = (uint)arg1.ReadByte();
			if ( this.teamTypeId < 0 )
			{
				throw new Exception("Forbidden value (" + this.teamTypeId + ") on element of AbstractFightTeamInformations.teamTypeId.");
			}
		}
		
	}
}
