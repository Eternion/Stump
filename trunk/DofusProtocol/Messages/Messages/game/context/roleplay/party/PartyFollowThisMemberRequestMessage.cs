using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PartyFollowThisMemberRequestMessage : PartyFollowMemberRequestMessage
	{
		public const uint protocolId = 5588;
		internal Boolean _isInitialized = false;
		public Boolean enabled = false;
		
		public PartyFollowThisMemberRequestMessage()
		{
		}
		
		public PartyFollowThisMemberRequestMessage(uint arg1, Boolean arg2)
			: this()
		{
			initPartyFollowThisMemberRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5588;
		}
		
		public PartyFollowThisMemberRequestMessage initPartyFollowThisMemberRequestMessage(uint arg1 = 0, Boolean arg2 = false)
		{
			base.initPartyFollowMemberRequestMessage(arg1);
			this.enabled = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.enabled = false;
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PartyFollowThisMemberRequestMessage(arg1);
		}
		
		public void serializeAs_PartyFollowThisMemberRequestMessage(BigEndianWriter arg1)
		{
			base.serializeAs_PartyFollowMemberRequestMessage(arg1);
			arg1.WriteBoolean(this.enabled);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartyFollowThisMemberRequestMessage(arg1);
		}
		
		public void deserializeAs_PartyFollowThisMemberRequestMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.enabled = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
