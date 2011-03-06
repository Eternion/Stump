using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeRemovedPaymentForCraftMessage : Message
	{
		public const uint protocolId = 6031;
		internal Boolean _isInitialized = false;
		public Boolean onlySuccess = false;
		public uint objectUID = 0;
		
		public ExchangeRemovedPaymentForCraftMessage()
		{
		}
		
		public ExchangeRemovedPaymentForCraftMessage(Boolean arg1, uint arg2)
			: this()
		{
			initExchangeRemovedPaymentForCraftMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6031;
		}
		
		public ExchangeRemovedPaymentForCraftMessage initExchangeRemovedPaymentForCraftMessage(Boolean arg1 = false, uint arg2 = 0)
		{
			this.onlySuccess = arg1;
			this.@objectUID = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.onlySuccess = false;
			this.@objectUID = 0;
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
			this.serializeAs_ExchangeRemovedPaymentForCraftMessage(arg1);
		}
		
		public void serializeAs_ExchangeRemovedPaymentForCraftMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.onlySuccess);
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element objectUID.");
			}
			arg1.WriteInt((int)this.@objectUID);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeRemovedPaymentForCraftMessage(arg1);
		}
		
		public void deserializeAs_ExchangeRemovedPaymentForCraftMessage(BigEndianReader arg1)
		{
			this.onlySuccess = (Boolean)arg1.ReadBoolean();
			this.@objectUID = (uint)arg1.ReadInt();
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element of ExchangeRemovedPaymentForCraftMessage.objectUID.");
			}
		}
		
	}
}
