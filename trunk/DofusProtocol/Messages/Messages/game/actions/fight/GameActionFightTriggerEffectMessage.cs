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
	
	public class GameActionFightTriggerEffectMessage : GameActionFightDispellEffectMessage
	{
		public const uint protocolId = 6147;
		internal Boolean _isInitialized = false;
		
		public GameActionFightTriggerEffectMessage()
		{
		}
		
		public GameActionFightTriggerEffectMessage(uint arg1, int arg2, int arg3, uint arg4)
			: this()
		{
			initGameActionFightTriggerEffectMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 6147;
		}
		
		public GameActionFightTriggerEffectMessage initGameActionFightTriggerEffectMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 0)
		{
			base.initGameActionFightDispellEffectMessage(arg1, arg2, arg3, arg4);
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
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
			this.serializeAs_GameActionFightTriggerEffectMessage(arg1);
		}
		
		public void serializeAs_GameActionFightTriggerEffectMessage(BigEndianWriter arg1)
		{
			base.serializeAs_GameActionFightDispellEffectMessage(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightTriggerEffectMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightTriggerEffectMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
		}
		
	}
}
