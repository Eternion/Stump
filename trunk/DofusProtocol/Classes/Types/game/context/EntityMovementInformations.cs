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
	
	public class EntityMovementInformations : Object
	{
		public const uint protocolId = 63;
		public int id = 0;
		public List<int> steps;
		
		public EntityMovementInformations()
		{
			this.steps = new List<int>();
		}
		
		public EntityMovementInformations(int arg1, List<int> arg2)
			: this()
		{
			initEntityMovementInformations(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 63;
		}
		
		public EntityMovementInformations initEntityMovementInformations(int arg1 = 0, List<int> arg2 = null)
		{
			this.id = arg1;
			this.steps = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.id = 0;
			this.steps = new List<int>();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_EntityMovementInformations(arg1);
		}
		
		public void serializeAs_EntityMovementInformations(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.id);
			arg1.WriteShort((short)this.steps.Count);
			var loc1 = 0;
			while ( loc1 < this.steps.Count )
			{
				arg1.WriteByte((byte)this.steps[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_EntityMovementInformations(arg1);
		}
		
		public void deserializeAs_EntityMovementInformations(BigEndianReader arg1)
		{
			var loc3 = 0;
			this.id = (int)arg1.ReadInt();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = arg1.ReadByte();
				this.steps.Add((int)loc3);
				++loc2;
			}
		}
		
	}
}
