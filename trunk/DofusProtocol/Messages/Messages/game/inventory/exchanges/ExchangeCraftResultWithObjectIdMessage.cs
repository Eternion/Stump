using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeCraftResultWithObjectIdMessage : ExchangeCraftResultMessage
	{
		public const uint protocolId = 6000;
		internal Boolean _isInitialized = false;
		public uint objectGenericId = 0;
		
		public ExchangeCraftResultWithObjectIdMessage()
		{
		}
		
		public ExchangeCraftResultWithObjectIdMessage(uint arg1, uint arg2)
			: this()
		{
			initExchangeCraftResultWithObjectIdMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6000;
		}
		
		public ExchangeCraftResultWithObjectIdMessage initExchangeCraftResultWithObjectIdMessage(uint arg1 = 0, uint arg2 = 0)
		{
			base.initExchangeCraftResultMessage(arg1);
			this.@objectGenericId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.@objectGenericId = 0;
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
			this.serializeAs_ExchangeCraftResultWithObjectIdMessage(arg1);
		}
		
		public void serializeAs_ExchangeCraftResultWithObjectIdMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ExchangeCraftResultMessage(arg1);
			if ( this.@objectGenericId < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectGenericId + ") on element objectGenericId.");
			}
			arg1.WriteInt((int)this.@objectGenericId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeCraftResultWithObjectIdMessage(arg1);
		}
		
		public void deserializeAs_ExchangeCraftResultWithObjectIdMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.@objectGenericId = (uint)arg1.ReadInt();
			if ( this.@objectGenericId < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectGenericId + ") on element of ExchangeCraftResultWithObjectIdMessage.objectGenericId.");
			}
		}
		
	}
}
