using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeReplyTaxVendorMessage : Message
	{
		public const uint protocolId = 5787;
		internal Boolean _isInitialized = false;
		public uint objectValue = 0;
		public uint totalTaxValue = 0;
		
		public ExchangeReplyTaxVendorMessage()
		{
		}
		
		public ExchangeReplyTaxVendorMessage(uint arg1, uint arg2)
			: this()
		{
			initExchangeReplyTaxVendorMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5787;
		}
		
		public ExchangeReplyTaxVendorMessage initExchangeReplyTaxVendorMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.@objectValue = arg1;
			this.totalTaxValue = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectValue = 0;
			this.totalTaxValue = 0;
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
			this.serializeAs_ExchangeReplyTaxVendorMessage(arg1);
		}
		
		public void serializeAs_ExchangeReplyTaxVendorMessage(BigEndianWriter arg1)
		{
			if ( this.@objectValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectValue + ") on element objectValue.");
			}
			arg1.WriteInt((int)this.@objectValue);
			if ( this.totalTaxValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.totalTaxValue + ") on element totalTaxValue.");
			}
			arg1.WriteInt((int)this.totalTaxValue);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeReplyTaxVendorMessage(arg1);
		}
		
		public void deserializeAs_ExchangeReplyTaxVendorMessage(BigEndianReader arg1)
		{
			this.@objectValue = (uint)arg1.ReadInt();
			if ( this.@objectValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectValue + ") on element of ExchangeReplyTaxVendorMessage.objectValue.");
			}
			this.totalTaxValue = (uint)arg1.ReadInt();
			if ( this.totalTaxValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.totalTaxValue + ") on element of ExchangeReplyTaxVendorMessage.totalTaxValue.");
			}
		}
		
	}
}
