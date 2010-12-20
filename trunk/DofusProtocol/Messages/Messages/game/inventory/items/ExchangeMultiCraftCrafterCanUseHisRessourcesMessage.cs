using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeMultiCraftCrafterCanUseHisRessourcesMessage : Message
	{
		public const uint protocolId = 6020;
		internal Boolean _isInitialized = false;
		public Boolean allowed = false;
		
		public ExchangeMultiCraftCrafterCanUseHisRessourcesMessage()
		{
		}
		
		public ExchangeMultiCraftCrafterCanUseHisRessourcesMessage(Boolean arg1)
			: this()
		{
			initExchangeMultiCraftCrafterCanUseHisRessourcesMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6020;
		}
		
		public ExchangeMultiCraftCrafterCanUseHisRessourcesMessage initExchangeMultiCraftCrafterCanUseHisRessourcesMessage(Boolean arg1 = false)
		{
			this.allowed = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.allowed = false;
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
			this.serializeAs_ExchangeMultiCraftCrafterCanUseHisRessourcesMessage(arg1);
		}
		
		public void serializeAs_ExchangeMultiCraftCrafterCanUseHisRessourcesMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.allowed);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeMultiCraftCrafterCanUseHisRessourcesMessage(arg1);
		}
		
		public void deserializeAs_ExchangeMultiCraftCrafterCanUseHisRessourcesMessage(BigEndianReader arg1)
		{
			this.allowed = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
