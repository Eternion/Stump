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
	
	public class NpcGenericActionRequestMessage : Message
	{
		public const uint protocolId = 5898;
		internal Boolean _isInitialized = false;
		public int npcId = 0;
		public uint npcActionId = 0;
		public int npcMapId = 0;
		
		public NpcGenericActionRequestMessage()
		{
		}
		
		public NpcGenericActionRequestMessage(int arg1, uint arg2, int arg3)
			: this()
		{
			initNpcGenericActionRequestMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5898;
		}
		
		public NpcGenericActionRequestMessage initNpcGenericActionRequestMessage(int arg1 = 0, uint arg2 = 0, int arg3 = 0)
		{
			this.npcId = arg1;
			this.npcActionId = arg2;
			this.npcMapId = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.npcId = 0;
			this.npcActionId = 0;
			this.npcMapId = 0;
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
			this.serializeAs_NpcGenericActionRequestMessage(arg1);
		}
		
		public void serializeAs_NpcGenericActionRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.npcId);
			if ( this.npcActionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.npcActionId + ") on element npcActionId.");
			}
			arg1.WriteByte((byte)this.npcActionId);
			arg1.WriteInt((int)this.npcMapId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_NpcGenericActionRequestMessage(arg1);
		}
		
		public void deserializeAs_NpcGenericActionRequestMessage(BigEndianReader arg1)
		{
			this.npcId = (int)arg1.ReadInt();
			this.npcActionId = (uint)arg1.ReadByte();
			if ( this.npcActionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.npcActionId + ") on element of NpcGenericActionRequestMessage.npcActionId.");
			}
			this.npcMapId = (int)arg1.ReadInt();
		}
		
	}
}
