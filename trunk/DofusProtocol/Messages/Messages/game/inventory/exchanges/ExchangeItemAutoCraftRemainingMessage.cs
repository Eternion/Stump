using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeItemAutoCraftRemainingMessage : Message
	{
		public const uint protocolId = 6015;
		internal Boolean _isInitialized = false;
		public uint count = 0;
		
		public ExchangeItemAutoCraftRemainingMessage()
		{
		}
		
		public ExchangeItemAutoCraftRemainingMessage(uint arg1)
			: this()
		{
			initExchangeItemAutoCraftRemainingMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6015;
		}
		
		public ExchangeItemAutoCraftRemainingMessage initExchangeItemAutoCraftRemainingMessage(uint arg1 = 0)
		{
			this.count = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.count = 0;
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
			this.serializeAs_ExchangeItemAutoCraftRemainingMessage(arg1);
		}
		
		public void serializeAs_ExchangeItemAutoCraftRemainingMessage(BigEndianWriter arg1)
		{
			if ( this.count < 0 )
			{
				throw new Exception("Forbidden value (" + this.count + ") on element count.");
			}
			arg1.WriteInt((int)this.count);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeItemAutoCraftRemainingMessage(arg1);
		}
		
		public void deserializeAs_ExchangeItemAutoCraftRemainingMessage(BigEndianReader arg1)
		{
			this.count = (uint)arg1.ReadInt();
			if ( this.count < 0 )
			{
				throw new Exception("Forbidden value (" + this.count + ") on element of ExchangeItemAutoCraftRemainingMessage.count.");
			}
		}
		
	}
}
