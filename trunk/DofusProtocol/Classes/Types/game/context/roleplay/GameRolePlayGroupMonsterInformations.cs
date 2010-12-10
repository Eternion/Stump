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
	
	public class GameRolePlayGroupMonsterInformations : GameRolePlayActorInformations
	{
		public const uint protocolId = 160;
		public int mainCreatureGenericId = 0;
		public uint mainCreaturelevel = 0;
		public List<MonsterInGroupInformations> underlings;
		public int ageBonus = 0;
		public int alignmentSide = 0;
		
		public GameRolePlayGroupMonsterInformations()
		{
			this.underlings = new List<MonsterInGroupInformations>();
		}
		
		public GameRolePlayGroupMonsterInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, int arg4, uint arg5, List<MonsterInGroupInformations> arg6, int arg7, int arg8)
			: this()
		{
			initGameRolePlayGroupMonsterInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}
		
		public override uint getTypeId()
		{
			return 160;
		}
		
		public GameRolePlayGroupMonsterInformations initGameRolePlayGroupMonsterInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, int arg4 = 0, uint arg5 = 0, List<MonsterInGroupInformations> arg6 = null, int arg7 = 0, int arg8 = 0)
		{
			base.initGameRolePlayActorInformations(arg1, arg2, arg3);
			this.mainCreatureGenericId = arg4;
			this.mainCreaturelevel = arg5;
			this.underlings = arg6;
			this.ageBonus = arg7;
			this.alignmentSide = arg8;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.mainCreatureGenericId = 0;
			this.mainCreaturelevel = 0;
			this.underlings = new List<MonsterInGroupInformations>();
			this.ageBonus = 0;
			this.alignmentSide = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameRolePlayGroupMonsterInformations(arg1);
		}
		
		public void serializeAs_GameRolePlayGroupMonsterInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameRolePlayActorInformations(arg1);
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
			if ( this.ageBonus < -1 || this.ageBonus > 1000 )
			{
				throw new Exception("Forbidden value (" + this.ageBonus + ") on element ageBonus.");
			}
			arg1.WriteShort((short)this.ageBonus);
			arg1.WriteByte((byte)this.alignmentSide);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayGroupMonsterInformations(arg1);
		}
		
		public void deserializeAs_GameRolePlayGroupMonsterInformations(BigEndianReader arg1)
		{
			object loc3 = null;
			base.deserialize(arg1);
			this.mainCreatureGenericId = (int)arg1.ReadInt();
			this.mainCreaturelevel = (uint)arg1.ReadShort();
			if ( this.mainCreaturelevel < 0 )
			{
				throw new Exception("Forbidden value (" + this.mainCreaturelevel + ") on element of GameRolePlayGroupMonsterInformations.mainCreaturelevel.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new MonsterInGroupInformations()) as MonsterInGroupInformations).deserialize(arg1);
				this.underlings.Add((MonsterInGroupInformations)loc3);
				++loc2;
			}
			this.ageBonus = (int)arg1.ReadShort();
			if ( this.ageBonus < -1 || this.ageBonus > 1000 )
			{
				throw new Exception("Forbidden value (" + this.ageBonus + ") on element of GameRolePlayGroupMonsterInformations.ageBonus.");
			}
			this.alignmentSide = (int)arg1.ReadByte();
		}
		
	}
}
