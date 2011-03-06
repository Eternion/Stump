using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeWaitingResultMessage : Message
	{
		public const uint protocolId = 5786;
		internal Boolean _isInitialized = false;
		public Boolean bwait = false;
		
		public ExchangeWaitingResultMessage()
		{
		}
		
		public ExchangeWaitingResultMessage(Boolean arg1)
			: this()
		{
			initExchangeWaitingResultMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5786;
		}
		
		public ExchangeWaitingResultMessage initExchangeWaitingResultMessage(Boolean arg1 = false)
		{
			this.bwait = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.bwait = false;
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
			this.serializeAs_ExchangeWaitingResultMessage(arg1);
		}
		
		public void serializeAs_ExchangeWaitingResultMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.bwait);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeWaitingResultMessage(arg1);
		}
		
		public void deserializeAs_ExchangeWaitingResultMessage(BigEndianReader arg1)
		{
			this.bwait = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
