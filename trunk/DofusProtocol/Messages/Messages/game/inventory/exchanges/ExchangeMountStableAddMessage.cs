using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeMountStableAddMessage : Message
	{
		public const uint protocolId = 5971;
		internal Boolean _isInitialized = false;
		public MountClientData mountDescription;
		
		public ExchangeMountStableAddMessage()
		{
			this.mountDescription = new MountClientData();
		}
		
		public ExchangeMountStableAddMessage(MountClientData arg1)
			: this()
		{
			initExchangeMountStableAddMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5971;
		}
		
		public ExchangeMountStableAddMessage initExchangeMountStableAddMessage(MountClientData arg1 = null)
		{
			this.mountDescription = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.mountDescription = new MountClientData();
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
			this.serializeAs_ExchangeMountStableAddMessage(arg1);
		}
		
		public void serializeAs_ExchangeMountStableAddMessage(BigEndianWriter arg1)
		{
			this.mountDescription.serializeAs_MountClientData(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeMountStableAddMessage(arg1);
		}
		
		public void deserializeAs_ExchangeMountStableAddMessage(BigEndianReader arg1)
		{
			this.mountDescription = new MountClientData();
			this.mountDescription.deserialize(arg1);
		}
		
	}
}
