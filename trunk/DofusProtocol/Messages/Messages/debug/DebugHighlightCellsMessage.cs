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
	
	public class DebugHighlightCellsMessage : Message
	{
		public const uint protocolId = 2001;
		internal Boolean _isInitialized = false;
		public int color = 0;
		public List<uint> cells;
		
		public DebugHighlightCellsMessage()
		{
			this.cells = new List<uint>();
		}
		
		public DebugHighlightCellsMessage(int arg1, List<uint> arg2)
			: this()
		{
			initDebugHighlightCellsMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 2001;
		}
		
		public DebugHighlightCellsMessage initDebugHighlightCellsMessage(int arg1 = 0, List<uint> arg2 = null)
		{
			this.color = arg1;
			this.cells = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.color = 0;
			this.cells = new List<uint>();
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
			this.serializeAs_DebugHighlightCellsMessage(arg1);
		}
		
		public void serializeAs_DebugHighlightCellsMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.color);
			arg1.WriteShort((short)this.cells.Count);
			var loc1 = 0;
			while ( loc1 < this.cells.Count )
			{
				if ( this.cells[loc1] < 0 || this.cells[loc1] > 559 )
				{
					throw new Exception("Forbidden value (" + this.cells[loc1] + ") on element 2 (starting at 1) of cells.");
				}
				arg1.WriteShort((short)this.cells[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_DebugHighlightCellsMessage(arg1);
		}
		
		public void deserializeAs_DebugHighlightCellsMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			this.color = (int)arg1.ReadInt();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadShort()) < 0 || loc3 > 559 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of cells.");
				}
				this.cells.Add((uint)loc3);
				++loc2;
			}
		}
		
	}
}
