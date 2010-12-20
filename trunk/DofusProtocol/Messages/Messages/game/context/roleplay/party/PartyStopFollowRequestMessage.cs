using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PartyStopFollowRequestMessage : Message
	{
		public const uint protocolId = 5574;
		
		public PartyStopFollowRequestMessage()
		{
		}
		
		public override uint getMessageId()
		{
			return 5574;
		}
		
		public PartyStopFollowRequestMessage initPartyStopFollowRequestMessage()
		{
			return this;
		}
		
		public override void reset()
		{
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
		}
		
		public void serializeAs_PartyStopFollowRequestMessage(BigEndianWriter arg1)
		{
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
		}
		
		public void deserializeAs_PartyStopFollowRequestMessage(BigEndianReader arg1)
		{
		}
		
	}
}
