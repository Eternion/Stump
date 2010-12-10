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
	
	public class IgnoredListMessage : Message
	{
		public const uint protocolId = 5674;
		internal Boolean _isInitialized = false;
		public List<IgnoredInformations> ignoredList;
		
		public IgnoredListMessage()
		{
			this.ignoredList = new List<IgnoredInformations>();
		}
		
		public IgnoredListMessage(List<IgnoredInformations> arg1)
			: this()
		{
			initIgnoredListMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5674;
		}
		
		public IgnoredListMessage initIgnoredListMessage(List<IgnoredInformations> arg1)
		{
			this.ignoredList = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.ignoredList = new List<IgnoredInformations>();
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
			this.serializeAs_IgnoredListMessage(arg1);
		}
		
		public void serializeAs_IgnoredListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.ignoredList.Count);
			var loc1 = 0;
			while ( loc1 < this.ignoredList.Count )
			{
				arg1.WriteShort((short)this.ignoredList[loc1].getTypeId());
				this.ignoredList[loc1].serialize(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_IgnoredListMessage(arg1);
		}
		
		public void deserializeAs_IgnoredListMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			object loc4 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = (ushort)arg1.ReadUShort();
				(( loc4 = ProtocolTypeManager.GetInstance<IgnoredInformations>((uint)loc3)) as IgnoredInformations).deserialize(arg1);
				this.ignoredList.Add((IgnoredInformations)loc4);
				++loc2;
			}
		}
		
	}
}
