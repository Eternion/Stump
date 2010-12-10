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
	
	public class GameActionFightSlideMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 5525;
		internal Boolean _isInitialized = false;
		public int targetId = 0;
		public int startCellId = 0;
		public int endCellId = 0;
		
		public GameActionFightSlideMessage()
		{
		}
		
		public GameActionFightSlideMessage(uint arg1, int arg2, int arg3, int arg4, int arg5)
			: this()
		{
			initGameActionFightSlideMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 5525;
		}
		
		public GameActionFightSlideMessage initGameActionFightSlideMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, int arg4 = 0, int arg5 = 0)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.targetId = arg3;
			this.startCellId = arg4;
			this.endCellId = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.targetId = 0;
			this.startCellId = 0;
			this.endCellId = 0;
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
			this.serializeAs_GameActionFightSlideMessage(arg1);
		}
		
		public void serializeAs_GameActionFightSlideMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			arg1.WriteInt((int)this.targetId);
			if ( this.startCellId < -1 || this.startCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.startCellId + ") on element startCellId.");
			}
			arg1.WriteShort((short)this.startCellId);
			if ( this.endCellId < -1 || this.endCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.endCellId + ") on element endCellId.");
			}
			arg1.WriteShort((short)this.endCellId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightSlideMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightSlideMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.targetId = (int)arg1.ReadInt();
			this.startCellId = (int)arg1.ReadShort();
			if ( this.startCellId < -1 || this.startCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.startCellId + ") on element of GameActionFightSlideMessage.startCellId.");
			}
			this.endCellId = (int)arg1.ReadShort();
			if ( this.endCellId < -1 || this.endCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.endCellId + ") on element of GameActionFightSlideMessage.endCellId.");
			}
		}
		
	}
}
