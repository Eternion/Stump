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
	
	public class ObjectItemQuantity : Item
	{
		public const uint protocolId = 119;
		public uint objectUID = 0;
		public uint quantity = 0;
		
		public ObjectItemQuantity()
		{
		}
		
		public ObjectItemQuantity(uint arg1, uint arg2)
			: this()
		{
			initObjectItemQuantity(arg1, arg2);
		}
		
		public override uint getTypeId()
		{
			return 119;
		}
		
		public ObjectItemQuantity initObjectItemQuantity(uint arg1 = 0, uint arg2 = 0)
		{
			this.@objectUID = arg1;
			this.quantity = arg2;
			return this;
		}
		
		public override void reset()
		{
			this.@objectUID = 0;
			this.quantity = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectItemQuantity(arg1);
		}
		
		public void serializeAs_ObjectItemQuantity(BigEndianWriter arg1)
		{
			base.serializeAs_Item(arg1);
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element objectUID.");
			}
			arg1.WriteInt((int)this.@objectUID);
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element quantity.");
			}
			arg1.WriteInt((int)this.quantity);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectItemQuantity(arg1);
		}
		
		public void deserializeAs_ObjectItemQuantity(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.@objectUID = (uint)arg1.ReadInt();
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element of ObjectItemQuantity.objectUID.");
			}
			this.quantity = (uint)arg1.ReadInt();
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element of ObjectItemQuantity.quantity.");
			}
		}
		
	}
}
