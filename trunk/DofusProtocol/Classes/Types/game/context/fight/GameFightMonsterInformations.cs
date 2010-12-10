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
	
	public class GameFightMonsterInformations : GameFightAIInformations
	{
		public const uint protocolId = 29;
		public uint creatureGenericId = 0;
		public uint creatureGrade = 0;
		
		public GameFightMonsterInformations()
		{
		}
		
		public GameFightMonsterInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, uint arg4, Boolean arg5, GameFightMinimalStats arg6, uint arg7, uint arg8)
			: this()
		{
			initGameFightMonsterInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}
		
		public override uint getTypeId()
		{
			return 29;
		}
		
		public GameFightMonsterInformations initGameFightMonsterInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, uint arg4 = 2, Boolean arg5 = false, GameFightMinimalStats arg6 = null, uint arg7 = 0, uint arg8 = 0)
		{
			base.initGameFightAIInformations(arg1, arg2, arg3, arg4, arg5, arg6);
			this.creatureGenericId = arg7;
			this.creatureGrade = arg8;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.creatureGenericId = 0;
			this.creatureGrade = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameFightMonsterInformations(arg1);
		}
		
		public void serializeAs_GameFightMonsterInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameFightAIInformations(arg1);
			if ( this.creatureGenericId < 0 )
			{
				throw new Exception("Forbidden value (" + this.creatureGenericId + ") on element creatureGenericId.");
			}
			arg1.WriteShort((short)this.creatureGenericId);
			if ( this.creatureGrade < 0 )
			{
				throw new Exception("Forbidden value (" + this.creatureGrade + ") on element creatureGrade.");
			}
			arg1.WriteByte((byte)this.creatureGrade);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightMonsterInformations(arg1);
		}
		
		public void deserializeAs_GameFightMonsterInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.creatureGenericId = (uint)arg1.ReadShort();
			if ( this.creatureGenericId < 0 )
			{
				throw new Exception("Forbidden value (" + this.creatureGenericId + ") on element of GameFightMonsterInformations.creatureGenericId.");
			}
			this.creatureGrade = (uint)arg1.ReadByte();
			if ( this.creatureGrade < 0 )
			{
				throw new Exception("Forbidden value (" + this.creatureGrade + ") on element of GameFightMonsterInformations.creatureGrade.");
			}
		}
		
	}
}
