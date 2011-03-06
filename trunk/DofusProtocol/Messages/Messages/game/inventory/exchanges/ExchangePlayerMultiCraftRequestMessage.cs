using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangePlayerMultiCraftRequestMessage : ExchangeRequestMessage
	{
		public const uint protocolId = 5784;
		internal Boolean _isInitialized = false;
		public uint target = 0;
		public uint skillId = 0;
		
		public ExchangePlayerMultiCraftRequestMessage()
		{
		}
		
		public ExchangePlayerMultiCraftRequestMessage(int arg1, uint arg2, uint arg3)
			: this()
		{
			initExchangePlayerMultiCraftRequestMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5784;
		}
		
		public ExchangePlayerMultiCraftRequestMessage initExchangePlayerMultiCraftRequestMessage(int arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			base.initExchangeRequestMessage(arg1);
			this.target = arg2;
			this.skillId = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.target = 0;
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ExchangePlayerMultiCraftRequestMessage(arg1);
		}
		
		public void serializeAs_ExchangePlayerMultiCraftRequestMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ExchangeRequestMessage(arg1);
			if ( this.target < 0 )
			{
				throw new Exception("Forbidden value (" + this.target + ") on element target.");
			}
			arg1.WriteInt((int)this.target);
			if ( this.skillId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillId + ") on element skillId.");
			}
			arg1.WriteInt((int)this.skillId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangePlayerMultiCraftRequestMessage(arg1);
		}
		
		public void deserializeAs_ExchangePlayerMultiCraftRequestMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.target = (uint)arg1.ReadInt();
			if ( this.target < 0 )
			{
				throw new Exception("Forbidden value (" + this.target + ") on element of ExchangePlayerMultiCraftRequestMessage.target.");
			}
			this.skillId = (uint)arg1.ReadInt();
			if ( this.skillId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillId + ") on element of ExchangePlayerMultiCraftRequestMessage.skillId.");
			}
		}
		
	}
}
