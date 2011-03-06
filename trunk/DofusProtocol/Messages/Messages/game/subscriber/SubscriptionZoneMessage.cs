using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class SubscriptionZoneMessage : Message
	{
		public const uint protocolId = 5573;
		internal Boolean _isInitialized = false;
		public Boolean active = false;
		
		public SubscriptionZoneMessage()
		{
		}
		
		public SubscriptionZoneMessage(Boolean arg1)
			: this()
		{
			initSubscriptionZoneMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5573;
		}
		
		public SubscriptionZoneMessage initSubscriptionZoneMessage(Boolean arg1 = false)
		{
			this.active = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.active = false;
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
			this.serializeAs_SubscriptionZoneMessage(arg1);
		}
		
		public void serializeAs_SubscriptionZoneMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.active);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SubscriptionZoneMessage(arg1);
		}
		
		public void deserializeAs_SubscriptionZoneMessage(BigEndianReader arg1)
		{
			this.active = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
