using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeReplayCountModifiedMessage : Message
	{
		public const uint protocolId = 6023;
		internal Boolean _isInitialized = false;
		public int count = 0;
		
		public ExchangeReplayCountModifiedMessage()
		{
		}
		
		public ExchangeReplayCountModifiedMessage(int arg1)
			: this()
		{
			initExchangeReplayCountModifiedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6023;
		}
		
		public ExchangeReplayCountModifiedMessage initExchangeReplayCountModifiedMessage(int arg1 = 0)
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
			this.serializeAs_ExchangeReplayCountModifiedMessage(arg1);
		}
		
		public void serializeAs_ExchangeReplayCountModifiedMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.count);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeReplayCountModifiedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeReplayCountModifiedMessage(BigEndianReader arg1)
		{
			this.count = (int)arg1.ReadInt();
		}
		
	}
}
