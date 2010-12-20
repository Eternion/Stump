using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage : Message
	{
		public const uint protocolId = 6021;
		internal Boolean _isInitialized = false;
		public Boolean allow = false;
		
		public ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage()
		{
		}
		
		public ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage(Boolean arg1)
			: this()
		{
			initExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6021;
		}
		
		public ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage initExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage(Boolean arg1 = false)
		{
			this.allow = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.allow = false;
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
			this.serializeAs_ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage(arg1);
		}
		
		public void serializeAs_ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.allow);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage(arg1);
		}
		
		public void deserializeAs_ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage(BigEndianReader arg1)
		{
			this.allow = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
