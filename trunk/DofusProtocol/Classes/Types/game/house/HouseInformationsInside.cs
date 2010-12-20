using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class HouseInformationsInside : Object
	{
		public const uint protocolId = 218;
		public uint houseId = 0;
		public uint modelId = 0;
		public int ownerId = 0;
		public String ownerName = "";
		public int worldX = 0;
		public int worldY = 0;
		public uint price = 0;
		public Boolean isLocked = false;
		
		public HouseInformationsInside()
		{
		}
		
		public HouseInformationsInside(uint arg1, uint arg2, int arg3, String arg4, int arg5, int arg6, uint arg7, Boolean arg8)
			: this()
		{
			initHouseInformationsInside(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}
		
		public virtual uint getTypeId()
		{
			return 218;
		}
		
		public HouseInformationsInside initHouseInformationsInside(uint arg1 = 0, uint arg2 = 0, int arg3 = 0, String arg4 = "", int arg5 = 0, int arg6 = 0, uint arg7 = 0, Boolean arg8 = false)
		{
			this.houseId = arg1;
			this.modelId = arg2;
			this.ownerId = arg3;
			this.ownerName = arg4;
			this.worldX = arg5;
			this.worldY = arg6;
			this.price = arg7;
			this.isLocked = arg8;
			return this;
		}
		
		public virtual void reset()
		{
			this.houseId = 0;
			this.modelId = 0;
			this.ownerId = 0;
			this.ownerName = "";
			this.worldX = 0;
			this.worldY = 0;
			this.price = 0;
			this.isLocked = false;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_HouseInformationsInside(arg1);
		}
		
		public void serializeAs_HouseInformationsInside(BigEndianWriter arg1)
		{
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element houseId.");
			}
			arg1.WriteInt((int)this.houseId);
			if ( this.modelId < 0 )
			{
				throw new Exception("Forbidden value (" + this.modelId + ") on element modelId.");
			}
			arg1.WriteShort((short)this.modelId);
			arg1.WriteInt((int)this.ownerId);
			arg1.WriteUTF((string)this.ownerName);
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
			if ( this.price < 0 || this.price > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element price.");
			}
			arg1.WriteUInt((uint)this.price);
			arg1.WriteBoolean(this.isLocked);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseInformationsInside(arg1);
		}
		
		public void deserializeAs_HouseInformationsInside(BigEndianReader arg1)
		{
			this.houseId = (uint)arg1.ReadInt();
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element of HouseInformationsInside.houseId.");
			}
			this.modelId = (uint)arg1.ReadShort();
			if ( this.modelId < 0 )
			{
				throw new Exception("Forbidden value (" + this.modelId + ") on element of HouseInformationsInside.modelId.");
			}
			this.ownerId = (int)arg1.ReadInt();
			this.ownerName = (String)arg1.ReadUTF();
			this.worldX = (int)arg1.ReadShort();
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element of HouseInformationsInside.worldX.");
			}
			this.worldY = (int)arg1.ReadShort();
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element of HouseInformationsInside.worldY.");
			}
			this.price = (uint)arg1.ReadUInt();
			if ( this.price < 0 || this.price > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element of HouseInformationsInside.price.");
			}
			this.isLocked = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
