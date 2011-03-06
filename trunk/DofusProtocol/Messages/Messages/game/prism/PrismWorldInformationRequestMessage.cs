using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PrismWorldInformationRequestMessage : Message
	{
		public const uint protocolId = 5985;
		internal Boolean _isInitialized = false;
		public Boolean join = false;
		
		public PrismWorldInformationRequestMessage()
		{
		}
		
		public PrismWorldInformationRequestMessage(Boolean arg1)
			: this()
		{
			initPrismWorldInformationRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5985;
		}
		
		public PrismWorldInformationRequestMessage initPrismWorldInformationRequestMessage(Boolean arg1 = false)
		{
			this.join = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.join = false;
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
			this.serializeAs_PrismWorldInformationRequestMessage(arg1);
		}
		
		public void serializeAs_PrismWorldInformationRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.join);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismWorldInformationRequestMessage(arg1);
		}
		
		public void deserializeAs_PrismWorldInformationRequestMessage(BigEndianReader arg1)
		{
			this.join = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
