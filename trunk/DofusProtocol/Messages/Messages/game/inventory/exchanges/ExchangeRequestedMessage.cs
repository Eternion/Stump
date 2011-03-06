using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeRequestedMessage : Message
	{
		public const uint protocolId = 5522;
		internal Boolean _isInitialized = false;
		public int exchangeType = 0;
		
		public ExchangeRequestedMessage()
		{
		}
		
		public ExchangeRequestedMessage(int arg1)
			: this()
		{
			initExchangeRequestedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5522;
		}
		
		public ExchangeRequestedMessage initExchangeRequestedMessage(int arg1 = 0)
		{
			this.exchangeType = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.exchangeType = 0;
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
			this.serializeAs_ExchangeRequestedMessage(arg1);
		}
		
		public void serializeAs_ExchangeRequestedMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.exchangeType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeRequestedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeRequestedMessage(BigEndianReader arg1)
		{
			this.exchangeType = (int)arg1.ReadByte();
		}
		
	}
}
