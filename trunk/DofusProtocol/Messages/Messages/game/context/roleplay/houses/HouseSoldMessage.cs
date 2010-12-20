using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class HouseSoldMessage : Message
	{
		public const uint protocolId = 5737;
		internal Boolean _isInitialized = false;
		public uint houseId = 0;
		public uint realPrice = 0;
		public String buyerName = "";
		
		public HouseSoldMessage()
		{
		}
		
		public HouseSoldMessage(uint arg1, uint arg2, String arg3)
			: this()
		{
			initHouseSoldMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5737;
		}
		
		public HouseSoldMessage initHouseSoldMessage(uint arg1 = 0, uint arg2 = 0, String arg3 = "")
		{
			this.houseId = arg1;
			this.realPrice = arg2;
			this.buyerName = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.houseId = 0;
			this.realPrice = 0;
			this.buyerName = "";
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
			this.serializeAs_HouseSoldMessage(arg1);
		}
		
		public void serializeAs_HouseSoldMessage(BigEndianWriter arg1)
		{
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element houseId.");
			}
			arg1.WriteInt((int)this.houseId);
			if ( this.realPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.realPrice + ") on element realPrice.");
			}
			arg1.WriteInt((int)this.realPrice);
			arg1.WriteUTF((string)this.buyerName);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseSoldMessage(arg1);
		}
		
		public void deserializeAs_HouseSoldMessage(BigEndianReader arg1)
		{
			this.houseId = (uint)arg1.ReadInt();
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element of HouseSoldMessage.houseId.");
			}
			this.realPrice = (uint)arg1.ReadInt();
			if ( this.realPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.realPrice + ") on element of HouseSoldMessage.realPrice.");
			}
			this.buyerName = (String)arg1.ReadUTF();
		}
		
	}
}
