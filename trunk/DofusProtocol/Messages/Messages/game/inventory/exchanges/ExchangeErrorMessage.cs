using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeErrorMessage : Message
	{
		public const uint protocolId = 5513;
		internal Boolean _isInitialized = false;
		public int errorType = 0;
		
		public ExchangeErrorMessage()
		{
		}
		
		public ExchangeErrorMessage(int arg1)
			: this()
		{
			initExchangeErrorMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5513;
		}
		
		public ExchangeErrorMessage initExchangeErrorMessage(int arg1 = 0)
		{
			this.errorType = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.errorType = 0;
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
			this.serializeAs_ExchangeErrorMessage(arg1);
		}
		
		public void serializeAs_ExchangeErrorMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.errorType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeErrorMessage(arg1);
		}
		
		public void deserializeAs_ExchangeErrorMessage(BigEndianReader arg1)
		{
			this.errorType = (int)arg1.ReadByte();
		}
		
	}
}
