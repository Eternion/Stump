using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PartyRestrictedMessage : Message
	{
		public const uint protocolId = 6175;
		internal Boolean _isInitialized = false;
		public Boolean restricted = false;
		
		public PartyRestrictedMessage()
		{
		}
		
		public PartyRestrictedMessage(Boolean arg1)
			: this()
		{
			initPartyRestrictedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6175;
		}
		
		public PartyRestrictedMessage initPartyRestrictedMessage(Boolean arg1 = false)
		{
			this.restricted = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.restricted = false;
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
			this.serializeAs_PartyRestrictedMessage(arg1);
		}
		
		public void serializeAs_PartyRestrictedMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.restricted);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartyRestrictedMessage(arg1);
		}
		
		public void deserializeAs_PartyRestrictedMessage(BigEndianReader arg1)
		{
			this.restricted = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
