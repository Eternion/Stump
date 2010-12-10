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
	
	public class GameContextMoveMultipleElementsMessage : Message
	{
		public const uint protocolId = 254;
		internal Boolean _isInitialized = false;
		public List<EntityMovementInformations> movements;
		
		public GameContextMoveMultipleElementsMessage()
		{
			this.movements = new List<EntityMovementInformations>();
		}
		
		public GameContextMoveMultipleElementsMessage(List<EntityMovementInformations> arg1)
			: this()
		{
			initGameContextMoveMultipleElementsMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 254;
		}
		
		public GameContextMoveMultipleElementsMessage initGameContextMoveMultipleElementsMessage(List<EntityMovementInformations> arg1)
		{
			this.movements = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.movements = new List<EntityMovementInformations>();
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
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameContextMoveMultipleElementsMessage(arg1);
		}
		
		public void serializeAs_GameContextMoveMultipleElementsMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.movements.Count);
			var loc1 = 0;
			while ( loc1 < this.movements.Count )
			{
				this.movements[loc1].serializeAs_EntityMovementInformations(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameContextMoveMultipleElementsMessage(arg1);
		}
		
		public void deserializeAs_GameContextMoveMultipleElementsMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new EntityMovementInformations()) as EntityMovementInformations).deserialize(arg1);
				this.movements.Add((EntityMovementInformations)loc3);
				++loc2;
			}
		}
		
	}
}
