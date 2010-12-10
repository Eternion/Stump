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
	
	public class AbstractGameActionFightTargetedAbilityMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 6118;
		internal Boolean _isInitialized = false;
		public int destinationCellId = 0;
		public uint critical = 1;
		public Boolean silentCast = false;
		
		public AbstractGameActionFightTargetedAbilityMessage()
		{
		}
		
		public AbstractGameActionFightTargetedAbilityMessage(uint arg1, int arg2, int arg3, uint arg4, Boolean arg5)
			: this()
		{
			initAbstractGameActionFightTargetedAbilityMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 6118;
		}
		
		public AbstractGameActionFightTargetedAbilityMessage initAbstractGameActionFightTargetedAbilityMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 1, Boolean arg5 = false)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.destinationCellId = arg3;
			this.critical = arg4;
			this.silentCast = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.destinationCellId = 0;
			this.critical = 1;
			this.silentCast = false;
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
			this.serializeAs_AbstractGameActionFightTargetedAbilityMessage(arg1);
		}
		
		public void serializeAs_AbstractGameActionFightTargetedAbilityMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			if ( this.destinationCellId < -1 || this.destinationCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.destinationCellId + ") on element destinationCellId.");
			}
			arg1.WriteShort((short)this.destinationCellId);
			arg1.WriteByte((byte)this.critical);
			arg1.WriteBoolean(this.silentCast);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AbstractGameActionFightTargetedAbilityMessage(arg1);
		}
		
		public void deserializeAs_AbstractGameActionFightTargetedAbilityMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.destinationCellId = (int)arg1.ReadShort();
			if ( this.destinationCellId < -1 || this.destinationCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.destinationCellId + ") on element of AbstractGameActionFightTargetedAbilityMessage.destinationCellId.");
			}
			this.critical = (uint)arg1.ReadByte();
			if ( this.critical < 0 )
			{
				throw new Exception("Forbidden value (" + this.critical + ") on element of AbstractGameActionFightTargetedAbilityMessage.critical.");
			}
			this.silentCast = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
