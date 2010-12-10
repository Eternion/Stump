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
	
	public class FightLoot : Object
	{
		public const uint protocolId = 41;
		public List<uint> objects;
		public uint kamas = 0;
		
		public FightLoot()
		{
			this.@objects = new List<uint>();
		}
		
		public FightLoot(List<uint> arg1, uint arg2)
			: this()
		{
			initFightLoot(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 41;
		}
		
		public FightLoot initFightLoot(List<uint> arg1, uint arg2 = 0)
		{
			this.@objects = arg1;
			this.kamas = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.@objects = new List<uint>();
			this.kamas = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightLoot(arg1);
		}
		
		public void serializeAs_FightLoot(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.@objects.Count);
			var loc1 = 0;
			while ( loc1 < this.@objects.Count )
			{
				if ( this.@objects[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.@objects[loc1] + ") on element 1 (starting at 1) of objects.");
				}
				arg1.WriteShort((short)this.@objects[loc1]);
				++loc1;
			}
			if ( this.kamas < 0 )
			{
				throw new Exception("Forbidden value (" + this.kamas + ") on element kamas.");
			}
			arg1.WriteInt((int)this.kamas);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightLoot(arg1);
		}
		
		public void deserializeAs_FightLoot(BigEndianReader arg1)
		{
			var loc3 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of objects.");
				}
				this.@objects.Add((uint)loc3);
				++loc2;
			}
			this.kamas = (uint)arg1.ReadInt();
			if ( this.kamas < 0 )
			{
				throw new Exception("Forbidden value (" + this.kamas + ") on element of FightLoot.kamas.");
			}
		}
		
	}
}
