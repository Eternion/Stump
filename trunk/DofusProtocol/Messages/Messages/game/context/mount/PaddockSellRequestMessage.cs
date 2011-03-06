using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PaddockSellRequestMessage : Message
	{
		public const uint protocolId = 5953;
		internal Boolean _isInitialized = false;
		public uint price = 0;
		
		public PaddockSellRequestMessage()
		{
		}
		
		public PaddockSellRequestMessage(uint arg1)
			: this()
		{
			initPaddockSellRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5953;
		}
		
		public PaddockSellRequestMessage initPaddockSellRequestMessage(uint arg1 = 0)
		{
			this.price = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.price = 0;
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
			this.serializeAs_PaddockSellRequestMessage(arg1);
		}
		
		public void serializeAs_PaddockSellRequestMessage(BigEndianWriter arg1)
		{
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element price.");
			}
			arg1.WriteInt((int)this.price);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockSellRequestMessage(arg1);
		}
		
		public void deserializeAs_PaddockSellRequestMessage(BigEndianReader arg1)
		{
			this.price = (uint)arg1.ReadInt();
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element of PaddockSellRequestMessage.price.");
			}
		}
		
	}
}
