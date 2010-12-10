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
	
	public class FightResultPlayerListEntry : FightResultFighterListEntry
	{
		public const uint protocolId = 24;
		public uint level = 0;
		public List<FightResultAdditionalData> additional;
		
		public FightResultPlayerListEntry()
		{
			this.additional = new List<FightResultAdditionalData>();
		}
		
		public FightResultPlayerListEntry(uint arg1, FightLoot arg2, int arg3, Boolean arg4, uint arg5, List<FightResultAdditionalData> arg6)
			: this()
		{
			initFightResultPlayerListEntry(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getTypeId()
		{
			return 24;
		}
		
		public FightResultPlayerListEntry initFightResultPlayerListEntry(uint arg1 = 0, FightLoot arg2 = null, int arg3 = 0, Boolean arg4 = false, uint arg5 = 0, List<FightResultAdditionalData> arg6 = null)
		{
			base.initFightResultFighterListEntry(arg1, arg2, arg3, arg4);
			this.level = arg5;
			this.additional = arg6;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.level = 0;
			this.additional = new List<FightResultAdditionalData>();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightResultPlayerListEntry(arg1);
		}
		
		public void serializeAs_FightResultPlayerListEntry(BigEndianWriter arg1)
		{
			base.serializeAs_FightResultFighterListEntry(arg1);
			if ( this.level < 1 || this.level > 200 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element level.");
			}
			arg1.WriteByte((byte)this.level);
			arg1.WriteShort((short)this.additional.Count);
			var loc1 = 0;
			while ( loc1 < this.additional.Count )
			{
				arg1.WriteShort((short)this.additional[loc1].getTypeId());
				this.additional[loc1].serialize(arg1);
				++loc1;
			}
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightResultPlayerListEntry(arg1);
		}
		
		public void deserializeAs_FightResultPlayerListEntry(BigEndianReader arg1)
		{
			var loc3 = 0;
			object loc4 = null;
			base.deserialize(arg1);
			this.level = (uint)arg1.ReadByte();
			if ( this.level < 1 || this.level > 200 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element of FightResultPlayerListEntry.level.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = (ushort)arg1.ReadUShort();
				(( loc4 = ProtocolTypeManager.GetInstance<FightResultAdditionalData>((uint)loc3)) as FightResultAdditionalData).deserialize(arg1);
				this.additional.Add((FightResultAdditionalData)loc4);
				++loc2;
			}
		}
		
	}
}
