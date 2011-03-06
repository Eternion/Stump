using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeBidHouseBuyMessage : Message
	{
		public const uint protocolId = 5804;
		internal Boolean _isInitialized = false;
		public uint uid = 0;
		public uint qty = 0;
		public uint price = 0;
		
		public ExchangeBidHouseBuyMessage()
		{
		}
		
		public ExchangeBidHouseBuyMessage(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initExchangeBidHouseBuyMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5804;
		}
		
		public ExchangeBidHouseBuyMessage initExchangeBidHouseBuyMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			this.uid = arg1;
			this.qty = arg2;
			this.price = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.uid = 0;
			this.qty = 0;
			this.price = 0;
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
			this.serializeAs_ExchangeBidHouseBuyMessage(arg1);
		}
		
		public void serializeAs_ExchangeBidHouseBuyMessage(BigEndianWriter arg1)
		{
			if ( this.uid < 0 )
			{
				throw new Exception("Forbidden value (" + this.uid + ") on element uid.");
			}
			arg1.WriteInt((int)this.uid);
			if ( this.qty < 0 )
			{
				throw new Exception("Forbidden value (" + this.qty + ") on element qty.");
			}
			arg1.WriteInt((int)this.qty);
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element price.");
			}
			arg1.WriteInt((int)this.price);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeBidHouseBuyMessage(arg1);
		}
		
		public void deserializeAs_ExchangeBidHouseBuyMessage(BigEndianReader arg1)
		{
			this.uid = (uint)arg1.ReadInt();
			if ( this.uid < 0 )
			{
				throw new Exception("Forbidden value (" + this.uid + ") on element of ExchangeBidHouseBuyMessage.uid.");
			}
			this.qty = (uint)arg1.ReadInt();
			if ( this.qty < 0 )
			{
				throw new Exception("Forbidden value (" + this.qty + ") on element of ExchangeBidHouseBuyMessage.qty.");
			}
			this.price = (uint)arg1.ReadInt();
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element of ExchangeBidHouseBuyMessage.price.");
			}
		}
		
	}
}
