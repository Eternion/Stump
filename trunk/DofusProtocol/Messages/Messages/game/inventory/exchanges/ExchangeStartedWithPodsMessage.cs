using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeStartedWithPodsMessage : ExchangeStartedMessage
	{
		public const uint protocolId = 6129;
		internal Boolean _isInitialized = false;
		public int firstCharacterId = 0;
		public uint firstCharacterCurrentWeight = 0;
		public uint firstCharacterMaxWeight = 0;
		public int secondCharacterId = 0;
		public uint secondCharacterCurrentWeight = 0;
		public uint secondCharacterMaxWeight = 0;
		
		public ExchangeStartedWithPodsMessage()
		{
		}
		
		public ExchangeStartedWithPodsMessage(int arg1, int arg2, uint arg3, uint arg4, int arg5, uint arg6, uint arg7)
			: this()
		{
			initExchangeStartedWithPodsMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getMessageId()
		{
			return 6129;
		}
		
		public ExchangeStartedWithPodsMessage initExchangeStartedWithPodsMessage(int arg1 = 0, int arg2 = 0, uint arg3 = 0, uint arg4 = 0, int arg5 = 0, uint arg6 = 0, uint arg7 = 0)
		{
			base.initExchangeStartedMessage(arg1);
			this.firstCharacterId = arg2;
			this.firstCharacterCurrentWeight = arg3;
			this.firstCharacterMaxWeight = arg4;
			this.secondCharacterId = arg5;
			this.secondCharacterCurrentWeight = arg6;
			this.secondCharacterMaxWeight = arg7;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.firstCharacterId = 0;
			this.firstCharacterCurrentWeight = 0;
			this.firstCharacterMaxWeight = 0;
			this.secondCharacterId = 0;
			this.secondCharacterCurrentWeight = 0;
			this.secondCharacterMaxWeight = 0;
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
			this.serializeAs_ExchangeStartedWithPodsMessage(arg1);
		}
		
		public void serializeAs_ExchangeStartedWithPodsMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ExchangeStartedMessage(arg1);
			arg1.WriteInt((int)this.firstCharacterId);
			if ( this.firstCharacterCurrentWeight < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstCharacterCurrentWeight + ") on element firstCharacterCurrentWeight.");
			}
			arg1.WriteInt((int)this.firstCharacterCurrentWeight);
			if ( this.firstCharacterMaxWeight < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstCharacterMaxWeight + ") on element firstCharacterMaxWeight.");
			}
			arg1.WriteInt((int)this.firstCharacterMaxWeight);
			arg1.WriteInt((int)this.secondCharacterId);
			if ( this.secondCharacterCurrentWeight < 0 )
			{
				throw new Exception("Forbidden value (" + this.secondCharacterCurrentWeight + ") on element secondCharacterCurrentWeight.");
			}
			arg1.WriteInt((int)this.secondCharacterCurrentWeight);
			if ( this.secondCharacterMaxWeight < 0 )
			{
				throw new Exception("Forbidden value (" + this.secondCharacterMaxWeight + ") on element secondCharacterMaxWeight.");
			}
			arg1.WriteInt((int)this.secondCharacterMaxWeight);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeStartedWithPodsMessage(arg1);
		}
		
		public void deserializeAs_ExchangeStartedWithPodsMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.firstCharacterId = (int)arg1.ReadInt();
			this.firstCharacterCurrentWeight = (uint)arg1.ReadInt();
			if ( this.firstCharacterCurrentWeight < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstCharacterCurrentWeight + ") on element of ExchangeStartedWithPodsMessage.firstCharacterCurrentWeight.");
			}
			this.firstCharacterMaxWeight = (uint)arg1.ReadInt();
			if ( this.firstCharacterMaxWeight < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstCharacterMaxWeight + ") on element of ExchangeStartedWithPodsMessage.firstCharacterMaxWeight.");
			}
			this.secondCharacterId = (int)arg1.ReadInt();
			this.secondCharacterCurrentWeight = (uint)arg1.ReadInt();
			if ( this.secondCharacterCurrentWeight < 0 )
			{
				throw new Exception("Forbidden value (" + this.secondCharacterCurrentWeight + ") on element of ExchangeStartedWithPodsMessage.secondCharacterCurrentWeight.");
			}
			this.secondCharacterMaxWeight = (uint)arg1.ReadInt();
			if ( this.secondCharacterMaxWeight < 0 )
			{
				throw new Exception("Forbidden value (" + this.secondCharacterMaxWeight + ") on element of ExchangeStartedWithPodsMessage.secondCharacterMaxWeight.");
			}
		}
		
	}
}
