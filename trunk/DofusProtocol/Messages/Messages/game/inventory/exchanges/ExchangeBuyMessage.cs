using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeBuyMessage : Message
	{
		public const uint protocolId = 5774;
		internal Boolean _isInitialized = false;
		public uint objectToBuyId = 0;
		public uint quantity = 0;
		
		public ExchangeBuyMessage()
		{
		}
		
		public ExchangeBuyMessage(uint arg1, uint arg2)
			: this()
		{
			initExchangeBuyMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5774;
		}
		
		public ExchangeBuyMessage initExchangeBuyMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.@objectToBuyId = arg1;
			this.quantity = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectToBuyId = 0;
			this.quantity = 0;
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
			this.serializeAs_ExchangeBuyMessage(arg1);
		}
		
		public void serializeAs_ExchangeBuyMessage(BigEndianWriter arg1)
		{
			if ( this.@objectToBuyId < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectToBuyId + ") on element objectToBuyId.");
			}
			arg1.WriteInt((int)this.@objectToBuyId);
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element quantity.");
			}
			arg1.WriteInt((int)this.quantity);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeBuyMessage(arg1);
		}
		
		public void deserializeAs_ExchangeBuyMessage(BigEndianReader arg1)
		{
			this.@objectToBuyId = (uint)arg1.ReadInt();
			if ( this.@objectToBuyId < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectToBuyId + ") on element of ExchangeBuyMessage.objectToBuyId.");
			}
			this.quantity = (uint)arg1.ReadInt();
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element of ExchangeBuyMessage.quantity.");
			}
		}
		
	}
}
