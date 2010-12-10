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
	
	public class GameContextRefreshEntityLookMessage : Message
	{
		public const uint protocolId = 5637;
		internal Boolean _isInitialized = false;
		public int id = 0;
		public EntityLook look;
		
		public GameContextRefreshEntityLookMessage()
		{
			this.look = new EntityLook();
		}
		
		public GameContextRefreshEntityLookMessage(int arg1, EntityLook arg2)
			: this()
		{
			initGameContextRefreshEntityLookMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5637;
		}
		
		public GameContextRefreshEntityLookMessage initGameContextRefreshEntityLookMessage(int arg1 = 0, EntityLook arg2 = null)
		{
			this.id = arg1;
			this.look = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.id = 0;
			this.look = new EntityLook();
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
			this.serializeAs_GameContextRefreshEntityLookMessage(arg1);
		}
		
		public void serializeAs_GameContextRefreshEntityLookMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.id);
			this.look.serializeAs_EntityLook(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameContextRefreshEntityLookMessage(arg1);
		}
		
		public void deserializeAs_GameContextRefreshEntityLookMessage(BigEndianReader arg1)
		{
			this.id = (int)arg1.ReadInt();
			this.look = new EntityLook();
			this.look.deserialize(arg1);
		}
		
	}
}
