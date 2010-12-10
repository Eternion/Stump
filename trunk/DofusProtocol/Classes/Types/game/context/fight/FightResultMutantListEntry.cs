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
	
	public class FightResultMutantListEntry : FightResultFighterListEntry
	{
		public const uint protocolId = 216;
		public uint level = 0;
		
		public FightResultMutantListEntry()
		{
		}
		
		public FightResultMutantListEntry(uint arg1, FightLoot arg2, int arg3, Boolean arg4, uint arg5)
			: this()
		{
			initFightResultMutantListEntry(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getTypeId()
		{
			return 216;
		}
		
		public FightResultMutantListEntry initFightResultMutantListEntry(uint arg1 = 0, FightLoot arg2 = null, int arg3 = 0, Boolean arg4 = false, uint arg5 = 0)
		{
			base.initFightResultFighterListEntry(arg1, arg2, arg3, arg4);
			this.level = arg5;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.level = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightResultMutantListEntry(arg1);
		}
		
		public void serializeAs_FightResultMutantListEntry(BigEndianWriter arg1)
		{
			base.serializeAs_FightResultFighterListEntry(arg1);
			if ( this.level < 0 || this.level > 65535 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element level.");
			}
			arg1.WriteShort((short)this.level);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightResultMutantListEntry(arg1);
		}
		
		public void deserializeAs_FightResultMutantListEntry(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.level = (uint)arg1.ReadUShort();
			if ( this.level < 0 || this.level > 65535 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element of FightResultMutantListEntry.level.");
			}
		}
		
	}
}
