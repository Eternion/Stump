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
	
	public class FightResultListEntry : Object
	{
		public const uint protocolId = 16;
		public uint outcome = 0;
		public FightLoot rewards;
		
		public FightResultListEntry()
		{
			this.rewards = new FightLoot();
		}
		
		public FightResultListEntry(uint arg1, FightLoot arg2)
			: this()
		{
			initFightResultListEntry(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 16;
		}
		
		public FightResultListEntry initFightResultListEntry(uint arg1 = 0, FightLoot arg2 = null)
		{
			this.outcome = arg1;
			this.rewards = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.outcome = 0;
			this.rewards = new FightLoot();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightResultListEntry(arg1);
		}
		
		public void serializeAs_FightResultListEntry(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.outcome);
			this.rewards.serializeAs_FightLoot(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightResultListEntry(arg1);
		}
		
		public void deserializeAs_FightResultListEntry(BigEndianReader arg1)
		{
			this.outcome = (uint)arg1.ReadShort();
			if ( this.outcome < 0 )
			{
				throw new Exception("Forbidden value (" + this.outcome + ") on element of FightResultListEntry.outcome.");
			}
			this.rewards = new FightLoot();
			this.rewards.deserialize(arg1);
		}
		
	}
}
