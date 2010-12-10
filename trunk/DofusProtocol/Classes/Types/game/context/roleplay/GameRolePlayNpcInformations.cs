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
	
	public class GameRolePlayNpcInformations : GameRolePlayActorInformations
	{
		public const uint protocolId = 156;
		public uint npcId = 0;
		public Boolean sex = false;
		public uint specialArtworkId = 0;
		public Boolean canGiveQuest = false;
		
		public GameRolePlayNpcInformations()
		{
		}
		
		public GameRolePlayNpcInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, uint arg4, Boolean arg5, uint arg6, Boolean arg7)
			: this()
		{
			initGameRolePlayNpcInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getTypeId()
		{
			return 156;
		}
		
		public GameRolePlayNpcInformations initGameRolePlayNpcInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, uint arg4 = 0, Boolean arg5 = false, uint arg6 = 0, Boolean arg7 = false)
		{
			base.initGameRolePlayActorInformations(arg1, arg2, arg3);
			this.npcId = arg4;
			this.sex = arg5;
			this.specialArtworkId = arg6;
			this.canGiveQuest = arg7;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.npcId = 0;
			this.sex = false;
			this.specialArtworkId = 0;
			this.canGiveQuest = false;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameRolePlayNpcInformations(arg1);
		}
		
		public void serializeAs_GameRolePlayNpcInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameRolePlayActorInformations(arg1);
			if ( this.npcId < 0 )
			{
				throw new Exception("Forbidden value (" + this.npcId + ") on element npcId.");
			}
			arg1.WriteShort((short)this.npcId);
			arg1.WriteBoolean(this.sex);
			if ( this.specialArtworkId < 0 )
			{
				throw new Exception("Forbidden value (" + this.specialArtworkId + ") on element specialArtworkId.");
			}
			arg1.WriteShort((short)this.specialArtworkId);
			arg1.WriteBoolean(this.canGiveQuest);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayNpcInformations(arg1);
		}
		
		public void deserializeAs_GameRolePlayNpcInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.npcId = (uint)arg1.ReadShort();
			if ( this.npcId < 0 )
			{
				throw new Exception("Forbidden value (" + this.npcId + ") on element of GameRolePlayNpcInformations.npcId.");
			}
			this.sex = (Boolean)arg1.ReadBoolean();
			this.specialArtworkId = (uint)arg1.ReadShort();
			if ( this.specialArtworkId < 0 )
			{
				throw new Exception("Forbidden value (" + this.specialArtworkId + ") on element of GameRolePlayNpcInformations.specialArtworkId.");
			}
			this.canGiveQuest = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
