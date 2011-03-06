using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeKamaModifiedMessage : ExchangeObjectMessage
	{
		public const uint protocolId = 5521;
		internal Boolean _isInitialized = false;
		public uint quantity = 0;
		
		public ExchangeKamaModifiedMessage()
		{
		}
		
		public ExchangeKamaModifiedMessage(Boolean arg1, uint arg2)
			: this()
		{
			initExchangeKamaModifiedMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5521;
		}
		
		public ExchangeKamaModifiedMessage initExchangeKamaModifiedMessage(Boolean arg1 = false, uint arg2 = 0)
		{
			base.initExchangeObjectMessage(arg1);
			this.quantity = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.quantity = 0;
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ExchangeKamaModifiedMessage(arg1);
		}
		
		public void serializeAs_ExchangeKamaModifiedMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ExchangeObjectMessage(arg1);
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element quantity.");
			}
			arg1.WriteInt((int)this.quantity);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeKamaModifiedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeKamaModifiedMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.quantity = (uint)arg1.ReadInt();
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element of ExchangeKamaModifiedMessage.quantity.");
			}
		}
		
	}
}
