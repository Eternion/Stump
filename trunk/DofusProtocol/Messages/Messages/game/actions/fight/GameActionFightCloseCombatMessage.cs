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
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightCloseCombatMessage : AbstractGameActionFightTargetedAbilityMessage
	{
		public const uint protocolId = 6116;
		internal Boolean _isInitialized = false;
		public uint weaponGenericId = 0;
		
		public GameActionFightCloseCombatMessage()
		{
		}
		
		public GameActionFightCloseCombatMessage(uint arg1, int arg2, int arg3, uint arg4, Boolean arg5, uint arg6)
			: this()
		{
			initGameActionFightCloseCombatMessage(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getMessageId()
		{
			return 6116;
		}
		
		public GameActionFightCloseCombatMessage initGameActionFightCloseCombatMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 1, Boolean arg5 = false, uint arg6 = 0)
		{
			base.initAbstractGameActionFightTargetedAbilityMessage(arg1, arg2, arg3, arg4, arg5);
			this.weaponGenericId = arg6;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.weaponGenericId = 0;
			this._isInitialized = false;
		}
		
		public override void pack(BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack(BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameActionFightCloseCombatMessage(arg1);
		}
		
		public void serializeAs_GameActionFightCloseCombatMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionFightTargetedAbilityMessage(arg1);
			if ( this.weaponGenericId < 0 )
			{
				throw new Exception("Forbidden value (" + this.weaponGenericId + ") on element weaponGenericId.");
			}
			arg1.WriteInt((int)this.weaponGenericId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightCloseCombatMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightCloseCombatMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.weaponGenericId = (uint)arg1.ReadInt();
			if ( this.weaponGenericId < 0 )
			{
				throw new Exception("Forbidden value (" + this.weaponGenericId + ") on element of GameActionFightCloseCombatMessage.weaponGenericId.");
			}
		}
		
	}
}
