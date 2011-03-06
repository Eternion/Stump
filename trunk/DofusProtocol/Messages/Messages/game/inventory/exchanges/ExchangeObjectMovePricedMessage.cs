using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeObjectMovePricedMessage : ExchangeObjectMoveMessage
	{
		public const uint protocolId = 5514;
		internal Boolean _isInitialized = false;
		public int price = 0;
		
		public ExchangeObjectMovePricedMessage()
		{
		}
		
		public ExchangeObjectMovePricedMessage(uint arg1, int arg2, int arg3)
			: this()
		{
			initExchangeObjectMovePricedMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5514;
		}
		
		public ExchangeObjectMovePricedMessage initExchangeObjectMovePricedMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0)
		{
			base.initExchangeObjectMoveMessage(arg1, arg2);
			this.price = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.price = 0;
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
			this.serializeAs_ExchangeObjectMovePricedMessage(arg1);
		}
		
		public void serializeAs_ExchangeObjectMovePricedMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ExchangeObjectMoveMessage(arg1);
			arg1.WriteInt((int)this.price);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeObjectMovePricedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeObjectMovePricedMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.price = (int)arg1.ReadInt();
		}
		
	}
}
