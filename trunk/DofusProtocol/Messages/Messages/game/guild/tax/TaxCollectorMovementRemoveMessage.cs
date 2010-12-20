using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class TaxCollectorMovementRemoveMessage : Message
	{
		public const uint protocolId = 5915;
		internal Boolean _isInitialized = false;
		public int collectorId = 0;
		
		public TaxCollectorMovementRemoveMessage()
		{
		}
		
		public TaxCollectorMovementRemoveMessage(int arg1)
			: this()
		{
			initTaxCollectorMovementRemoveMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5915;
		}
		
		public TaxCollectorMovementRemoveMessage initTaxCollectorMovementRemoveMessage(int arg1 = 0)
		{
			this.collectorId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.collectorId = 0;
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
			this.serializeAs_TaxCollectorMovementRemoveMessage(arg1);
		}
		
		public void serializeAs_TaxCollectorMovementRemoveMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.collectorId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorMovementRemoveMessage(arg1);
		}
		
		public void deserializeAs_TaxCollectorMovementRemoveMessage(BigEndianReader arg1)
		{
			this.collectorId = (int)arg1.ReadInt();
		}
		
	}
}
