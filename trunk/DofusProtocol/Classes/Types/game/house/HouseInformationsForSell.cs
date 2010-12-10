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
	
	public class HouseInformationsForSell : Object
	{
		public const uint protocolId = 221;
		public uint modelId = 0;
		public String ownerName = "";
		public Boolean ownerConnected = false;
		public int worldX = 0;
		public int worldY = 0;
		public uint subAreaId = 0;
		public int nbRoom = 0;
		public int nbChest = 0;
		public List<int> skillListIds;
		public Boolean isLocked = false;
		public uint price = 0;
		
		public HouseInformationsForSell()
		{
			this.skillListIds = new List<int>();
		}
		
		public HouseInformationsForSell(uint arg1, String arg2, Boolean arg3, int arg4, int arg5, uint arg6, int arg7, int arg8, List<int> arg9, Boolean arg10, uint arg11)
			: this()
		{
			initHouseInformationsForSell(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
		}
		
		public virtual uint getTypeId()
		{
			return 221;
		}
		
		public HouseInformationsForSell initHouseInformationsForSell(uint arg1 = 0, String arg2 = "", Boolean arg3 = false, int arg4 = 0, int arg5 = 0, uint arg6 = 0, int arg7 = 0, int arg8 = 0, List<int> arg9 = null, Boolean arg10 = false, uint arg11 = 0)
		{
			this.modelId = arg1;
			this.ownerName = arg2;
			this.ownerConnected = arg3;
			this.worldX = arg4;
			this.worldY = arg5;
			this.subAreaId = arg6;
			this.nbRoom = arg7;
			this.nbChest = arg8;
			this.skillListIds = arg9;
			this.isLocked = arg10;
			this.price = arg11;
			return this;
		}
		
		public virtual void reset()
		{
			this.modelId = 0;
			this.ownerName = "";
			this.ownerConnected = false;
			this.worldX = 0;
			this.worldY = 0;
			this.subAreaId = 0;
			this.nbRoom = 0;
			this.nbChest = 0;
			this.skillListIds = new List<int>();
			this.isLocked = false;
			this.price = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_HouseInformationsForSell(arg1);
		}
		
		public void serializeAs_HouseInformationsForSell(BigEndianWriter arg1)
		{
			if ( this.modelId < 0 )
			{
				throw new Exception("Forbidden value (" + this.modelId + ") on element modelId.");
			}
			arg1.WriteInt((int)this.modelId);
			arg1.WriteUTF((string)this.ownerName);
			arg1.WriteBoolean(this.ownerConnected);
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element worldX.");
			}
			arg1.WriteShort((short)this.worldX);
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element worldY.");
			}
			arg1.WriteShort((short)this.worldY);
			if ( this.subAreaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subAreaId + ") on element subAreaId.");
			}
			arg1.WriteShort((short)this.subAreaId);
			arg1.WriteByte((byte)this.nbRoom);
			arg1.WriteByte((byte)this.nbChest);
			arg1.WriteShort((short)this.skillListIds.Count);
			var loc1 = 0;
			while ( loc1 < this.skillListIds.Count )
			{
				arg1.WriteInt((int)this.skillListIds[loc1]);
				++loc1;
			}
			arg1.WriteBoolean(this.isLocked);
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element price.");
			}
			arg1.WriteInt((int)this.price);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseInformationsForSell(arg1);
		}
		
		public void deserializeAs_HouseInformationsForSell(BigEndianReader arg1)
		{
			var loc3 = 0;
			this.modelId = (uint)arg1.ReadInt();
			if ( this.modelId < 0 )
			{
				throw new Exception("Forbidden value (" + this.modelId + ") on element of HouseInformationsForSell.modelId.");
			}
			this.ownerName = (String)arg1.ReadUTF();
			this.ownerConnected = (Boolean)arg1.ReadBoolean();
			this.worldX = (int)arg1.ReadShort();
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element of HouseInformationsForSell.worldX.");
			}
			this.worldY = (int)arg1.ReadShort();
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element of HouseInformationsForSell.worldY.");
			}
			this.subAreaId = (uint)arg1.ReadShort();
			if ( this.subAreaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subAreaId + ") on element of HouseInformationsForSell.subAreaId.");
			}
			this.nbRoom = (int)arg1.ReadByte();
			this.nbChest = (int)arg1.ReadByte();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = arg1.ReadInt();
				this.skillListIds.Add((int)loc3);
				++loc2;
			}
			this.isLocked = (Boolean)arg1.ReadBoolean();
			this.price = (uint)arg1.ReadInt();
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element of HouseInformationsForSell.price.");
			}
		}
		
	}
}
