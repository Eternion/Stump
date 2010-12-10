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
	
	public class PartyLocateMembersMessage : Message
	{
		public const uint protocolId = 5595;
		internal Boolean _isInitialized = false;
		public List<uint> geopositions;
		
		public PartyLocateMembersMessage()
		{
			this.geopositions = new List<uint>();
		}
		
		public PartyLocateMembersMessage(List<uint> arg1)
			: this()
		{
			initPartyLocateMembersMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5595;
		}
		
		public PartyLocateMembersMessage initPartyLocateMembersMessage(List<uint> arg1)
		{
			this.geopositions = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.geopositions = new List<uint>();
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
			this.serializeAs_PartyLocateMembersMessage(arg1);
		}
		
		public void serializeAs_PartyLocateMembersMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.geopositions.Count);
			var loc1 = 0;
			while ( loc1 < this.geopositions.Count )
			{
				if ( this.geopositions[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.geopositions[loc1] + ") on element 1 (starting at 1) of geopositions.");
				}
				arg1.WriteInt((int)this.geopositions[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartyLocateMembersMessage(arg1);
		}
		
		public void deserializeAs_PartyLocateMembersMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadInt()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of geopositions.");
				}
				this.geopositions.Add((uint)loc3);
				++loc2;
			}
		}
		
	}
}
