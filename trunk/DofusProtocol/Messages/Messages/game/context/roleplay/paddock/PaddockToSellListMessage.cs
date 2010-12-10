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
	
	public class PaddockToSellListMessage : Message
	{
		public const uint protocolId = 6138;
		internal Boolean _isInitialized = false;
		public uint pageIndex = 0;
		public uint totalPage = 0;
		public List<PaddockInformationsForSell> paddockList;
		
		public PaddockToSellListMessage()
		{
			this.paddockList = new List<PaddockInformationsForSell>();
		}
		
		public PaddockToSellListMessage(uint arg1, uint arg2, List<PaddockInformationsForSell> arg3)
			: this()
		{
			initPaddockToSellListMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6138;
		}
		
		public PaddockToSellListMessage initPaddockToSellListMessage(uint arg1 = 0, uint arg2 = 0, List<PaddockInformationsForSell> arg3 = null)
		{
			this.pageIndex = arg1;
			this.totalPage = arg2;
			this.paddockList = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.pageIndex = 0;
			this.totalPage = 0;
			this.paddockList = new List<PaddockInformationsForSell>();
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
			this.serializeAs_PaddockToSellListMessage(arg1);
		}
		
		public void serializeAs_PaddockToSellListMessage(BigEndianWriter arg1)
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
			arg1.WriteShort((short)this.paddockList.Count);
			var loc1 = 0;
			while ( loc1 < this.paddockList.Count )
			{
				this.paddockList[loc1].serializeAs_PaddockInformationsForSell(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockToSellListMessage(arg1);
		}
		
		public void deserializeAs_PaddockToSellListMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.pageIndex = (uint)arg1.ReadShort();
			if ( this.pageIndex < 0 )
			{
				throw new Exception("Forbidden value (" + this.pageIndex + ") on element of PaddockToSellListMessage.pageIndex.");
			}
			this.totalPage = (uint)arg1.ReadShort();
			if ( this.totalPage < 0 )
			{
				throw new Exception("Forbidden value (" + this.totalPage + ") on element of PaddockToSellListMessage.totalPage.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new PaddockInformationsForSell()) as PaddockInformationsForSell).deserialize(arg1);
				this.paddockList.Add((PaddockInformationsForSell)loc3);
				++loc2;
			}
		}
		
	}
}
