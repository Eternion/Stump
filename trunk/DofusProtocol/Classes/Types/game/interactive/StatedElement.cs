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
namespace Stump.DofusProtocol.Classes
{
	
	public class StatedElement : Object
	{
		public const uint protocolId = 108;
		public uint elementId = 0;
		public uint elementCellId = 0;
		public uint elementState = 0;
		
		public StatedElement()
		{
		}
		
		public StatedElement(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initStatedElement(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 108;
		}
		
		public StatedElement initStatedElement(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			this.elementId = arg1;
			this.elementCellId = arg2;
			this.elementState = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.elementId = 0;
			this.elementCellId = 0;
			this.elementState = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_StatedElement(arg1);
		}
		
		public void serializeAs_StatedElement(BigEndianWriter arg1)
		{
			if ( this.elementId < 0 )
			{
				throw new Exception("Forbidden value (" + this.elementId + ") on element elementId.");
			}
			arg1.WriteInt((int)this.elementId);
			if ( this.elementCellId < 0 || this.elementCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.elementCellId + ") on element elementCellId.");
			}
			arg1.WriteShort((short)this.elementCellId);
			if ( this.elementState < 0 )
			{
				throw new Exception("Forbidden value (" + this.elementState + ") on element elementState.");
			}
			arg1.WriteInt((int)this.elementState);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_StatedElement(arg1);
		}
		
		public void deserializeAs_StatedElement(BigEndianReader arg1)
		{
			this.elementId = (uint)arg1.ReadInt();
			if ( this.elementId < 0 )
			{
				throw new Exception("Forbidden value (" + this.elementId + ") on element of StatedElement.elementId.");
			}
			this.elementCellId = (uint)arg1.ReadShort();
			if ( this.elementCellId < 0 || this.elementCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.elementCellId + ") on element of StatedElement.elementCellId.");
			}
			this.elementState = (uint)arg1.ReadInt();
			if ( this.elementState < 0 )
			{
				throw new Exception("Forbidden value (" + this.elementState + ") on element of StatedElement.elementState.");
			}
		}
		
	}
}
