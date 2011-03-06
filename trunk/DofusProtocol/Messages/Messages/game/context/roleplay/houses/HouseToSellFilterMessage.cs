using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class HouseToSellFilterMessage : Message
	{
		public const uint protocolId = 6137;
		internal Boolean _isInitialized = false;
		public int areaId = 0;
		public int atLeastNbRoom = 0;
		public int atLeastNbChest = 0;
		public int skillRequested = 0;
		public uint maxPrice = 0;
		
		public HouseToSellFilterMessage()
		{
		}
		
		public HouseToSellFilterMessage(int arg1, int arg2, int arg3, int arg4, uint arg5)
			: this()
		{
			initHouseToSellFilterMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 6137;
		}
		
		public HouseToSellFilterMessage initHouseToSellFilterMessage(int arg1 = 0, int arg2 = 0, int arg3 = 0, int arg4 = 0, uint arg5 = 0)
		{
			this.areaId = arg1;
			this.atLeastNbRoom = arg2;
			this.atLeastNbChest = arg3;
			this.skillRequested = arg4;
			this.maxPrice = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.areaId = 0;
			this.atLeastNbRoom = 0;
			this.atLeastNbChest = 0;
			this.skillRequested = 0;
			this.maxPrice = 0;
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
			this.serializeAs_HouseToSellFilterMessage(arg1);
		}
		
		public void serializeAs_HouseToSellFilterMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.areaId);
			arg1.WriteByte((byte)this.atLeastNbRoom);
			arg1.WriteByte((byte)this.atLeastNbChest);
			arg1.WriteByte((byte)this.skillRequested);
			if ( this.maxPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxPrice + ") on element maxPrice.");
			}
			arg1.WriteInt((int)this.maxPrice);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseToSellFilterMessage(arg1);
		}
		
		public void deserializeAs_HouseToSellFilterMessage(BigEndianReader arg1)
		{
			this.areaId = (int)arg1.ReadInt();
			this.atLeastNbRoom = (int)arg1.ReadByte();
			this.atLeastNbChest = (int)arg1.ReadByte();
			this.skillRequested = (int)arg1.ReadByte();
			this.maxPrice = (uint)arg1.ReadInt();
			if ( this.maxPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxPrice + ") on element of HouseToSellFilterMessage.maxPrice.");
			}
		}
		
	}
}
