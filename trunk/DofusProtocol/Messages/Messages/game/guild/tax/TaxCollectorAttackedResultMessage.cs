using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class TaxCollectorAttackedResultMessage : Message
	{
		public const uint protocolId = 5635;
		internal Boolean _isInitialized = false;
		public Boolean deadOrAlive = false;
		public TaxCollectorBasicInformations basicInfos;
		
		public TaxCollectorAttackedResultMessage()
		{
			this.basicInfos = new TaxCollectorBasicInformations();
		}
		
		public TaxCollectorAttackedResultMessage(Boolean arg1, TaxCollectorBasicInformations arg2)
			: this()
		{
			initTaxCollectorAttackedResultMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5635;
		}
		
		public TaxCollectorAttackedResultMessage initTaxCollectorAttackedResultMessage(Boolean arg1 = false, TaxCollectorBasicInformations arg2 = null)
		{
			this.deadOrAlive = arg1;
			this.basicInfos = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.deadOrAlive = false;
			this.basicInfos = new TaxCollectorBasicInformations();
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
			this.serializeAs_TaxCollectorAttackedResultMessage(arg1);
		}
		
		public void serializeAs_TaxCollectorAttackedResultMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.deadOrAlive);
			this.basicInfos.serializeAs_TaxCollectorBasicInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorAttackedResultMessage(arg1);
		}
		
		public void deserializeAs_TaxCollectorAttackedResultMessage(BigEndianReader arg1)
		{
			this.deadOrAlive = (Boolean)arg1.ReadBoolean();
			this.basicInfos = new TaxCollectorBasicInformations();
			this.basicInfos.deserialize(arg1);
		}
		
	}
}
