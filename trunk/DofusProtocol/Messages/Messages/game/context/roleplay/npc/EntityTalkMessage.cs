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
	
	public class EntityTalkMessage : Message
	{
		public const uint protocolId = 6110;
		internal Boolean _isInitialized = false;
		public int entityId = 0;
		public uint textId = 0;
		public List<String> parameters;
		
		public EntityTalkMessage()
		{
			this.parameters = new List<String>();
		}
		
		public EntityTalkMessage(int arg1, uint arg2, List<String> arg3)
			: this()
		{
			initEntityTalkMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6110;
		}
		
		public EntityTalkMessage initEntityTalkMessage(int arg1 = 0, uint arg2 = 0, List<String> arg3 = null)
		{
			this.entityId = arg1;
			this.textId = arg2;
			this.parameters = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.entityId = 0;
			this.textId = 0;
			this.parameters = new List<String>();
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
			this.serializeAs_EntityTalkMessage(arg1);
		}
		
		public void serializeAs_EntityTalkMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.entityId);
			if ( this.textId < 0 )
			{
				throw new Exception("Forbidden value (" + this.textId + ") on element textId.");
			}
			arg1.WriteShort((short)this.textId);
			arg1.WriteShort((short)this.parameters.Count);
			var loc1 = 0;
			while ( loc1 < this.parameters.Count )
			{
				arg1.WriteUTF((string)this.parameters[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_EntityTalkMessage(arg1);
		}
		
		public void deserializeAs_EntityTalkMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.entityId = (int)arg1.ReadInt();
			this.textId = (uint)arg1.ReadShort();
			if ( this.textId < 0 )
			{
				throw new Exception("Forbidden value (" + this.textId + ") on element of EntityTalkMessage.textId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = arg1.ReadUTF();
				this.parameters.Add((String)loc3);
				++loc2;
			}
		}
		
	}
}
