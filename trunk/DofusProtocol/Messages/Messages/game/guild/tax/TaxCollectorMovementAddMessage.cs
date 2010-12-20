using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class TaxCollectorMovementAddMessage : Message
	{
		public const uint protocolId = 5917;
		internal Boolean _isInitialized = false;
		public TaxCollectorInformations informations;
		
		public TaxCollectorMovementAddMessage()
		{
			this.informations = new TaxCollectorInformations();
		}
		
		public TaxCollectorMovementAddMessage(TaxCollectorInformations arg1)
			: this()
		{
			initTaxCollectorMovementAddMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5917;
		}
		
		public TaxCollectorMovementAddMessage initTaxCollectorMovementAddMessage(TaxCollectorInformations arg1 = null)
		{
			this.informations = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.informations = new TaxCollectorInformations();
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
			this.serializeAs_TaxCollectorMovementAddMessage(arg1);
		}
		
		public void serializeAs_TaxCollectorMovementAddMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.informations.getTypeId());
			this.informations.serialize(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorMovementAddMessage(arg1);
		}
		
		public void deserializeAs_TaxCollectorMovementAddMessage(BigEndianReader arg1)
		{
			var loc1 = (ushort)arg1.ReadUShort();
			this.informations = ProtocolTypeManager.GetInstance<TaxCollectorInformations>((uint)loc1);
			this.informations.deserialize(arg1);
		}
		
	}
}
