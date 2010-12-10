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
	
	public class AlignmentAreaUpdateMessage : Message
	{
		public const uint protocolId = 6060;
		internal Boolean _isInitialized = false;
		public uint areaId = 0;
		public int side = 0;
		
		public AlignmentAreaUpdateMessage()
		{
		}
		
		public AlignmentAreaUpdateMessage(uint arg1, int arg2)
			: this()
		{
			initAlignmentAreaUpdateMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6060;
		}
		
		public AlignmentAreaUpdateMessage initAlignmentAreaUpdateMessage(uint arg1 = 0, int arg2 = 0)
		{
			this.areaId = arg1;
			this.side = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.areaId = 0;
			this.side = 0;
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
			this.serializeAs_AlignmentAreaUpdateMessage(arg1);
		}
		
		public void serializeAs_AlignmentAreaUpdateMessage(BigEndianWriter arg1)
		{
			if ( this.areaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.areaId + ") on element areaId.");
			}
			arg1.WriteShort((short)this.areaId);
			arg1.WriteByte((byte)this.side);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AlignmentAreaUpdateMessage(arg1);
		}
		
		public void deserializeAs_AlignmentAreaUpdateMessage(BigEndianReader arg1)
		{
			this.areaId = (uint)arg1.ReadShort();
			if ( this.areaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.areaId + ") on element of AlignmentAreaUpdateMessage.areaId.");
			}
			this.side = (int)arg1.ReadByte();
		}
		
	}
}
