using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PaddockSellBuyDialogMessage : Message
	{
		public const uint protocolId = 6018;
		internal Boolean _isInitialized = false;
		public Boolean bsell = false;
		public uint ownerId = 0;
		public uint price = 0;
		
		public PaddockSellBuyDialogMessage()
		{
		}
		
		public PaddockSellBuyDialogMessage(Boolean arg1, uint arg2, uint arg3)
			: this()
		{
			initPaddockSellBuyDialogMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6018;
		}
		
		public PaddockSellBuyDialogMessage initPaddockSellBuyDialogMessage(Boolean arg1 = false, uint arg2 = 0, uint arg3 = 0)
		{
			this.bsell = arg1;
			this.ownerId = arg2;
			this.price = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.bsell = false;
			this.ownerId = 0;
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
			this.serializeAs_PaddockSellBuyDialogMessage(arg1);
		}
		
		public void serializeAs_PaddockSellBuyDialogMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.bsell);
			if ( this.ownerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.ownerId + ") on element ownerId.");
			}
			arg1.WriteInt((int)this.ownerId);
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element price.");
			}
			arg1.WriteInt((int)this.price);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockSellBuyDialogMessage(arg1);
		}
		
		public void deserializeAs_PaddockSellBuyDialogMessage(BigEndianReader arg1)
		{
			this.bsell = (Boolean)arg1.ReadBoolean();
			this.ownerId = (uint)arg1.ReadInt();
			if ( this.ownerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.ownerId + ") on element of PaddockSellBuyDialogMessage.ownerId.");
			}
			this.price = (uint)arg1.ReadInt();
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element of PaddockSellBuyDialogMessage.price.");
			}
		}
		
	}
}
