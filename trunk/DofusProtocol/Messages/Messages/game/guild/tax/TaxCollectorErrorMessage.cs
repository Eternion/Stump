using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class TaxCollectorErrorMessage : Message
	{
		public const uint protocolId = 5634;
		internal Boolean _isInitialized = false;
		public int reason = 0;
		
		public TaxCollectorErrorMessage()
		{
		}
		
		public TaxCollectorErrorMessage(int arg1)
			: this()
		{
			initTaxCollectorErrorMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5634;
		}
		
		public TaxCollectorErrorMessage initTaxCollectorErrorMessage(int arg1 = 0)
		{
			this.reason = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.reason = 0;
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
			this.serializeAs_TaxCollectorErrorMessage(arg1);
		}
		
		public void serializeAs_TaxCollectorErrorMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.reason);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorErrorMessage(arg1);
		}
		
		public void deserializeAs_TaxCollectorErrorMessage(BigEndianReader arg1)
		{
			this.reason = (int)arg1.ReadByte();
		}
		
	}
}
