using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeMountPaddockAddMessage : Message
	{
		public const uint protocolId = 6049;
		internal Boolean _isInitialized = false;
		public MountClientData mountDescription;
		
		public ExchangeMountPaddockAddMessage()
		{
			this.mountDescription = new MountClientData();
		}
		
		public ExchangeMountPaddockAddMessage(MountClientData arg1)
			: this()
		{
			initExchangeMountPaddockAddMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6049;
		}
		
		public ExchangeMountPaddockAddMessage initExchangeMountPaddockAddMessage(MountClientData arg1 = null)
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
			this.serializeAs_ExchangeMountPaddockAddMessage(arg1);
		}
		
		public void serializeAs_ExchangeMountPaddockAddMessage(BigEndianWriter arg1)
		{
			this.mountDescription.serializeAs_MountClientData(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeMountPaddockAddMessage(arg1);
		}
		
		public void deserializeAs_ExchangeMountPaddockAddMessage(BigEndianReader arg1)
		{
			this.mountDescription = new MountClientData();
			this.mountDescription.deserialize(arg1);
		}
		
	}
}
