using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeCraftResultMessage : Message
	{
		public const uint protocolId = 5790;
		internal Boolean _isInitialized = false;
		public uint craftResult = 0;
		
		public ExchangeCraftResultMessage()
		{
		}
		
		public ExchangeCraftResultMessage(uint arg1)
			: this()
		{
			initExchangeCraftResultMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5790;
		}
		
		public ExchangeCraftResultMessage initExchangeCraftResultMessage(uint arg1 = 0)
		{
			this.craftResult = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.craftResult = 0;
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
			this.serializeAs_ExchangeCraftResultMessage(arg1);
		}
		
		public void serializeAs_ExchangeCraftResultMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.craftResult);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeCraftResultMessage(arg1);
		}
		
		public void deserializeAs_ExchangeCraftResultMessage(BigEndianReader arg1)
		{
			this.craftResult = (uint)arg1.ReadByte();
			if ( this.craftResult < 0 )
			{
				throw new Exception("Forbidden value (" + this.craftResult + ") on element of ExchangeCraftResultMessage.craftResult.");
			}
		}
		
	}
}
