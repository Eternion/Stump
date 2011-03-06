using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeBidHouseSearchMessage : Message
	{
		public const uint protocolId = 5806;
		internal Boolean _isInitialized = false;
		public uint type = 0;
		public uint genId = 0;
		
		public ExchangeBidHouseSearchMessage()
		{
		}
		
		public ExchangeBidHouseSearchMessage(uint arg1, uint arg2)
			: this()
		{
			initExchangeBidHouseSearchMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5806;
		}
		
		public ExchangeBidHouseSearchMessage initExchangeBidHouseSearchMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.type = arg1;
			this.genId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.type = 0;
			this.genId = 0;
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
			this.serializeAs_ExchangeBidHouseSearchMessage(arg1);
		}
		
		public void serializeAs_ExchangeBidHouseSearchMessage(BigEndianWriter arg1)
		{
			if ( this.type < 0 )
			{
				throw new Exception("Forbidden value (" + this.type + ") on element type.");
			}
			arg1.WriteInt((int)this.type);
			if ( this.genId < 0 )
			{
				throw new Exception("Forbidden value (" + this.genId + ") on element genId.");
			}
			arg1.WriteInt((int)this.genId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeBidHouseSearchMessage(arg1);
		}
		
		public void deserializeAs_ExchangeBidHouseSearchMessage(BigEndianReader arg1)
		{
			this.type = (uint)arg1.ReadInt();
			if ( this.type < 0 )
			{
				throw new Exception("Forbidden value (" + this.type + ") on element of ExchangeBidHouseSearchMessage.type.");
			}
			this.genId = (uint)arg1.ReadInt();
			if ( this.genId < 0 )
			{
				throw new Exception("Forbidden value (" + this.genId + ") on element of ExchangeBidHouseSearchMessage.genId.");
			}
		}
		
	}
}
