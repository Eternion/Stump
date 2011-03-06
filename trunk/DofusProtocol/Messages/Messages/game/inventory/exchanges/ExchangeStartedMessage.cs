using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeStartedMessage : Message
	{
		public const uint protocolId = 5512;
		internal Boolean _isInitialized = false;
		public int exchangeType = 0;
		
		public ExchangeStartedMessage()
		{
		}
		
		public ExchangeStartedMessage(int arg1)
			: this()
		{
			initExchangeStartedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5512;
		}
		
		public ExchangeStartedMessage initExchangeStartedMessage(int arg1 = 0)
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
			this.serializeAs_ExchangeStartedMessage(arg1);
		}
		
		public void serializeAs_ExchangeStartedMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.exchangeType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeStartedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeStartedMessage(BigEndianReader arg1)
		{
			this.exchangeType = (int)arg1.ReadByte();
		}
		
	}
}
