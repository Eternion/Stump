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
	
	public class AccountCapabilitiesMessage : Message
	{
		public const uint protocolId = 6216;
		internal Boolean _isInitialized = false;
		public int accountId = 0;
		public Boolean tutorialAvailable = false;
		public uint breedsVisible = 0;
		public uint breedsAvailable = 0;
		
		public AccountCapabilitiesMessage()
		{
		}
		
		public AccountCapabilitiesMessage(int arg1, Boolean arg2, uint arg3, uint arg4)
			: this()
		{
			initAccountCapabilitiesMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 6216;
		}
		
		public AccountCapabilitiesMessage initAccountCapabilitiesMessage(int arg1 = 0, Boolean arg2 = false, uint arg3 = 0, uint arg4 = 0)
		{
			this.accountId = arg1;
			this.tutorialAvailable = arg2;
			this.breedsVisible = arg3;
			this.breedsAvailable = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.accountId = 0;
			this.tutorialAvailable = false;
			this.breedsVisible = 0;
			this.breedsAvailable = 0;
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
			this.serializeAs_AccountCapabilitiesMessage(arg1);
		}
		
		public void serializeAs_AccountCapabilitiesMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.accountId);
			arg1.WriteBoolean(this.tutorialAvailable);
			if ( this.breedsVisible < 0 )
			{
				throw new Exception("Forbidden value (" + this.breedsVisible + ") on element breedsVisible.");
			}
			arg1.WriteShort((short)this.breedsVisible);
			if ( this.breedsAvailable < 0 )
			{
				throw new Exception("Forbidden value (" + this.breedsAvailable + ") on element breedsAvailable.");
			}
			arg1.WriteShort((short)this.breedsAvailable);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AccountCapabilitiesMessage(arg1);
		}
		
		public void deserializeAs_AccountCapabilitiesMessage(BigEndianReader arg1)
		{
			this.accountId = (int)arg1.ReadInt();
			this.tutorialAvailable = (Boolean)arg1.ReadBoolean();
			this.breedsVisible = (uint)arg1.ReadShort();
			if ( this.breedsVisible < 0 )
			{
				throw new Exception("Forbidden value (" + this.breedsVisible + ") on element of AccountCapabilitiesMessage.breedsVisible.");
			}
			this.breedsAvailable = (uint)arg1.ReadShort();
			if ( this.breedsAvailable < 0 )
			{
				throw new Exception("Forbidden value (" + this.breedsAvailable + ") on element of AccountCapabilitiesMessage.breedsAvailable.");
			}
		}
		
	}
}
