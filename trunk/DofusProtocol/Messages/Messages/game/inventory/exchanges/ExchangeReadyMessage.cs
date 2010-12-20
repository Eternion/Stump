using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeReadyMessage : Message
	{
		public const uint protocolId = 5511;
		internal Boolean _isInitialized = false;
		public Boolean ready = false;
		
		public ExchangeReadyMessage()
		{
		}
		
		public ExchangeReadyMessage(Boolean arg1)
			: this()
		{
			initExchangeReadyMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5511;
		}
		
		public ExchangeReadyMessage initExchangeReadyMessage(Boolean arg1 = false)
		{
			this.ready = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.ready = false;
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
			this.serializeAs_ExchangeReadyMessage(arg1);
		}
		
		public void serializeAs_ExchangeReadyMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.ready);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeReadyMessage(arg1);
		}
		
		public void deserializeAs_ExchangeReadyMessage(BigEndianReader arg1)
		{
			this.ready = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
