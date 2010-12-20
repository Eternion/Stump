using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeCraftInformationObjectMessage : ExchangeCraftResultWithObjectIdMessage
	{
		public const uint protocolId = 5794;
		internal Boolean _isInitialized = false;
		public uint playerId = 0;
		
		public ExchangeCraftInformationObjectMessage()
		{
		}
		
		public ExchangeCraftInformationObjectMessage(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initExchangeCraftInformationObjectMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5794;
		}
		
		public ExchangeCraftInformationObjectMessage initExchangeCraftInformationObjectMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			base.initExchangeCraftResultWithObjectIdMessage(arg1, arg2);
			this.playerId = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.playerId = 0;
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
			this.serializeAs_ExchangeCraftInformationObjectMessage(arg1);
		}
		
		public void serializeAs_ExchangeCraftInformationObjectMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ExchangeCraftResultWithObjectIdMessage(arg1);
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element playerId.");
			}
			arg1.WriteInt((int)this.playerId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeCraftInformationObjectMessage(arg1);
		}
		
		public void deserializeAs_ExchangeCraftInformationObjectMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.playerId = (uint)arg1.ReadInt();
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element of ExchangeCraftInformationObjectMessage.playerId.");
			}
		}
		
	}
}
