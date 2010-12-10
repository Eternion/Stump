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
	
	public class CompassUpdatePartyMemberMessage : CompassUpdateMessage
	{
		public const uint protocolId = 5589;
		internal Boolean _isInitialized = false;
		public uint memberId = 0;
		
		public CompassUpdatePartyMemberMessage()
		{
		}
		
		public CompassUpdatePartyMemberMessage(uint arg1, int arg2, int arg3, uint arg4)
			: this()
		{
			initCompassUpdatePartyMemberMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5589;
		}
		
		public CompassUpdatePartyMemberMessage initCompassUpdatePartyMemberMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 0)
		{
			base.initCompassUpdateMessage(arg1, arg2, arg3);
			this.memberId = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.memberId = 0;
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
			this.serializeAs_CompassUpdatePartyMemberMessage(arg1);
		}
		
		public void serializeAs_CompassUpdatePartyMemberMessage(BigEndianWriter arg1)
		{
			base.serializeAs_CompassUpdateMessage(arg1);
			if ( this.memberId < 0 )
			{
				throw new Exception("Forbidden value (" + this.memberId + ") on element memberId.");
			}
			arg1.WriteInt((int)this.memberId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CompassUpdatePartyMemberMessage(arg1);
		}
		
		public void deserializeAs_CompassUpdatePartyMemberMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.memberId = (uint)arg1.ReadInt();
			if ( this.memberId < 0 )
			{
				throw new Exception("Forbidden value (" + this.memberId + ") on element of CompassUpdatePartyMemberMessage.memberId.");
			}
		}
		
	}
}
