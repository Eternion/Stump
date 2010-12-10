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
	
	public class GameActionFightStateChangeMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 5569;
		internal Boolean _isInitialized = false;
		public int targetId = 0;
		public int stateId = 0;
		public Boolean active = false;
		
		public GameActionFightStateChangeMessage()
		{
		}
		
		public GameActionFightStateChangeMessage(uint arg1, int arg2, int arg3, int arg4, Boolean arg5)
			: this()
		{
			initGameActionFightStateChangeMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 5569;
		}
		
		public GameActionFightStateChangeMessage initGameActionFightStateChangeMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, int arg4 = 0, Boolean arg5 = false)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.targetId = arg3;
			this.stateId = arg4;
			this.active = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.targetId = 0;
			this.stateId = 0;
			this.active = false;
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
			this.serializeAs_GameActionFightStateChangeMessage(arg1);
		}
		
		public void serializeAs_GameActionFightStateChangeMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			arg1.WriteInt((int)this.targetId);
			arg1.WriteShort((short)this.stateId);
			arg1.WriteBoolean(this.active);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightStateChangeMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightStateChangeMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.targetId = (int)arg1.ReadInt();
			this.stateId = (int)arg1.ReadShort();
			this.active = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
