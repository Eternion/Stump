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
	
	public class HouseToSellListMessage : Message
	{
		public const uint protocolId = 6140;
		internal Boolean _isInitialized = false;
		public uint pageIndex = 0;
		public uint totalPage = 0;
		public List<HouseInformationsForSell> houseList;
		
		public HouseToSellListMessage()
		{
			this.houseList = new List<HouseInformationsForSell>();
		}
		
		public HouseToSellListMessage(uint arg1, uint arg2, List<HouseInformationsForSell> arg3)
			: this()
		{
			initHouseToSellListMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6140;
		}
		
		public HouseToSellListMessage initHouseToSellListMessage(uint arg1 = 0, uint arg2 = 0, List<HouseInformationsForSell> arg3 = null)
		{
			this.pageIndex = arg1;
			this.totalPage = arg2;
			this.houseList = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.pageIndex = 0;
			this.totalPage = 0;
			this.houseList = new List<HouseInformationsForSell>();
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
			this.serializeAs_HouseToSellListMessage(arg1);
		}
		
		public void serializeAs_HouseToSellListMessage(BigEndianWriter arg1)
		{
			if ( this.pageIndex < 0 )
			{
				throw new Exception("Forbidden value (" + this.pageIndex + ") on element pageIndex.");
			}
			arg1.WriteShort((short)this.pageIndex);
			if ( this.totalPage < 0 )
			{
				throw new Exception("Forbidden value (" + this.totalPage + ") on element totalPage.");
			}
			arg1.WriteShort((short)this.totalPage);
			arg1.WriteShort((short)this.houseList.Count);
			var loc1 = 0;
			while ( loc1 < this.houseList.Count )
			{
				this.houseList[loc1].serializeAs_HouseInformationsForSell(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseToSellListMessage(arg1);
		}
		
		public void deserializeAs_HouseToSellListMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.pageIndex = (uint)arg1.ReadShort();
			if ( this.pageIndex < 0 )
			{
				throw new Exception("Forbidden value (" + this.pageIndex + ") on element of HouseToSellListMessage.pageIndex.");
			}
			this.totalPage = (uint)arg1.ReadShort();
			if ( this.totalPage < 0 )
			{
				throw new Exception("Forbidden value (" + this.totalPage + ") on element of HouseToSellListMessage.totalPage.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new HouseInformationsForSell()) as HouseInformationsForSell).deserialize(arg1);
				this.houseList.Add((HouseInformationsForSell)loc3);
				++loc2;
			}
		}
		
	}
}
