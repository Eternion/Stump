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
	
	public class CharacterReportMessage : Message
	{
		public const uint protocolId = 6079;
		internal Boolean _isInitialized = false;
		public uint reportedId = 0;
		public uint reason = 0;
		
		public CharacterReportMessage()
		{
		}
		
		public CharacterReportMessage(uint arg1, uint arg2)
			: this()
		{
			initCharacterReportMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6079;
		}
		
		public CharacterReportMessage initCharacterReportMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.reportedId = arg1;
			this.reason = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.reportedId = 0;
			this.reason = 0;
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
			this.serializeAs_CharacterReportMessage(arg1);
		}
		
		public void serializeAs_CharacterReportMessage(BigEndianWriter arg1)
		{
			if ( this.reportedId < 0 || this.reportedId > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.reportedId + ") on element reportedId.");
			}
			arg1.WriteUInt((uint)this.reportedId);
			if ( this.reason < 0 )
			{
				throw new Exception("Forbidden value (" + this.reason + ") on element reason.");
			}
			arg1.WriteByte((byte)this.reason);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterReportMessage(arg1);
		}
		
		public void deserializeAs_CharacterReportMessage(BigEndianReader arg1)
		{
			this.reportedId = (uint)arg1.ReadUInt();
			if ( this.reportedId < 0 || this.reportedId > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.reportedId + ") on element of CharacterReportMessage.reportedId.");
			}
			this.reason = (uint)arg1.ReadByte();
			if ( this.reason < 0 )
			{
				throw new Exception("Forbidden value (" + this.reason + ") on element of CharacterReportMessage.reason.");
			}
		}
		
	}
}
