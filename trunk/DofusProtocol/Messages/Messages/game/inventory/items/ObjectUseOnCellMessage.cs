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
	
	public class ObjectUseOnCellMessage : ObjectUseMessage
	{
		public const uint protocolId = 3013;
		internal Boolean _isInitialized = false;
		public uint cells = 0;
		
		public ObjectUseOnCellMessage()
		{
		}
		
		public ObjectUseOnCellMessage(uint arg1, uint arg2)
			: this()
		{
			initObjectUseOnCellMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 3013;
		}
		
		public ObjectUseOnCellMessage initObjectUseOnCellMessage(uint arg1 = 0, uint arg2 = 0)
		{
			base.initObjectUseMessage(arg1);
			this.cells = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.cells = 0;
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
			this.serializeAs_ObjectUseOnCellMessage(arg1);
		}
		
		public void serializeAs_ObjectUseOnCellMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectUseMessage(arg1);
			if ( this.cells < 0 || this.cells > 559 )
			{
				throw new Exception("Forbidden value (" + this.cells + ") on element cells.");
			}
			arg1.WriteShort((short)this.cells);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectUseOnCellMessage(arg1);
		}
		
		public void deserializeAs_ObjectUseOnCellMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.cells = (uint)arg1.ReadShort();
			if ( this.cells < 0 || this.cells > 559 )
			{
				throw new Exception("Forbidden value (" + this.cells + ") on element of ObjectUseOnCellMessage.cells.");
			}
		}
		
	}
}
