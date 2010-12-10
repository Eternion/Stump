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
	
	public class ActorRestrictionsInformations : Object
	{
		public const uint protocolId = 204;
		public Boolean cantBeAggressed = false;
		public Boolean cantBeChallenged = false;
		public Boolean cantTrade = false;
		public Boolean cantBeAttackedByMutant = false;
		public Boolean cantRun = false;
		public Boolean forceSlowWalk = false;
		public Boolean cantMinimize = false;
		public Boolean cantMove = false;
		public Boolean cantAggress = false;
		public Boolean cantChallenge = false;
		public Boolean cantExchange = false;
		public Boolean cantAttack = false;
		public Boolean cantChat = false;
		public Boolean cantBeMerchant = false;
		public Boolean cantUseObject = false;
		public Boolean cantUseTaxCollector = false;
		public Boolean cantUseInteractive = false;
		public Boolean cantSpeakToNPC = false;
		public Boolean cantChangeZone = false;
		public Boolean cantAttackMonster = false;
		public Boolean cantWalk8Directions = false;
		
		public ActorRestrictionsInformations()
		{
		}
		
		public ActorRestrictionsInformations(Boolean arg1, Boolean arg2, Boolean arg3, Boolean arg4, Boolean arg5, Boolean arg6, Boolean arg7, Boolean arg8, Boolean arg9, Boolean arg10, Boolean arg11, Boolean arg12, Boolean arg13, Boolean arg14, Boolean arg15, Boolean arg16, Boolean arg17, Boolean arg18, Boolean arg19, Boolean arg20, Boolean arg21)
			: this()
		{
			initActorRestrictionsInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21);
		}
		
		public virtual uint getTypeId()
		{
			return 204;
		}
		
		public ActorRestrictionsInformations initActorRestrictionsInformations(Boolean arg1 = false, Boolean arg2 = false, Boolean arg3 = false, Boolean arg4 = false, Boolean arg5 = false, Boolean arg6 = false, Boolean arg7 = false, Boolean arg8 = false, Boolean arg9 = false, Boolean arg10 = false, Boolean arg11 = false, Boolean arg12 = false, Boolean arg13 = false, Boolean arg14 = false, Boolean arg15 = false, Boolean arg16 = false, Boolean arg17 = false, Boolean arg18 = false, Boolean arg19 = false, Boolean arg20 = false, Boolean arg21 = false)
		{
			this.cantBeAggressed = arg1;
			this.cantBeChallenged = arg2;
			this.cantTrade = arg3;
			this.cantBeAttackedByMutant = arg4;
			this.cantRun = arg5;
			this.forceSlowWalk = arg6;
			this.cantMinimize = arg7;
			this.cantMove = arg8;
			this.cantAggress = arg9;
			this.cantChallenge = arg10;
			this.cantExchange = arg11;
			this.cantAttack = arg12;
			this.cantChat = arg13;
			this.cantBeMerchant = arg14;
			this.cantUseObject = arg15;
			this.cantUseTaxCollector = arg16;
			this.cantUseInteractive = arg17;
			this.cantSpeakToNPC = arg18;
			this.cantChangeZone = arg19;
			this.cantAttackMonster = arg20;
			this.cantWalk8Directions = arg21;
			return this;
		}
		
		public virtual void reset()
		{
			this.cantBeAggressed = false;
			this.cantBeChallenged = false;
			this.cantTrade = false;
			this.cantBeAttackedByMutant = false;
			this.cantRun = false;
			this.forceSlowWalk = false;
			this.cantMinimize = false;
			this.cantMove = false;
			this.cantAggress = false;
			this.cantChallenge = false;
			this.cantExchange = false;
			this.cantAttack = false;
			this.cantChat = false;
			this.cantBeMerchant = false;
			this.cantUseObject = false;
			this.cantUseTaxCollector = false;
			this.cantUseInteractive = false;
			this.cantSpeakToNPC = false;
			this.cantChangeZone = false;
			this.cantAttackMonster = false;
			this.cantWalk8Directions = false;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ActorRestrictionsInformations(arg1);
		}
		
		public void serializeAs_ActorRestrictionsInformations(BigEndianWriter arg1)
		{
			var loc1 = 0;
			BooleanByteWrapper.SetFlag(loc1, 0, this.cantBeAggressed);
			BooleanByteWrapper.SetFlag(loc1, 1, this.cantBeChallenged);
			BooleanByteWrapper.SetFlag(loc1, 2, this.cantTrade);
			BooleanByteWrapper.SetFlag(loc1, 3, this.cantBeAttackedByMutant);
			BooleanByteWrapper.SetFlag(loc1, 4, this.cantRun);
			BooleanByteWrapper.SetFlag(loc1, 5, this.forceSlowWalk);
			BooleanByteWrapper.SetFlag(loc1, 6, this.cantMinimize);
			BooleanByteWrapper.SetFlag(loc1, 7, this.cantMove);
			arg1.WriteByte((byte)loc1);
			var loc2 = 0;
			BooleanByteWrapper.SetFlag(loc2, 0, this.cantAggress);
			BooleanByteWrapper.SetFlag(loc2, 1, this.cantChallenge);
			BooleanByteWrapper.SetFlag(loc2, 2, this.cantExchange);
			BooleanByteWrapper.SetFlag(loc2, 3, this.cantAttack);
			BooleanByteWrapper.SetFlag(loc2, 4, this.cantChat);
			BooleanByteWrapper.SetFlag(loc2, 5, this.cantBeMerchant);
			BooleanByteWrapper.SetFlag(loc2, 6, this.cantUseObject);
			BooleanByteWrapper.SetFlag(loc2, 7, this.cantUseTaxCollector);
			arg1.WriteByte((byte)loc2);
			var loc3 = 0;
			BooleanByteWrapper.SetFlag(loc3, 0, this.cantUseInteractive);
			BooleanByteWrapper.SetFlag(loc3, 1, this.cantSpeakToNPC);
			BooleanByteWrapper.SetFlag(loc3, 2, this.cantChangeZone);
			BooleanByteWrapper.SetFlag(loc3, 3, this.cantAttackMonster);
			BooleanByteWrapper.SetFlag(loc3, 4, this.cantWalk8Directions);
			arg1.WriteByte((byte)loc3);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ActorRestrictionsInformations(arg1);
		}
		
		public void deserializeAs_ActorRestrictionsInformations(BigEndianReader arg1)
		{
			var loc1 = arg1.ReadByte();
			this.cantBeAggressed = (Boolean)BooleanByteWrapper.GetFlag(loc1, 0);
			this.cantBeChallenged = (Boolean)BooleanByteWrapper.GetFlag(loc1, 1);
			this.cantTrade = (Boolean)BooleanByteWrapper.GetFlag(loc1, 2);
			this.cantBeAttackedByMutant = (Boolean)BooleanByteWrapper.GetFlag(loc1, 3);
			this.cantRun = (Boolean)BooleanByteWrapper.GetFlag(loc1, 4);
			this.forceSlowWalk = (Boolean)BooleanByteWrapper.GetFlag(loc1, 5);
			this.cantMinimize = (Boolean)BooleanByteWrapper.GetFlag(loc1, 6);
			this.cantMove = (Boolean)BooleanByteWrapper.GetFlag(loc1, 7);
			var loc2 = arg1.ReadByte();
			this.cantAggress = (Boolean)BooleanByteWrapper.GetFlag(loc2, 0);
			this.cantChallenge = (Boolean)BooleanByteWrapper.GetFlag(loc2, 1);
			this.cantExchange = (Boolean)BooleanByteWrapper.GetFlag(loc2, 2);
			this.cantAttack = (Boolean)BooleanByteWrapper.GetFlag(loc2, 3);
			this.cantChat = (Boolean)BooleanByteWrapper.GetFlag(loc2, 4);
			this.cantBeMerchant = (Boolean)BooleanByteWrapper.GetFlag(loc2, 5);
			this.cantUseObject = (Boolean)BooleanByteWrapper.GetFlag(loc2, 6);
			this.cantUseTaxCollector = (Boolean)BooleanByteWrapper.GetFlag(loc2, 7);
			var loc3 = arg1.ReadByte();
			this.cantUseInteractive = (Boolean)BooleanByteWrapper.GetFlag(loc3, 0);
			this.cantSpeakToNPC = (Boolean)BooleanByteWrapper.GetFlag(loc3, 1);
			this.cantChangeZone = (Boolean)BooleanByteWrapper.GetFlag(loc3, 2);
			this.cantAttackMonster = (Boolean)BooleanByteWrapper.GetFlag(loc3, 3);
			this.cantWalk8Directions = (Boolean)BooleanByteWrapper.GetFlag(loc3, 4);
		}
		
	}
}
