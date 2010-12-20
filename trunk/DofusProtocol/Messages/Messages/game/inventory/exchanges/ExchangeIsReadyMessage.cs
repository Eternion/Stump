using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeIsReadyMessage : Message
	{
		public const uint protocolId = 5509;
		internal Boolean _isInitialized = false;
		public uint id = 0;
		public Boolean ready = false;
		
		public ExchangeIsReadyMessage()
		{
		}
		
		public ExchangeIsReadyMessage(uint arg1, Boolean arg2)
			: this()
		{
			initExchangeIsReadyMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5509;
		}
		
		public ExchangeIsReadyMessage initExchangeIsReadyMessage(uint arg1 = 0, Boolean arg2 = false)
		{
			this.id = arg1;
			this.ready = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.id = 0;
			this.ready = false;
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
			this.serializeAs_ExchangeIsReadyMessage(arg1);
		}
		
		public void serializeAs_ExchangeIsReadyMessage(BigEndianWriter arg1)
		{
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element id.");
			}
			arg1.WriteInt((int)this.id);
			arg1.WriteBoolean(this.ready);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeIsReadyMessage(arg1);
		}
		
		public void deserializeAs_ExchangeIsReadyMessage(BigEndianReader arg1)
		{
			this.id = (uint)arg1.ReadInt();
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element of ExchangeIsReadyMessage.id.");
			}
			this.ready = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
