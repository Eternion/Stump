using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeObjectTransfertAllToInvMessage : Message
	{
		public const uint protocolId = 6032;
		
		public ExchangeObjectTransfertAllToInvMessage()
		{
		}
		
		public override uint getMessageId()
		{
			return 6032;
		}
		
		public ExchangeObjectTransfertAllToInvMessage initExchangeObjectTransfertAllToInvMessage()
		{
			return this;
		}
		
		public override void reset()
		{
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
		}
		
		public void serializeAs_ExchangeObjectTransfertAllToInvMessage(BigEndianWriter arg1)
		{
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
		}
		
		public void deserializeAs_ExchangeObjectTransfertAllToInvMessage(BigEndianReader arg1)
		{
		}
		
	}
}
