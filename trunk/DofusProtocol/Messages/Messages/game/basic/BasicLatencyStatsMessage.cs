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
	
	public class BasicLatencyStatsMessage : Message
	{
		public const uint protocolId = 5663;
		internal Boolean _isInitialized = false;
		public uint latency = 0;
		public uint sampleCount = 0;
		public uint max = 0;
		
		public BasicLatencyStatsMessage()
		{
		}
		
		public BasicLatencyStatsMessage(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initBasicLatencyStatsMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5663;
		}
		
		public BasicLatencyStatsMessage initBasicLatencyStatsMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			this.latency = arg1;
			this.sampleCount = arg2;
			this.max = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.latency = 0;
			this.sampleCount = 0;
			this.max = 0;
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
			this.serializeAs_BasicLatencyStatsMessage(arg1);
		}
		
		public void serializeAs_BasicLatencyStatsMessage(BigEndianWriter arg1)
		{
			if ( this.latency < 0 || this.latency > 65535 )
			{
				throw new Exception("Forbidden value (" + this.latency + ") on element latency.");
			}
			arg1.WriteShort((short)this.latency);
			if ( this.sampleCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.sampleCount + ") on element sampleCount.");
			}
			arg1.WriteShort((short)this.sampleCount);
			if ( this.max < 0 )
			{
				throw new Exception("Forbidden value (" + this.max + ") on element max.");
			}
			arg1.WriteShort((short)this.max);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_BasicLatencyStatsMessage(arg1);
		}
		
		public void deserializeAs_BasicLatencyStatsMessage(BigEndianReader arg1)
		{
			this.latency = (uint)arg1.ReadUShort();
			if ( this.latency < 0 || this.latency > 65535 )
			{
				throw new Exception("Forbidden value (" + this.latency + ") on element of BasicLatencyStatsMessage.latency.");
			}
			this.sampleCount = (uint)arg1.ReadShort();
			if ( this.sampleCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.sampleCount + ") on element of BasicLatencyStatsMessage.sampleCount.");
			}
			this.max = (uint)arg1.ReadShort();
			if ( this.max < 0 )
			{
				throw new Exception("Forbidden value (" + this.max + ") on element of BasicLatencyStatsMessage.max.");
			}
		}
		
	}
}
