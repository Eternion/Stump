using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeObjectMoveKamaMessage : Message
	{
		public const uint protocolId = 5520;
		internal Boolean _isInitialized = false;
		public int quantity = 0;
		
		public ExchangeObjectMoveKamaMessage()
		{
		}
		
		public ExchangeObjectMoveKamaMessage(int arg1)
			: this()
		{
			initExchangeObjectMoveKamaMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5520;
		}
		
		public ExchangeObjectMoveKamaMessage initExchangeObjectMoveKamaMessage(int arg1 = 0)
		{
			this.quantity = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
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
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ExchangeObjectMoveKamaMessage(arg1);
		}
		
		public void serializeAs_ExchangeObjectMoveKamaMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.quantity);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeObjectMoveKamaMessage(arg1);
		}
		
		public void deserializeAs_ExchangeObjectMoveKamaMessage(BigEndianReader arg1)
		{
			this.quantity = (int)arg1.ReadInt();
		}
		
	}
}
