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
	
	public class FightResultFighterListEntry : FightResultListEntry
	{
		public const uint protocolId = 189;
		public int id = 0;
		public Boolean alive = false;
		
		public FightResultFighterListEntry()
		{
		}
		
		public FightResultFighterListEntry(uint arg1, FightLoot arg2, int arg3, Boolean arg4)
			: this()
		{
			initFightResultFighterListEntry(arg1, arg2, arg3, arg4);
		}
		
		public override uint getTypeId()
		{
			return 189;
		}
		
		public FightResultFighterListEntry initFightResultFighterListEntry(uint arg1 = 0, FightLoot arg2 = null, int arg3 = 0, Boolean arg4 = false)
		{
			base.initFightResultListEntry(arg1, arg2);
			this.id = arg3;
			this.alive = arg4;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.id = 0;
			this.alive = false;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightResultFighterListEntry(arg1);
		}
		
		public void serializeAs_FightResultFighterListEntry(BigEndianWriter arg1)
		{
			base.serializeAs_FightResultListEntry(arg1);
			arg1.WriteInt((int)this.id);
			arg1.WriteBoolean(this.alive);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightResultFighterListEntry(arg1);
		}
		
		public void deserializeAs_FightResultFighterListEntry(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.id = (int)arg1.ReadInt();
			this.alive = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
