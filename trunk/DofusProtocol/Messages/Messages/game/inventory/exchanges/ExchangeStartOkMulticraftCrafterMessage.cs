using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeStartOkMulticraftCrafterMessage : Message
	{
		public const uint protocolId = 5818;
		internal Boolean _isInitialized = false;
		public uint maxCase = 0;
		public uint skillId = 0;
		
		public ExchangeStartOkMulticraftCrafterMessage()
		{
		}
		
		public ExchangeStartOkMulticraftCrafterMessage(uint arg1, uint arg2)
			: this()
		{
			initExchangeStartOkMulticraftCrafterMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5818;
		}
		
		public ExchangeStartOkMulticraftCrafterMessage initExchangeStartOkMulticraftCrafterMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.maxCase = arg1;
			this.skillId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.maxCase = 0;
			this.skillId = 0;
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
			this.serializeAs_ExchangeStartOkMulticraftCrafterMessage(arg1);
		}
		
		public void serializeAs_ExchangeStartOkMulticraftCrafterMessage(BigEndianWriter arg1)
		{
			if ( this.maxCase < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxCase + ") on element maxCase.");
			}
			arg1.WriteByte((byte)this.maxCase);
			if ( this.skillId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillId + ") on element skillId.");
			}
			arg1.WriteInt((int)this.skillId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeStartOkMulticraftCrafterMessage(arg1);
		}
		
		public void deserializeAs_ExchangeStartOkMulticraftCrafterMessage(BigEndianReader arg1)
		{
			this.maxCase = (uint)arg1.ReadByte();
			if ( this.maxCase < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxCase + ") on element of ExchangeStartOkMulticraftCrafterMessage.maxCase.");
			}
			this.skillId = (uint)arg1.ReadInt();
			if ( this.skillId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillId + ") on element of ExchangeStartOkMulticraftCrafterMessage.skillId.");
			}
		}
		
	}
}
