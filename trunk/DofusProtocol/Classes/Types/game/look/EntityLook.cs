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
	
	public class EntityLook : Object
	{
		public const uint protocolId = 55;
		public uint bonesId = 0;
		public List<uint> skins;
		public List<int> indexedColors;
		public List<int> scales;
		public List<SubEntity> subentities;
		
		public EntityLook()
		{
			this.skins = new List<uint>();
			this.indexedColors = new List<int>();
			this.scales = new List<int>();
			this.subentities = new List<SubEntity>();
		}
		
		public EntityLook(uint arg1, List<uint> arg2, List<int> arg3, List<int> arg4, List<SubEntity> arg5)
			: this()
		{
			initEntityLook(arg1, arg2, arg3, arg4, arg5);
		}
		
		public virtual uint getTypeId()
		{
			return 55;
		}
		
		public EntityLook initEntityLook(uint arg1 = 0, List<uint> arg2 = null, List<int> arg3 = null, List<int> arg4 = null, List<SubEntity> arg5 = null)
		{
			this.bonesId = arg1;
			this.skins = arg2;
			this.indexedColors = arg3;
			this.scales = arg4;
			this.subentities = arg5;
			return this;
		}
		
		public virtual void reset()
		{
			this.bonesId = 0;
			this.skins = new List<uint>();
			this.indexedColors = new List<int>();
			this.scales = new List<int>();
			this.subentities = new List<SubEntity>();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_EntityLook(arg1);
		}
		
		public void serializeAs_EntityLook(BigEndianWriter arg1)
		{
			if ( this.bonesId < 0 )
			{
				throw new Exception("Forbidden value (" + this.bonesId + ") on element bonesId.");
			}
			arg1.WriteShort((short)this.bonesId);
			arg1.WriteShort((short)this.skins.Count);
			var loc1 = 0;
			while ( loc1 < this.skins.Count )
			{
				if ( this.skins[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.skins[loc1] + ") on element 2 (starting at 1) of skins.");
				}
				arg1.WriteShort((short)this.skins[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.indexedColors.Count);
			var loc2 = 0;
			while ( loc2 < this.indexedColors.Count )
			{
				arg1.WriteInt((int)this.indexedColors[loc2]);
				++loc2;
			}
			arg1.WriteShort((short)this.scales.Count);
			var loc3 = 0;
			while ( loc3 < this.scales.Count )
			{
				arg1.WriteShort((short)this.scales[loc3]);
				++loc3;
			}
			arg1.WriteShort((short)this.subentities.Count);
			var loc4 = 0;
			while ( loc4 < this.subentities.Count )
			{
				this.subentities[loc4].serializeAs_SubEntity(arg1);
				++loc4;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_EntityLook(arg1);
		}
		
		public void deserializeAs_EntityLook(BigEndianReader arg1)
		{
			var loc9 = 0;
			var loc10 = 0;
			var loc11 = 0;
			object loc12 = null;
			this.bonesId = (uint)arg1.ReadShort();
			if ( this.bonesId < 0 )
			{
				throw new Exception("Forbidden value (" + this.bonesId + ") on element of EntityLook.bonesId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc9 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc9 + ") on elements of skins.");
				}
				this.skins.Add((uint)loc9);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				loc10 = arg1.ReadInt();
				this.indexedColors.Add((int)loc10);
				++loc4;
			}
			var loc5 = (ushort)arg1.ReadUShort();
			var loc6 = 0;
			while ( loc6 < loc5 )
			{
				loc11 = arg1.ReadShort();
				this.scales.Add((int)loc11);
				++loc6;
			}
			var loc7 = (ushort)arg1.ReadUShort();
			var loc8 = 0;
			while ( loc8 < loc7 )
			{
				((loc12 = new SubEntity()) as SubEntity).deserialize(arg1);
				this.subentities.Add((SubEntity)loc12);
				++loc8;
			}
		}
		
	}
}
