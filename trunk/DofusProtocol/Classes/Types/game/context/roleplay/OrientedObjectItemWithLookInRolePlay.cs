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
	
	public class OrientedObjectItemWithLookInRolePlay : ObjectItemWithLookInRolePlay
	{
		public const uint protocolId = 199;
		public uint direction = 1;
		
		public OrientedObjectItemWithLookInRolePlay()
		{
		}
		
		public OrientedObjectItemWithLookInRolePlay(uint arg1, uint arg2, EntityLook arg3, uint arg4)
			: this()
		{
			initOrientedObjectItemWithLookInRolePlay(arg1, arg2, arg3, arg4);
		}
		
		public override uint getTypeId()
		{
			return 199;
		}
		
		public OrientedObjectItemWithLookInRolePlay initOrientedObjectItemWithLookInRolePlay(uint arg1 = 0, uint arg2 = 0, EntityLook arg3 = null, uint arg4 = 1)
		{
			base.initObjectItemWithLookInRolePlay(arg1, arg2, arg3);
			this.direction = arg4;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.direction = 1;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_OrientedObjectItemWithLookInRolePlay(arg1);
		}
		
		public void serializeAs_OrientedObjectItemWithLookInRolePlay(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectItemWithLookInRolePlay(arg1);
			arg1.WriteByte((byte)this.direction);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_OrientedObjectItemWithLookInRolePlay(arg1);
		}
		
		public void deserializeAs_OrientedObjectItemWithLookInRolePlay(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.direction = (uint)arg1.ReadByte();
			if ( this.direction < 0 )
			{
				throw new Exception("Forbidden value (" + this.direction + ") on element of OrientedObjectItemWithLookInRolePlay.direction.");
			}
		}
		
	}
}
