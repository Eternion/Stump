using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeMountStableBornAddMessage : ExchangeMountStableAddMessage
	{
		public const uint protocolId = 5966;
		internal Boolean _isInitialized = false;
		
		public ExchangeMountStableBornAddMessage()
		{
		}
		
		public ExchangeMountStableBornAddMessage(MountClientData arg1)
			: this()
		{
			initExchangeMountStableBornAddMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5966;
		}
		
		public ExchangeMountStableBornAddMessage initExchangeMountStableBornAddMessage(MountClientData arg1 = null)
		{
			base.initExchangeMountStableAddMessage(arg1);
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
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
			this.serializeAs_ExchangeMountStableBornAddMessage(arg1);
		}
		
		public void serializeAs_ExchangeMountStableBornAddMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ExchangeMountStableAddMessage(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeMountStableBornAddMessage(arg1);
		}
		
		public void deserializeAs_ExchangeMountStableBornAddMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
		}
		
	}
}
