using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeBidHouseListMessage : Message
	{
		public const uint protocolId = 5807;
		internal Boolean _isInitialized = false;
		public uint id = 0;
		
		public ExchangeBidHouseListMessage()
		{
		}
		
		public ExchangeBidHouseListMessage(uint arg1)
			: this()
		{
			initExchangeBidHouseListMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5807;
		}
		
		public ExchangeBidHouseListMessage initExchangeBidHouseListMessage(uint arg1 = 0)
		{
			this.id = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.id = 0;
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
			this.serializeAs_ExchangeBidHouseListMessage(arg1);
		}
		
		public void serializeAs_ExchangeBidHouseListMessage(BigEndianWriter arg1)
		{
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element id.");
			}
			arg1.WriteInt((int)this.id);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeBidHouseListMessage(arg1);
		}
		
		public void deserializeAs_ExchangeBidHouseListMessage(BigEndianReader arg1)
		{
			this.id = (uint)arg1.ReadInt();
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element of ExchangeBidHouseListMessage.id.");
			}
		}
		
	}
}
