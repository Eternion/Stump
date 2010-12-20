using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeReplayMessage : Message
	{
		public const uint protocolId = 6002;
		internal Boolean _isInitialized = false;
		public int count = 0;
		
		public ExchangeReplayMessage()
		{
		}
		
		public ExchangeReplayMessage(int arg1)
			: this()
		{
			initExchangeReplayMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6002;
		}
		
		public ExchangeReplayMessage initExchangeReplayMessage(int arg1 = 0)
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
			this.serializeAs_ExchangeReplayMessage(arg1);
		}
		
		public void serializeAs_ExchangeReplayMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.count);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeReplayMessage(arg1);
		}
		
		public void deserializeAs_ExchangeReplayMessage(BigEndianReader arg1)
		{
			this.count = (int)arg1.ReadInt();
		}
		
	}
}
