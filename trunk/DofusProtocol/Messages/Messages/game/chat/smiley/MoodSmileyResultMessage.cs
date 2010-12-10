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
	
	public class MoodSmileyResultMessage : Message
	{
		public const uint protocolId = 6196;
		internal Boolean _isInitialized = false;
		public uint resultCode = 1;
		public int smileyId = 0;
		
		public MoodSmileyResultMessage()
		{
		}
		
		public MoodSmileyResultMessage(uint arg1, int arg2)
			: this()
		{
			initMoodSmileyResultMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6196;
		}
		
		public MoodSmileyResultMessage initMoodSmileyResultMessage(uint arg1 = 1, int arg2 = 0)
		{
			this.resultCode = arg1;
			this.smileyId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.resultCode = 1;
			this.smileyId = 0;
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
			this.serializeAs_MoodSmileyResultMessage(arg1);
		}
		
		public void serializeAs_MoodSmileyResultMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.resultCode);
			arg1.WriteByte((byte)this.smileyId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MoodSmileyResultMessage(arg1);
		}
		
		public void deserializeAs_MoodSmileyResultMessage(BigEndianReader arg1)
		{
			this.resultCode = (uint)arg1.ReadByte();
			if ( this.resultCode < 0 )
			{
				throw new Exception("Forbidden value (" + this.resultCode + ") on element of MoodSmileyResultMessage.resultCode.");
			}
			this.smileyId = (int)arg1.ReadByte();
		}
		
	}
}
