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
	
	public class GroupMonsterStaticInformations : Object
	{
		public const uint protocolId = 140;
		public int mainCreatureGenericId = 0;
		public uint mainCreaturelevel = 0;
		public List<MonsterInGroupInformations> underlings;
		
		public GroupMonsterStaticInformations()
		{
			this.underlings = new List<MonsterInGroupInformations>();
		}
		
		public GroupMonsterStaticInformations(int arg1, uint arg2, List<MonsterInGroupInformations> arg3)
			: this()
		{
			initGroupMonsterStaticInformations(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 140;
		}
		
		public GroupMonsterStaticInformations initGroupMonsterStaticInformations(int arg1 = 0, uint arg2 = 0, List<MonsterInGroupInformations> arg3 = null)
		{
			this.mainCreatureGenericId = arg1;
			this.mainCreaturelevel = arg2;
			this.underlings = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.mainCreatureGenericId = 0;
			this.mainCreaturelevel = 0;
			this.underlings = new List<MonsterInGroupInformations>();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GroupMonsterStaticInformations(arg1);
		}
		
		public void serializeAs_GroupMonsterStaticInformations(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.mainCreatureGenericId);
			if ( this.mainCreaturelevel < 0 )
			{
				throw new Exception("Forbidden value (" + this.mainCreaturelevel + ") on element mainCreaturelevel.");
			}
			arg1.WriteShort((short)this.mainCreaturelevel);
			arg1.WriteShort((short)this.underlings.Count);
			var loc1 = 0;
			while ( loc1 < this.underlings.Count )
			{
				this.underlings[loc1].serializeAs_MonsterInGroupInformations(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GroupMonsterStaticInformations(arg1);
		}
		
		public void deserializeAs_GroupMonsterStaticInformations(BigEndianReader arg1)
		{
			object loc3 = null;
			this.mainCreatureGenericId = (int)arg1.ReadInt();
			this.mainCreaturelevel = (uint)arg1.ReadShort();
			if ( this.mainCreaturelevel < 0 )
			{
				throw new Exception("Forbidden value (" + this.mainCreaturelevel + ") on element of GroupMonsterStaticInformations.mainCreaturelevel.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new MonsterInGroupInformations()) as MonsterInGroupInformations).deserialize(arg1);
				this.underlings.Add((MonsterInGroupInformations)loc3);
				++loc2;
			}
		}
		
	}
}
