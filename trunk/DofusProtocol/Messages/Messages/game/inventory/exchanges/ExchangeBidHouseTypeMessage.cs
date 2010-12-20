using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeBidHouseTypeMessage : Message
	{
		public const uint protocolId = 5803;
		internal Boolean _isInitialized = false;
		public uint type = 0;
		
		public ExchangeBidHouseTypeMessage()
		{
		}
		
		public ExchangeBidHouseTypeMessage(uint arg1)
			: this()
		{
			initExchangeBidHouseTypeMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5803;
		}
		
		public ExchangeBidHouseTypeMessage initExchangeBidHouseTypeMessage(uint arg1 = 0)
		{
			this.type = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.type = 0;
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
			this.serializeAs_ExchangeBidHouseTypeMessage(arg1);
		}
		
		public void serializeAs_ExchangeBidHouseTypeMessage(BigEndianWriter arg1)
		{
			if ( this.type < 0 )
			{
				throw new Exception("Forbidden value (" + this.type + ") on element type.");
			}
			arg1.WriteInt((int)this.type);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeBidHouseTypeMessage(arg1);
		}
		
		public void deserializeAs_ExchangeBidHouseTypeMessage(BigEndianReader arg1)
		{
			this.type = (uint)arg1.ReadInt();
			if ( this.type < 0 )
			{
				throw new Exception("Forbidden value (" + this.type + ") on element of ExchangeBidHouseTypeMessage.type.");
			}
		}
		
	}
}
