using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeStartOkNpcTradeMessage : Message
	{
		public const uint protocolId = 5785;
		internal Boolean _isInitialized = false;
		public int npcId = 0;
		
		public ExchangeStartOkNpcTradeMessage()
		{
		}
		
		public ExchangeStartOkNpcTradeMessage(int arg1)
			: this()
		{
			initExchangeStartOkNpcTradeMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5785;
		}
		
		public ExchangeStartOkNpcTradeMessage initExchangeStartOkNpcTradeMessage(int arg1 = 0)
		{
			this.npcId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.npcId = 0;
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
			this.serializeAs_ExchangeStartOkNpcTradeMessage(arg1);
		}
		
		public void serializeAs_ExchangeStartOkNpcTradeMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.npcId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeStartOkNpcTradeMessage(arg1);
		}
		
		public void deserializeAs_ExchangeStartOkNpcTradeMessage(BigEndianReader arg1)
		{
			this.npcId = (int)arg1.ReadInt();
		}
		
	}
}
