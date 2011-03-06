using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeRequestMessage : Message
	{
		public const uint protocolId = 5505;
		internal Boolean _isInitialized = false;
		public int exchangeType = 0;
		
		public ExchangeRequestMessage()
		{
		}
		
		public ExchangeRequestMessage(int arg1)
			: this()
		{
			initExchangeRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5505;
		}
		
		public ExchangeRequestMessage initExchangeRequestMessage(int arg1 = 0)
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
			this.serializeAs_ExchangeRequestMessage(arg1);
		}
		
		public void serializeAs_ExchangeRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.exchangeType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeRequestMessage(arg1);
		}
		
		public void deserializeAs_ExchangeRequestMessage(BigEndianReader arg1)
		{
			this.exchangeType = (int)arg1.ReadByte();
		}
		
	}
}
