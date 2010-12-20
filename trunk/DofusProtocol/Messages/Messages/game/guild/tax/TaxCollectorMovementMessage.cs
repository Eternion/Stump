using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class TaxCollectorMovementMessage : Message
	{
		public const uint protocolId = 5633;
		internal Boolean _isInitialized = false;
		public Boolean hireOrFire = false;
		public TaxCollectorBasicInformations basicInfos;
		public String playerName = "";
		
		public TaxCollectorMovementMessage()
		{
			this.basicInfos = new TaxCollectorBasicInformations();
		}
		
		public TaxCollectorMovementMessage(Boolean arg1, TaxCollectorBasicInformations arg2, String arg3)
			: this()
		{
			initTaxCollectorMovementMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5633;
		}
		
		public TaxCollectorMovementMessage initTaxCollectorMovementMessage(Boolean arg1 = false, TaxCollectorBasicInformations arg2 = null, String arg3 = "")
		{
			this.hireOrFire = arg1;
			this.basicInfos = arg2;
			this.playerName = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.hireOrFire = false;
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
			this.serializeAs_TaxCollectorMovementMessage(arg1);
		}
		
		public void serializeAs_TaxCollectorMovementMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.hireOrFire);
			this.basicInfos.serializeAs_TaxCollectorBasicInformations(arg1);
			arg1.WriteUTF((string)this.playerName);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorMovementMessage(arg1);
		}
		
		public void deserializeAs_TaxCollectorMovementMessage(BigEndianReader arg1)
		{
			this.hireOrFire = (Boolean)arg1.ReadBoolean();
			this.basicInfos = new TaxCollectorBasicInformations();
			this.basicInfos.deserialize(arg1);
			this.playerName = (String)arg1.ReadUTF();
		}
		
	}
}
