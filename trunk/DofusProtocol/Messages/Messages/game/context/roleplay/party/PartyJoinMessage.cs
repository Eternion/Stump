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
	
	public class PartyJoinMessage : Message
	{
		public const uint protocolId = 5576;
		internal Boolean _isInitialized = false;
		public uint partyLeaderId = 0;
		public List<PartyMemberInformations> members;
		public Boolean restricted = false;
		
		public PartyJoinMessage()
		{
			this.members = new List<PartyMemberInformations>();
		}
		
		public PartyJoinMessage(uint arg1, List<PartyMemberInformations> arg2, Boolean arg3)
			: this()
		{
			initPartyJoinMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5576;
		}
		
		public PartyJoinMessage initPartyJoinMessage(uint arg1 = 0, List<PartyMemberInformations> arg2 = null, Boolean arg3 = false)
		{
			this.partyLeaderId = arg1;
			this.members = arg2;
			this.restricted = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.partyLeaderId = 0;
			this.members = new List<PartyMemberInformations>();
			this.restricted = false;
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
			this.serializeAs_PartyJoinMessage(arg1);
		}
		
		public void serializeAs_PartyJoinMessage(BigEndianWriter arg1)
		{
			if ( this.partyLeaderId < 0 )
			{
				throw new Exception("Forbidden value (" + this.partyLeaderId + ") on element partyLeaderId.");
			}
			arg1.WriteInt((int)this.partyLeaderId);
			arg1.WriteShort((short)this.members.Count);
			var loc1 = 0;
			while ( loc1 < this.members.Count )
			{
				this.members[loc1].serializeAs_PartyMemberInformations(arg1);
				++loc1;
			}
			arg1.WriteBoolean(this.restricted);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartyJoinMessage(arg1);
		}
		
		public void deserializeAs_PartyJoinMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.partyLeaderId = (uint)arg1.ReadInt();
			if ( this.partyLeaderId < 0 )
			{
				throw new Exception("Forbidden value (" + this.partyLeaderId + ") on element of PartyJoinMessage.partyLeaderId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new PartyMemberInformations()) as PartyMemberInformations).deserialize(arg1);
				this.members.Add((PartyMemberInformations)loc3);
				++loc2;
			}
			this.restricted = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
