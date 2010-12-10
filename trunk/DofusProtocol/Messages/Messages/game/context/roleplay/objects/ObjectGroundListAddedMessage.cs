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
	
	public class ObjectGroundListAddedMessage : Message
	{
		public const uint protocolId = 5925;
		internal Boolean _isInitialized = false;
		public List<uint> cells;
		public List<uint> referenceIds;
		
		public ObjectGroundListAddedMessage()
		{
			this.cells = new List<uint>();
			this.referenceIds = new List<uint>();
		}
		
		public ObjectGroundListAddedMessage(List<uint> arg1, List<uint> arg2)
			: this()
		{
			initObjectGroundListAddedMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5925;
		}
		
		public ObjectGroundListAddedMessage initObjectGroundListAddedMessage(List<uint> arg1, List<uint> arg2)
		{
			this.cells = arg1;
			this.referenceIds = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.cells = new List<uint>();
			this.referenceIds = new List<uint>();
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
			this.serializeAs_ObjectGroundListAddedMessage(arg1);
		}
		
		public void serializeAs_ObjectGroundListAddedMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.cells.Count);
			var loc1 = 0;
			while ( loc1 < this.cells.Count )
			{
				if ( this.cells[loc1] < 0 || this.cells[loc1] > 559 )
				{
					throw new Exception("Forbidden value (" + this.cells[loc1] + ") on element 1 (starting at 1) of cells.");
				}
				arg1.WriteShort((short)this.cells[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.referenceIds.Count);
			var loc2 = 0;
			while ( loc2 < this.referenceIds.Count )
			{
				if ( this.referenceIds[loc2] < 0 )
				{
					throw new Exception("Forbidden value (" + this.referenceIds[loc2] + ") on element 2 (starting at 1) of referenceIds.");
				}
				arg1.WriteInt((int)this.referenceIds[loc2]);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectGroundListAddedMessage(arg1);
		}
		
		public void deserializeAs_ObjectGroundListAddedMessage(BigEndianReader arg1)
		{
			var loc5 = 0;
			var loc6 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc5 = arg1.ReadShort()) < 0 || loc5 > 559 )
				{
					throw new Exception("Forbidden value (" + loc5 + ") on elements of cells.");
				}
				this.cells.Add((uint)loc5);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				if ( (loc6 = arg1.ReadInt()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc6 + ") on elements of referenceIds.");
				}
				this.referenceIds.Add((uint)loc6);
				++loc4;
			}
		}
		
	}
}
