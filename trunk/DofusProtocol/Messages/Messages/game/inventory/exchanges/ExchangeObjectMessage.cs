using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeObjectMessage : Message
	{
		public const uint protocolId = 5515;
		internal Boolean _isInitialized = false;
		public Boolean remote = false;
		
		public ExchangeObjectMessage()
		{
		}
		
		public ExchangeObjectMessage(Boolean arg1)
			: this()
		{
			initExchangeObjectMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5515;
		}
		
		public ExchangeObjectMessage initExchangeObjectMessage(Boolean arg1 = false)
		{
			this.remote = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.remote = false;
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
			this.serializeAs_ExchangeObjectMessage(arg1);
		}
		
		public void serializeAs_ExchangeObjectMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.remote);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeObjectMessage(arg1);
		}
		
		public void deserializeAs_ExchangeObjectMessage(BigEndianReader arg1)
		{
			this.remote = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
