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
	
	public class HouseInformations : Object
	{
		public const uint protocolId = 111;
		public uint houseId = 0;
		public List<uint> doorsOnMap;
		public String ownerName = "";
		public Boolean isOnSale = false;
		public uint modelId = 0;
		
		public HouseInformations()
		{
			this.doorsOnMap = new List<uint>();
		}
		
		public HouseInformations(uint arg1, List<uint> arg2, String arg3, Boolean arg4, uint arg5)
			: this()
		{
			initHouseInformations(arg1, arg2, arg3, arg4, arg5);
		}
		
		public virtual uint getTypeId()
		{
			return 111;
		}
		
		public HouseInformations initHouseInformations(uint arg1 = 0, List<uint> arg2 = null, String arg3 = "", Boolean arg4 = false, uint arg5 = 0)
		{
			this.houseId = arg1;
			this.doorsOnMap = arg2;
			this.ownerName = arg3;
			this.isOnSale = arg4;
			this.modelId = arg5;
			return this;
		}
		
		public virtual void reset()
		{
			this.houseId = 0;
			this.doorsOnMap = new List<uint>();
			this.ownerName = "";
			this.isOnSale = false;
			this.modelId = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_HouseInformations(arg1);
		}
		
		public void serializeAs_HouseInformations(BigEndianWriter arg1)
		{
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element houseId.");
			}
			arg1.WriteInt((int)this.houseId);
			arg1.WriteShort((short)this.doorsOnMap.Count);
			var loc1 = 0;
			while ( loc1 < this.doorsOnMap.Count )
			{
				if ( this.doorsOnMap[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.doorsOnMap[loc1] + ") on element 2 (starting at 1) of doorsOnMap.");
				}
				arg1.WriteInt((int)this.doorsOnMap[loc1]);
				++loc1;
			}
			arg1.WriteUTF((string)this.ownerName);
			arg1.WriteBoolean(this.isOnSale);
			if ( this.modelId < 0 )
			{
				throw new Exception("Forbidden value (" + this.modelId + ") on element modelId.");
			}
			arg1.WriteShort((short)this.modelId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseInformations(arg1);
		}
		
		public void deserializeAs_HouseInformations(BigEndianReader arg1)
		{
			var loc3 = 0;
			this.houseId = (uint)arg1.ReadInt();
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element of HouseInformations.houseId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadInt()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of doorsOnMap.");
				}
				this.doorsOnMap.Add((uint)loc3);
				++loc2;
			}
			this.ownerName = (String)arg1.ReadUTF();
			this.isOnSale = (Boolean)arg1.ReadBoolean();
			this.modelId = (uint)arg1.ReadShort();
			if ( this.modelId < 0 )
			{
				throw new Exception("Forbidden value (" + this.modelId + ") on element of HouseInformations.modelId.");
			}
		}
		
	}
}
