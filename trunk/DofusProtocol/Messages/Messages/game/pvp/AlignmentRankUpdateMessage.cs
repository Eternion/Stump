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
	
	public class AlignmentRankUpdateMessage : Message
	{
		public const uint protocolId = 6058;
		internal Boolean _isInitialized = false;
		public uint alignmentRank = 0;
		public Boolean verbose = false;
		
		public AlignmentRankUpdateMessage()
		{
		}
		
		public AlignmentRankUpdateMessage(uint arg1, Boolean arg2)
			: this()
		{
			initAlignmentRankUpdateMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6058;
		}
		
		public AlignmentRankUpdateMessage initAlignmentRankUpdateMessage(uint arg1 = 0, Boolean arg2 = false)
		{
			this.alignmentRank = arg1;
			this.verbose = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.alignmentRank = 0;
			this.verbose = false;
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
			this.serializeAs_AlignmentRankUpdateMessage(arg1);
		}
		
		public void serializeAs_AlignmentRankUpdateMessage(BigEndianWriter arg1)
		{
			if ( this.alignmentRank < 0 )
			{
				throw new Exception("Forbidden value (" + this.alignmentRank + ") on element alignmentRank.");
			}
			arg1.WriteByte((byte)this.alignmentRank);
			arg1.WriteBoolean(this.verbose);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AlignmentRankUpdateMessage(arg1);
		}
		
		public void deserializeAs_AlignmentRankUpdateMessage(BigEndianReader arg1)
		{
			this.alignmentRank = (uint)arg1.ReadByte();
			if ( this.alignmentRank < 0 )
			{
				throw new Exception("Forbidden value (" + this.alignmentRank + ") on element of AlignmentRankUpdateMessage.alignmentRank.");
			}
			this.verbose = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
